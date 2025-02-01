from pydantic import BaseModel
from typing import Optional

class NotificationCreate(BaseModel):
    product_id: str
    message: str
    minimum_stock: int

class NotificationUpdate(BaseModel):
    product_id: Optional[str] = None
    message: Optional[str] = None
    minimum_stock: Optional[int] = None
    active: Optional[bool] = None