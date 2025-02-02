from .product_routes import get_products, update_product_stock, get_product, get_all_products, create_product
from .websocket_routes import websocket_endpoint
from .notification_routes import create_notification, get_notifications, get_notification_by_id
from .warehouse_routes import get_all_warehouses
from .health_routes import health_check