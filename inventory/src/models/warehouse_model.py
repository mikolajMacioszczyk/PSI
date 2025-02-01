from sqlalchemy import Column, Integer, String
from sqlalchemy.orm import relationship
from src.common import Base

class Warehouse(Base):
    __tablename__ = 'warehouses'
    __table_args__ = {'extend_existing': True}
    id = Column(Integer, primary_key=True, index=True)
    name = Column(String, index=True)

    products = relationship("Product", back_populates="warehouse")