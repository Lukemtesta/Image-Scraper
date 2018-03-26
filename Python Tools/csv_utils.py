import csv

def readCSV(filepath, separator):

    database = dict()

    with open(filepath, 'r') as csvfile:
        contents = csv.reader(csvfile, delimiter=separator)
        for i, row in enumerate(contents):
        
            if i > 0:
                key = row[0]
                if not key in database:
                    database[key] = []
                    
                database[key].append(row[:-1])
                
    return database