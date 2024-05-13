from fastapi import FastAPI, Request
import sys
sys.path.append("/Users/frowing/Projects/NLP")
from data.data_db import ConnectDB
from model.vector import Vector
import json

app = FastAPI()

@app.post("/api/vectorInsert")
async def vectorInsert(request: Request):
    context = ConnectDB()
    data = await request.json()
    context.create(data)
    return {"message": "Vektor başarıyla eklendi"}

@app.get("/api/vectorGet/{prm}")
async def vectorGet(prm: str):
    context = ConnectDB()
    vectorsJson = context.getAll()

    for temp in vectorsJson:
        theVectorJson = temp['vector']
        if theVectorJson['name'] == prm:
            result = Vector(theVectorJson["name"],theVectorJson["vector"])
            if result is None:
                errorMessage = prm + " ID'li öğe bulunamadı"
                return {"errorMessage": errorMessage}        
            else:
             return { result }

    #if vectorJson is None:
     #   errorMessage = prm + " ID'li öğe bulunamadı"
      #  return {"errorMessage": errorMessage}
    #result = Vector(vectorJson["name"],vectorJson["vector"])
    
    return {"ERROR"} 
