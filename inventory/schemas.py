from pydantic import BaseModel
from typing import Optional

# Pydantic model for Product
class Product(BaseModel):
    sku: str  # Zmieniamy 'id' na 'sku' jako klucz podstawowy
    stock: int
    price: float
    warehouse_id: int
    wage: float
    rozmiarX: float
    rozmiarY: float
    rozmiarZ: float
    kolor: str

    class Config:
        from_attributes = True  # This allows Pydantic to work with ORM models like SQLAlchemy

# Schemat do tworzenia produktu
class ProductCreate(BaseModel):
    stock: int
    price: float
    warehouse_id: int  # Dodajemy numer magazynu, aby przypisać produkt do konkretnego magazynu
    wage: float
    rozmiarX: float
    rozmiarY: float
    rozmiarZ: float
    kolor: str

# Schemat do aktualizacji produktu
class ProductUpdate(BaseModel):
    stock: Optional[int] = None
    # price: Optional[float] = None
    # warehouse_id: Optional[int] = None  # Możliwość zmiany magazynu
    # wage: Optional[float] = None
    # rozmiarX: Optional[float] = None
    # rozmiarY: Optional[float] = None
    # rozmiarZ: Optional[float] = None
    # kolor: Optional[str] = None

# Schemat do tworzenia powiadomienia
class NotificationCreate(BaseModel):
    product_id: str  # Zmieniamy 'product_id' na 'sku' typu str
    message: str
    minimum_stock: int

# Schemat do edytowania powiadomienia
class NotificationUpdate(BaseModel):
    product_id: Optional[str] = None  # Zmieniamy 'product_id' na 'sku' typu str
    message: Optional[str] = None
    minimum_stock: Optional[int] = None
    active: Optional[bool] = None  # Możliwość edycji statusu aktywności powiadomienia
