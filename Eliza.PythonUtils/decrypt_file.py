from __future__ import annotations
import argparse
import os

from rf5_save_file import Rf5_Save_File

# Usage: From the CLI
# >>> python decrypt_file.py -i [path to encrypted file]
# The script will output the decrypted file with '_decrypted' appended its name.

if __name__ == '__main__':

    parser = argparse.ArgumentParser(description=
                'Decrypt an encrypted Rune Factory 5 save file.')
    parser.add_argument('-i', type=str, dest='input_path', help='path to encrypted file')
    args = parser.parse_args()

    input_path = args.input_path
    output_path = input_path + '_decrypted'

    Rf5_Save_File.just_decrypt_file(input_path, output_path)
