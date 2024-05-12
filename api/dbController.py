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
async def vectorInsert(prm: str):
    context = ConnectDB()
    vectorJson = context.getByID(prm)
    result = Vector(vectorJson["id"],vectorJson["vector"])
    return {result}
