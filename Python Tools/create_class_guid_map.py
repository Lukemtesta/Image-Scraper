import os
import PIL
import argparse

from PIL import Image

if __name__ == "__main__":

    parser = argparse.ArgumentParser()

    # default to lfpw dataset script directory 
    parser.add_argument('--root', dest='root', help='Root directory to search for images', required=True)
    parser.add_argument('--outfile', dest='outfile', help='Database filename')

    args = parser.parse_args();

    # new line deprecated in python 3.2+
    file_class_db = open( os.path.join(args.root, "..", args.outfile), 'w', newline = "\n")
    file_class_db.write("Class GUID\n")
    
    for i, (dirpath, _, filenames) in enumerate(os.walk(args.root)):
        
        file_class_db.write(os.path.basename(dirpath) + " " + str(i) + "\n")

    file_class_db.close()