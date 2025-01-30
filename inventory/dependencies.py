from fastapi import Depends, HTTPException, status
from fastapi.security import OAuth2PasswordBearer
from jose import JWTError, jwt
from typing import List
from sqlalchemy.orm import Session
import crud, database, models
import os

# OAuth2 token bearer
oauth2_scheme = OAuth2PasswordBearer(tokenUrl="token")

# Zmienna przechowująca SECRET_KEY, powinna pochodzić z konfiguracji lub zmiennych środowiskowych
SECRET_KEY = os.getenv("SECRET_KEY", "your_secret_key")  # Wczytywanie z env


# Funkcja do pobierania roli użytkownika na podstawie tokenu JWT
def get_user_role(db: Session, token: str):
    credentials_exception = HTTPException(
        status_code=status.HTTP_401_UNAUTHORIZED,
        detail="Could not validate credentials",
        headers={"WWW-Authenticate": "Bearer"},
    )

    try:
        # Dekodowanie tokenu
        payload = jwt.decode(token, SECRET_KEY, algorithms=["HS256"])
        user_id: int = payload.get("sub")
        role: str = payload.get("role")

        # Jeśli nie ma użytkownika lub roli, zwróć błąd
        if user_id is None or role is None:
            raise credentials_exception

        # Sprawdzamy, czy użytkownik istnieje w bazie danych
        user = db.query(models.User).filter(models.User.id == user_id).first()
        if user is None:
            raise credentials_exception

    except JWTError:
        # Jeśli wystąpi błąd podczas dekodowania tokenu
        raise credentials_exception

    return role

