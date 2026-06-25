# --------------------------

from fastapi import FastAPI
import pandas as pd
import joblib
import uvicorn

app = FastAPI()

model_upto_1cr = joblib.load("pakwheels_price_predictor_upto_1cr.joblib")
model_above_1cr = joblib.load("pakwheels_price_predictor_above_1cr.joblib")

@app.get("/")
def home():
    return {"message": "API Running"}

@app.post("/predict")
def predict(data: dict):

    vehicle_age = 2025 - int(data["model"])

    df = pd.DataFrame([{
        "brand": data["brand"],
        "model_name": data["model_name"],
        "variant": data["variant"],
        "model": data["model"],
        "mileage": data["mileage"],
        "city": data["city"],
        "fuel_type": data["fuel_type"],
        "transmission": data["transmission"],
        "registered": data["registered"],
        "assembly": data["assembly"],
        "engine_capacity": data["engine_capacity"],
        "vehicle_age": vehicle_age
    }])

    model = model_above_1cr if data["car_type"].lower() == "yes" else model_upto_1cr

    price = model.predict(df)[0]

    return {"predicted_price": int(price)}


# ✅ Run FastAPI automatically when file runs
if __name__ == "__main__":
    uvicorn.run("api:app", host="127.0.0.1", port=8000, reload=False)