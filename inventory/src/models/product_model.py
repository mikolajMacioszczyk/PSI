from sqlalchemy import Column, Integer, String, Float, ForeignKey
from sqlalchemy.orm import relationship
from src.common import Base

# Model Product
class Product(Base):
    __tablename__ = 'products'
    __table_args__ = {'extend_existing': True}
    sku = Column(String, primary_key=True, index=True)
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