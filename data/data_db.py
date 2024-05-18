from pymongo import MongoClient

class ConnectDB:
    def __init__(self, collection_name, connection_string="mongodb://localhost:27017/", database_name="local"):
        self.client = MongoClient(connection_string)
        self.db = self.client[database_name]
        self.collection = self.db[collection_name]

    def create(self, data):
        result = self.collection.insert_one(data)
        return result.inserted_id

    def getAll(self):
        return self.collection.find()

    def getByID(self,field_name ,name):
     return self.collection.find_one({field_name: name})

    def update(self, query, new_data):
        result = self.collection.update_many(query, {"$set": new_data})
        return result.modified_count

    def delete(self, query):
        result = self.collection.delete_many(query)
        return result.deleted_count
