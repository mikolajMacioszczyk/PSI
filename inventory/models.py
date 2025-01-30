from sqlalchemy import Column, Integer, String, Float, Boolean, ForeignKey
from sqlalchemy.orm import relationship, declarative_base  # Nowy sposób importu
# No longer importing from sqlalchemy.ext.declarative

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

    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, index=True)
    stock = Column(Integer)
    warehouse_id = Column(Integer, ForeignKey('warehouses.id'))

    # Relacja z magazynem
    warehouse = relationship("Warehouse", back_populates="products")

    # Relacja z powiadomieniami
    notifications = relationship("Notification", back_populates="product")


# Model Notification
class Notification(Base):
    __tablename__ = 'notifications'

    id = Column(Integer, primary_key=True, index=True)
    product_id = Column(Integer, ForeignKey('products.id'))
    message = Column(String)
    minimum_stock = Column(Integer)
    active = Column(Boolean, default=True)

    # Relacja z produktem
    product = relationship('Product', back_populates="notifications")
