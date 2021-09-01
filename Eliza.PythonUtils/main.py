from __future__ import annotations
import os

from rf5_save_file import Rf5_Save_File

def pprint_bytes(x: bytes, width: int=0x10):
    """Prints bytes in hex like a hex editor would."""
    length = len(x)
    n = length // width # num_chunks
    r = length % width # num bytes in incomplete chunk

    hexstring = x.hex() # Should be 2x as long
    if r != 0:
        hexstring.join('  '*(width-r)) # we're just printing so just pad with spaces
    for i in range(0, len(hexstring), width*2):
        line = hexstring[i:i+width*2]
        print(' '.join(line[j:j+2] for j in range(0, width*2, 2)))



if __name__ == '__main__':

    """
    source_file = 'C:/dev/proj/rf5-eliza/Eliza/Eliza.Test/Saves/107/rf5_gm001'
    save = Rf5_Save_File.from_encrypted_file(
        version='1.0.7',
        path=source_file
    )
    save.to_encrypted_file('C:/dev/proj/rf5-eliza/Eliza/Eliza.Test/Saves/107/rf5_gm001_pythoncyc')
    """


