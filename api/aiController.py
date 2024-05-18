from fastapi import FastAPI, Request
import sys

sys.path.append("/Users/frowing/Projects/NLP/")
from data.data_db import ConnectDB
from data.data_get import DataRead
from data.data_form import DataStem
from data.data_form2 import DataStem2
from model.vector import Vector
import json

app = FastAPI()

@app.get("/api/getAllArticle")
async def getAllArticle():
    getter = DataRead()
    articles = getter.readAllTxt()
    return articles

@app.post("/api/makeVectorAll/")
async def makeVektorAll():
    getter = DataRead()
    context = ConnectDB()
    former = DataStem('/Users/frowing/Downloads/cc.en.300.bin')
    articles = getter.readAllTxt()
    vektor_list = []

    for article in articles:
       vektor = former.makeStem(article.content)
       vektor_list.append(Vector(article.filename, vektor))

    for vektor in vektor_list:
      context.create(vektor.to_json())

    return {"Başarıyla Tamamlandı"}

@app.post("/api/makeVectorByID/{prm}")
async def makeVektor(prm : str):
     id = prm
     getter = DataRead()
     former = DataStem('/Users/frowing/Downloads/cc.en.300.bin')

     content = getter.readTxtByName(id + ".txt")    
     vectors = former.makeStem(content)
     vector = Vector(id,vectors)

     return {"vector": vector}   

@app.post("/api/makeVectorByID2/{prm}")
async def makeVektor2(prm : str):
      id = prm
      getter = DataRead()
      former = DataStem2()
      content = getter.readTxtByName(id + ".txt")    
      vectors = former.makeStem(content)
      vector = Vector(id,vectors)

      return {"vector": vector }   

@app.post("/api/makeVectorByWord/{prm}")
async def makeVektor3(prm : str):
      content = prm
      former = DataStem('/Users/frowing/Downloads/cc.en.300.bin')
      vectors = former.makeStem(content)
      vector = Vector(id,vectors)

      return {"vector": vector}  

@app.get("/api/calculateSimilarity")
async def calculateSimilarity(request: Request):
    getter = DataRead()
    context = ConnectDB()
    data = await request.json()

    if data is None:
        error_message = "Parametre boş dönüyor"
        return {"error": error_message}

    vector1ID = data["vector1"]
    vector2ID = data["vector2"]
    allVector = context.getAll()
    vector1Json = None
    vector2Json = None

    for temp in allVector:
        vector = temp['vector']
        if  vector1ID == vector['name']:
            vector1Json = vector
        elif vector2ID == vector['name']:
            vector2Json = vector


        if vector1Json is not None and vector2Json is not None:
            similarity = getter.cosine_similarity(vector1Json['vector'], vector2Json['vector'])
            return { "similarityRate" : round(similarity * 100, 2) }

    if vector1Json is None or vector2Json is None:
        error_message = "Bir veya daha fazla JSON değeri None olarak belirlendi."
        return {"error": error_message}
    
    
    return {'ERROR'}
    
