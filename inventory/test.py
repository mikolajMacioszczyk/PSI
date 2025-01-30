import psycopg2

# Establish a connection to the PostgreSQL database
conn = psycopg2.connect(
    database="postgres",
    user="postgres",
    host="localhost",
    password="postgres",
    port=5432
)

# Set the client encoding to UTF-8
conn.set_client_encoding('UTF8')

# Verify the connection and encoding
print(conn.encoding)
