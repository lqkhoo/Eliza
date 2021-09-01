import os
import unittest

from rf5_save_file import Rf5_Save_File


class TestParrot(unittest.TestCase):
    """This tests the whole process:
    1. Read in an encrypted save, decrypt the data section.
    2. Deserialize it into our classes. Then serialize everything from classes.
    3. Re-encrypt the data section
    4. Test that we get the same file back.

    This is an end-to-end safety net that catches anything going wrong at all,
    in the case that we haven't edited the data in any other way.
    """

    def _test_parrot(self, version: str, source_path: str, output_path: str):

        length = os.path.getsize(source_path)
        with open(source_path, 'rb') as f:
            original_data = f.read(length)

        save = Rf5_Save_File.from_encrypted_file(
            version=version,
            path=source_path
        )

        save.to_encrypted_file(output_path)

        out_length = os.path.getsize(output_path)
        with open(output_path, 'rb') as f:
            out_data = f.read(out_length)

        self.assertEqual(original_data, out_data)
        os.remove(output_path) # Delete the test output

    def test_parrot_v102(self):
        SOURCE_PATH = '../Eliza.Test/saves/data/102/rf5_gm001'
        OUTPUT_PATH = SOURCE_PATH + '_parrot_test'
        self._test_parrot('1.0.2', SOURCE_PATH, OUTPUT_PATH)


    def test_parrot_v106(self):
        """
        paths = [
            'data/106/rf5_gm000',
            # 'data/106/rf5_gm001',
            # 'data/106/rf5_gm002',
            'data/106/rf5_gm003',
            # 'data/106/rf5_gm004',
        ]

        for path in paths:
            output_path = path + '_parrot_test'
            self._test_parrot(path, output_path)
        """

        SOURCE_PATH = '../Eliza.Test/saves/data/106/rf5_gm000'
        OUTPUT_PATH = SOURCE_PATH + '_parrot_test'
        self._test_parrot('1.0.6', SOURCE_PATH, OUTPUT_PATH)

    
    def test_parrot_v107(self):
        SOURCE_PATH = '../Eliza.Test/saves/data/107/rf5_gm004'
        OUTPUT_PATH = SOURCE_PATH + '_parrot_test'
        self._test_parrot('1.0.7', SOURCE_PATH, OUTPUT_PATH)
    


if __name__ == '__main__':
    unittest.main()
    