from __future__ import annotations
from typing import Dict

import os
import item_ids

def get_folders(path: str):
    pass


if __name__ == '__main__':

    SOURCE_PATH = '..\\Eliza\\Data\\Images\\'
    DICT_OUTPUT_PATH = 'Items_generated.cs'

    filename_to_assemblypath_map : Dict[str, str] = {}
    filepaths = []

    for (dirpath, dirnames, filenames) in os.walk(SOURCE_PATH):
        for filename in filenames:
            
            filepath = "{}\{}".format(dirpath, filename)
            filepaths.append(filepath.replace('..\\Eliza\\', ''))
            assemblypath = filepath
            assemblypath = (assemblypath
                            .replace('..', '')
                            .replace('\\', '.'))
            assemblypath = 'resm:' + assemblypath[1:] + '?assembly=Eliza'
            filename_to_assemblypath_map[filename] = assemblypath
            # print(assemblypath)


    # Output c# partial class. Copy this into Eliza.Data.Items.cs
    with open(DICT_OUTPUT_PATH, 'w') as output:
        output.write('    // Generated from Python script\n')
        output.write('    public static partial class Items {\n')
        output.write('        public static readonly Dictionary<int, string> ItemIdToAssemblyResourceUri = new() {\n')

        output.write('        { 0, "resm:Eliza.Data.Images.Empty.png?assembly=Eliza" },\n')
        for id in item_ids.id_to_image_name.keys():
            # map id to assembly
            image_name = item_ids.id_to_image_name[id]
            assembly_path = filename_to_assemblypath_map[image_name]
            output.write('        {{ {}, "{}" }},\n'.format(id, assembly_path))

        output.write('        };\n')
        output.write('    }\n')

