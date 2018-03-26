import os
import PIL
import csv
import argparse

from PIL import Image

def readCSV(filepath, separator):

    database = dict()

    with open(filepath, 'r') as csvfile:
        contents = csv.reader(csvfile, delimter=separator)
        for i, row in enumerate(contents):
        
            if i > 0:
                key = ' '.join(row[:-1])
                data = row[-1]
                database[key] = data
                
    return database

if __name__ == "__main__":

    parser = argparse.ArgumentParser()

    # default to lfpw dataset script directory 
    parser.add_argument('--root', dest='root', help='Root directory to search for images', required=True)
    parser.add_argument('--database', dest='database', help='Fullpath to file containing class guid table', required=True)
    parser.add_argument('--outfile', dest='outfile', help='Target filename saved in parent folder of root', required=True)

    args = parser.parse_args();

    class_db = readCSV(args.database, ' ')

    # new line deprecated in python 3.2+
    file_dataset = open( os.path.join(args.root, "..", args.outfile), 'w', newline = "\n")
    
    for i, (dirpath, _, filenames) in enumerate(os.walk(args.root)):
            
        name = os.path.basename(dirpath)

        if not name in class_db:
            print("Class ", name, "not in database. Skipping. . .")
            continue
            
        for filename in filenames:
            
            fullpath = os.path.join(dirpath, filename)
            
            # Don't support wide string chars
            try:
                file_dataset.write( fullpath + " " + str(class_db[name]) + "\n" )
            except:
                continue
           
            
    file_dataset.close()