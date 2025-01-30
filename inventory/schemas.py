from pydantic import BaseModel
from typing import Optional

class Product(BaseModel):
    id: int
    name: str
    stock: int
    price: float
    warehouse_id: int

    class Config:
        from_attributes = True  # This allows Pydantic to work with ORM models like SQLAlchemy
# Schemat do tworzenia produktu
class ProductCreate(BaseModel):
    name: str
    stock: int
    price: float
    warehouse_id: int  # Dodajemy numer magazynu, aby przypisać produkt do konkretnego magazynu

# Schemat do aktualizacji produktu
class ProductUpdate(BaseModel):
    stock: Optional[int] = None
    price: Optional[float] = None
    warehouse_id: Optional[int] = None  # Możliwość zmiany magazynu

# Schemat do tworzenia powiadomienia
class NotificationCreate(BaseModel):
    product_id: int
    message: str
    minimum_stock: int

# Schemat do edytowania powiadomienia
class NotificationUpdate(BaseModel):
    product_id: Optional[int] = None
    message: Optional[str] = None
    minimum_stock: Optional[int] = None
    active: Optional[bool] = None  # Możliwość edycji statusu aktywności powiadomienia

