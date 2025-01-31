import pg8000
from sqlalchemy import create_engine
from sqlalchemy.orm import sessionmaker, declarative_base
import random
from faker import Faker
from sqlalchemy.orm import Session
from models import Warehouse, Product, Notification

# Initialize SQLAlchemy engine with the pg8000 driver
engine = create_engine(
    "postgresql+pg8000://postgres:221001@localhost:5432/warehouse"
)

# Create a session factory
SessionLocal = sessionmaker(autocommit=False, autoflush=False, bind=engine)

# Declare the base class for models
Base = declarative_base()

# Create the tables in the database if they don't exist
Base.metadata.create_all(bind=engine)

# Function to get a database session
def initDB():
    db = SessionLocal()
    try:
        generate_initial_data(db)
    finally:
        db.close()


def get_db():
    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

# Użycie biblioteki Faker do generowania danych
fake = Faker()
def generate_initial_data(db: Session):
    # Check if warehouses exist, if not - create
    if db.query(Warehouse).count() == 0:
        warehouses = [
            Warehouse(name="Warehouse A"),
            Warehouse(name="Warehouse B"),
            Warehouse(name="Warehouse C"),
        ]
        db.add_all(warehouses)
        db.commit()

        # Refresh each warehouse individually
        for warehouse in warehouses:
            db.refresh(warehouse)  # Refresh each warehouse object after commit

    # Check if products exist, if not - create
    if db.query(Product).count() == 0:
        products = [
            Product(
                sku=Product.generate_sku(i),  # Generowanie SKU na podstawie liczby istniejących produktów
                stock=random.randint(10, 500),  # Random stock
                price=round(random.uniform(5.0, 100.0), 2),  # Random price in range 5-100
                warehouse_id=random.choice([1, 2, 3]),  # Random warehouse assignment
                wage=round(random.uniform(1.0, 10.0), 2),  # Random weight
                rozmiarX=round(random.uniform(1.0, 100.0), 2),  # Random size X
                rozmiarY=round(random.uniform(1.0, 100.0), 2),  # Random size Y
                rozmiarZ=round(random.uniform(1.0, 100.0), 2),  # Random size Z
                kolor=fake.color_name(),  # Random color
            )
            for i in range(1, 101)  # Generate 100 products
        ]

        db.add_all(products)
        db.commit()  # Commit the products to get assigned ids

        # Refresh each product individually
        for product in products:
            db.refresh(product)  # Refresh each product object after commit

        db.commit()  # Commit again to save the SKU values

    # Check if notifications exist, if not - create
    if db.query(Notification).count() == 0:
        # Create low stock notifications for products
        low_stock_notifications = [
            Notification(
                product_id=product.sku,  # Odwołanie do sku zamiast id
                message=f"Low stock alert for product {product.sku}",
                minimum_stock=20,  # Example minimum stock level
            )
            for product in db.query(Product).all()
        ]
        db.add_all(low_stock_notifications)
        db.commit()
