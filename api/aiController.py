from fastapi import FastAPI, Request
import sys
sys.path.append("/Users/frowing/Projects/NLP")
from data.data_db import ConnectDB
from data.data_get import DataRead
from data.data_form import DataStem
from model.vector import Vector
import json

app = FastAPI()

@app.get("/api/makeVectorAll/")
async def makeVektorAll():
    getter = DataRead()
    former = DataStem('/Users/frowing/Downloads/cc.en.300.bin')
    fileNames = getter.readFilesName()
    vektor_list = []

    for fileName in fileNames:
       content = getter.readTxtByName(fileName)
       vektor = former.makeStem(content)
       vektor_list.append(Vector(fileName,vektor))

    return {vektor_list}

@app.post("/api/makeVectorByID/{prm}")
async def makeVektor(prm : str):
     id = prm
     getter = DataRead()
     former = DataStem('/Users/frowing/Downloads/cc.en.300.bin')

     content = getter.readTxtByName(id + ".txt")    
     vectors = former.makeStem(content)
     vector = Vector(id,vectors)

     return {"vector": vector}   

@app.get("/api/calculateSimilarity/{prm1}+{prm2}")
async def makeVektorAll(prm1: str, prm2: str):
    getter = DataRead()
    context = ConnectDB()
    vector1 = prm1
    vector2 = prm2
    vector1Json = context.getByID(vector1)
    vector2Json = context.getByID(vector2)
    similarity = getter.cosine_similarity(vector1Json["vector"],vector2Json["vector"])

    return {similarity}