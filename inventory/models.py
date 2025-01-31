from sqlalchemy import Column, Integer, String, Float, Boolean, ForeignKey
from sqlalchemy.orm import relationship, declarative_base
from sqlalchemy import event
from sqlalchemy.orm import validates
from typing import List
from pydantic import BaseModel

Base = declarative_base()  # Używanie importu z sqlalchemy.orm

# Model Warehouse
class Warehouse(Base):
    __tablename__ = 'warehouses'

    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, index=True)

    # Relacja do produktów
    products = relationship("Product", back_populates="warehouse")


# Model Product
class Product(Base):
    __tablename__ = 'products'

    sku = Column(String, primary_key=True, index=True)  # Zmieniamy SKU na primary_key
    stock = Column(Integer)
    price = Column(Float)
    warehouse_id = Column(Integer, ForeignKey('warehouses.id'))

    # Additional fields
    wage = Column(Float)
    rozmiarX = Column(Float)
    rozmiarY = Column(Float)
    rozmiarZ = Column(Float)
    kolor = Column(String)

    # Relacja z magazynem
    warehouse = relationship("Warehouse", back_populates="products")

    # Relacja z powiadomieniami
    notifications = relationship("Notification", back_populates="product")

    @staticmethod
    def generate_sku(id):
        """Generate SKU based on the product's id."""
        return f"00000000-0000-0000-0000-{str(id).zfill(12)}"


# Model Notification
class Notification(Base):
    __tablename__ = 'notifications'

    id = Column(Integer, primary_key=True, index=True)
    product_id = Column(String, ForeignKey('products.sku'))  # Odwołanie do sku, nie id
    message = Column(String)
    minimum_stock = Column(Integer)
    active = Column(Boolean, default=True)

    # Relacja z produktem
    product = relationship('Product', back_populates="notifications")

class SKURequest(BaseModel):
    skus: List[str]