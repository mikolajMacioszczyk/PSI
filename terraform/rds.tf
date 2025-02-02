resource "aws_db_subnet_group" "db_subnet_group" {
  name       = "db-subnet-group"
  subnet_ids = [aws_subnet.subnet-main.id, aws_subnet.subnet-backup.id]

  tags = {
    Name = "DB subnet group"
  }
}

resource "aws_security_group" "db_security_group" {
  name = "allow_access_to_db"
  description = "Allow access to DB from my IP and backend"
  vpc_id = aws_vpc.vpc-main.id

  ingress {
    from_port = 5432
    to_port = 5432
    protocol = "tcp"
    security_groups = [module.catalog-deployment.security_group_id, module.basket-deployment.security_group_id, module.orders-deployment.security_group_id, module.inventory-deployment.security_group_id]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}

resource "aws_db_instance" "shop_db" {
  identifier = "shopdb"
  db_name         = "shopdb"
  instance_class    = "db.t4g.micro"
  engine           = "postgres"
  engine_version   = "16.3"
  
  allocated_storage     = 20
  storage_type         = "gp3"
  
  username          = "postgres"
  password          = "postgres"
  
  publicly_accessible    = true
  skip_final_snapshot = true
  db_subnet_group_name = aws_db_subnet_group.db_subnet_group.name
  vpc_security_group_ids = [aws_security_group.db_security_group.id]
}

resource "aws_db_instance" "inventory_db" {
  identifier = "inventorydb"
  db_name         = "inventorydb"
  instance_class    = "db.t4g.micro"
  engine           = "postgres"
  engine_version   = "16.3"
  
  allocated_storage     = 20
  storage_type         = "gp3"
  
  username          = "postgres"
  password          = "postgres"
  
  publicly_accessible    = true
  skip_final_snapshot = true
  db_subnet_group_name = aws_db_subnet_group.db_subnet_group.name
  vpc_security_group_ids = [aws_security_group.db_security_group.id]
}