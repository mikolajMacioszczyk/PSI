import os

DB_USER = os.getenv("DB_USER", "postgres")
DB_PASSWORD = os.getenv("DB_PASSWORD", "postgres")

def get_db_host():
    endpoint = os.getenv("DB_HOST", "localhost")
    return endpoint.split(":")[0] if ":" in endpoint else endpoint
DB_HOST = get_db_host()
print(DB_HOST)

if os.getenv("DOCKER_ENV"):
    DB_HOST = "postgres"
DB_PORT = os.getenv("DB_PORT", "5432")
DB_NAME = os.getenv("DB_NAME", "postgres")

DATABASE_URL = f"postgresql://{DB_USER}:{DB_PASSWORD}@{DB_HOST}:{DB_PORT}/{DB_NAME}"
