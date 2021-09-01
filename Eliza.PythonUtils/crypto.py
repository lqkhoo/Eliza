from __future__ import annotations

from py3rijndael import RijndaelCbc, ZeroPadding

class Crypto(object):
    # RF5 uses the Rijndael cipher with 256-bit (32-byte) block size
    rijndael_cbc = RijndaelCbc(
            key='1cOSvkZ4HQCi6z/yQpEEl4neB+AIXwTX'.encode('utf-8'),
            iv = 'XuMigxpK61gLwgo1RsreLLGPcw3vJFze'.encode('utf-8'),
            padding=ZeroPadding(32),
            block_size=32
        )

    @staticmethod
    def decrypt(data: bytes) -> bytes:
        return Crypto.rijndael_cbc.decrypt(data)

    @staticmethod
    def encrypt(data: bytes) -> bytes:
        return Crypto.rijndael_cbc.encrypt(data)

    @staticmethod
    def checksum(data: bytes) -> int: # Not certain what algorithm this is
        
        # All variables are uint32 so we keep only the last 32 bits after every op
        sum = 0xcbf29ce4
        lo = 0
        running_sum = 0x39
        for idx in range(0, len(data)):
            value = data[idx]
            delta = (value - lo) & 0xffffffff
            lo = ((lo & 0xff) + 0xb2) & 0xffffffff
            sum = (sum * 0x1b3 ^ (delta ^ running_sum) & 0xff) & 0xffffffff
            running_sum = value
        return sum
