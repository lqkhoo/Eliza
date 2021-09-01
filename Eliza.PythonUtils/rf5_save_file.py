from __future__ import annotations
from typing import Dict, Tuple
import os


from construct import Container

from crypto import Crypto
from model import *


class Rf5_Save_File(object):

    def __init__(self, version: str, header: bytes, decrypted_data: bytes, footer: bytes):

        self._version: str = version
        self._initial_header: bytes = header
        self._initial_decrypted_data: bytes = decrypted_data
        self._initial_footer: bytes = footer

        self.model: IModel = None
        self.header: Container = None
        self.data: Container = None
        self.footer: Container = None

        self.model_map: Dict[str, IModel] = {
            '1.0.2': Model_JP_1_0_2,
            '1.0.6': Model_JP_1_0_6,
            '1.0.7': Model_JP_1_0_7
        }

        if self._version in self.model_map:
            self.model = self.model_map[self._version]() # Make sure to init

        if self.model is None:
            raise NotImplementedError("Unknown / unsupported version {}".format(self._version))
        else:
            self.header = self.model.Struct_RF5_HEADER.parse(header)
            self.data   = self.model.Struct_RF5_DATA.parse(decrypted_data)
            self.footer = self.model.Struct_RF5_FOOTER.parse(footer)


    @staticmethod
    def _decrypt_file(path: str) -> Tuple[bytes, bytes, bytes]:
        """Read in an encrypted file and returns the header,
        decrypted data, and footer separately.
        """

        header_nbytes = 0x20
        footer_nbytes = 0x10

        total_nbytes = os.path.getsize(path)
        with open(path, 'rb') as f:
            data_length = total_nbytes - header_nbytes - footer_nbytes
            header = f.read(header_nbytes)
            encrypted_data = f.read(data_length)
            footer = f.read(footer_nbytes)
            decrypted_data = Crypto.decrypt(encrypted_data)
            # pprint_bytes(decrypted_data)
        return header, decrypted_data, footer

    @staticmethod
    def just_decrypt_file(path: str, output_path: str) -> None:
        """Bypass serialization entirely and don't construct a model.
        Just do decryption for our hex editor"""
        header, decrypted_data, footer = Rf5_Save_File._decrypt_file(path)
        with open(output_path, 'wb') as f:
            f.write(b''.join([
                header,
                decrypted_data,
                footer
            ]))


    @staticmethod
    def from_encrypted_file(version: str, path: str) -> Rf5_Save_File:
        header, decrypted_data, footer = Rf5_Save_File._decrypt_file(path)    
        return Rf5_Save_File(version, header, decrypted_data, footer)



    def to_decrypted_file(self, output_path: str) -> None:
        header = self.model.Struct_RF5_HEADER.build(self.header)
        data = self.model.Struct_RF5_DATA.build(self.data)
        if len(data) < len(self._initial_decrypted_data):
            # Faithfully reproduce the junk data from rijndael's zero padding
            junk_len = len(self._initial_decrypted_data) - len(data)
            junk_data = self._initial_decrypted_data[len(data):len(data)+junk_len]
            data = b''.join([data, junk_data])
        footer = self.model.Struct_RF5_FOOTER.build(self.footer)
        with open(output_path, 'wb') as f:
            f.write(b''.join([header, data, footer]))


    def to_encrypted_file(self, output_path: str) -> None:
        header = self.model.Struct_RF5_HEADER.build(self.header)
        data = self.model.Struct_RF5_DATA.build(self.data)

        body_length = len(header) + len(data)

        if len(data) < len(self._initial_decrypted_data):
            # Junk data is from uninitialized segment of buffer.
            # Although it doesn't affect the data, it's needed to reproduce
            # the original footer, as it's included in the checksum.
            junk_len = len(self._initial_decrypted_data) - len(data)
            junk_data = self._initial_decrypted_data[len(data):len(data)+junk_len]
            extended_data = b''.join([data, junk_data])

        with open(output_path, 'wb') as f:
            f.write(header)
            f.write(extended_data)
            f.seek(0x20)

            encrypted_data = Crypto.encrypt(extended_data)

            padded_length = ((body_length - 0x20 + 0x1f) & ~0x1f) + 0x20 # Ensures multiple of 32 (0x20)
            body_data = b''.join([header, encrypted_data])
            sum = Crypto.checksum(body_data)
            footer = self.model.Struct_RF5_FOOTER.build(dict(
                bodyLength=body_length,
                length=padded_length,
                sum=sum,
                blank=self.footer.blank # Instead of 0x0, we just copy back this field as we found it
            ))

            f.write(encrypted_data)
            f.write(footer)
            # Output file is arbitrary. If file has shrunk, truncate leftover
            # junk data / previous footer / whatever the previous file might be.
            f.truncate()
