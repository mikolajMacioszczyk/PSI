from sqlalchemy.orm import declarative_base
from sqlalchemy import create_engine
from src.settings.db import DATABASE_URL
engine = create_engine(
    DATABASE_URL
)
Base = declarative_base()

active_connections = set()