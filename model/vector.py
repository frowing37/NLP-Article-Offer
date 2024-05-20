import json

class Vector:
    def __init__(self, name, vector):
        self.name = name
        self.vector = vector   

    def to_json(self):
        return json.dumps(self, default=lambda o: o.__dict__, indent=4)
    
    def to_dict(self):
        return {"name": self.name, "vector": self.vector}
    
