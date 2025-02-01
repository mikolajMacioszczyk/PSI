from pydantic import BaseModel
from typing import List

class SKURequest(BaseModel):
    skus: List[str]