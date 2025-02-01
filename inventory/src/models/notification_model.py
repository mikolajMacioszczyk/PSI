from sqlalchemy import Column, Integer, String, Boolean, ForeignKey
from sqlalchemy.orm import relationship
from src.common import Base
    # Model Notification
class Notification(Base):
    __tablename__ = 'notifications'
    __table_args__ = {'extend_existing': True}
    id = Column(Integer, primary_key=True, index=True)
    product_id = Column(String, ForeignKey('products.sku'))  # Odwołanie do sku, nie id
    message = Column(String)
    minimum_stock = Column(Integer)
    active = Column(Boolean, default=True)

    # Relacja z produktem
    product = relationship('Product', back_populates="notifications")