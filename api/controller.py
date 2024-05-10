from fastapi import FastAPI

app = FastAPI()

@app.get("/api/hello/{prm}")
async def hello(prm: str):
    return {"anan": str}
