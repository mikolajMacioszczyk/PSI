# Używamy oficjalnego obrazu Pythona
FROM python:3.10

# Ustawiamy katalog roboczy
WORKDIR /app

# Kopiujemy pliki aplikacji do kontenera
COPY . /app

# Instalujemy zależności
RUN pip install --no-cache-dir -r requirements.txt

# Otwieramy port 5000
EXPOSE 5000

# Uruchamiamy FastAPI z Uvicornem
CMD ["uvicorn", "src.main:app", "--host", "0.0.0.0", "--port", "5000"]
