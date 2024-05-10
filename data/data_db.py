from pymongo import MongoClient

class ConnectDB:
    def __init__(self, connection_string="mongodb://localhost:27017/", database_name="local", collection_name="try1"):
        self.client = MongoClient(connection_string)
        self.db = self.client[database_name]
        self.collection = self.db[collection_name]

    def create(self, data):
        result = self.collection.insert_one(data)
        return result.inserted_id

    def getAll(self, query=None):
        if query:
            return self.collection.find(query)
        else:
            return self.collection.find()

    def getByID(self, object_id):
        return self.collection.find_one({"id": object_id})

    def update(self, query, new_data):
        result = self.collection.update_many(query, {"$set": new_data})
        return result.modified_count

    def delete(self, query):
        result = self.collection.delete_many(query)
        return result.deleted_count
