import os
import PIL
import argparse

from PIL import Image

if __name__ == "__main__":

    parser = argparse.ArgumentParser()

    # default to lfpw dataset script directory 
    parser.add_argument('--root', dest='root', help='Root directory to search for images', required=True)
    parser.add_argument('--resx', type=int, dest='resx', help='Target X resolution', required=True)
    parser.add_argument('--resy', type=int, dest='resy', help='Target Y resolution', required=True)

    args = parser.parse_args();
	
    for dirpath, dirnames, filenames in os.walk(args.root):
        for filename in filenames:
            
            fullpath = os.path.join(dirpath, filename)

            try:
                print('Resizing', os.path.join(dirpath, filename))
            
                img = Image.open(fullpath)
                img = img.resize( (args.resx, args.resy), Image.ANTIALIAS)
                img.save(fullpath)
            except:
                print('file corrupt')
            
