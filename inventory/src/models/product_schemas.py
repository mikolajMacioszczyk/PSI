from pydantic import BaseModel
from typing import Optional

class ProductSchema(BaseModel):
    sku: str
    stock: int
    price: float
    warehouse_id: int
    wage: float
    rozmiarX: float
    rozmiarY: float
    rozmiarZ: float
    kolor: str

    class Config:
        from_attributes = True

class ProductCreate(BaseModel):
    stock: int
    price: float
    warehouse_id: int
    wage: float
    rozmiarX: float
    rozmiarY: float
    rozmiarZ: float
    kolor: str

class ProductUpdate(BaseModel):
    stock: Optional[int] = None
    # price: Optional[float] = None
    # warehouse_id: Optional[int] = None
    # wage: Optional[float] = None
    # rozmiarX: Optional[float] = None
    # rozmiarY: Optional[float] = None
    # rozmiarZ: Optional[float] = None
    # kolor: Optional[str] = None