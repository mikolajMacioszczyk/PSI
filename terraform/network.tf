resource "aws_vpc" "vpc-main" {
  cidr_block       = "10.0.0.0/16"
  instance_tenancy = "default"
  enable_dns_hostnames = true
  enable_dns_support = true

  tags = {
    Name = "vpc-lab3"
  }
}

resource "aws_internet_gateway" "gateway" {
  vpc_id = aws_vpc.vpc-main.id

  tags = {
    Name = "gateway-lab3"
  }
}

resource "aws_subnet" "subnet-main" {
  vpc_id     = aws_vpc.vpc-main.id
  availability_zone = "us-east-1a"
  cidr_block = "10.0.1.0/24"
  map_public_ip_on_launch = true

  tags = {
    Name = "subnet-lab6-main"
  }
}

resource "aws_subnet" "subnet-backup" {
  vpc_id     = aws_vpc.vpc-main.id
  availability_zone = "us-east-1b"
  cidr_block = "10.0.2.0/24"
  map_public_ip_on_launch = true

  tags = {
    Name = "subnet-lab6-backup"
  }
}

resource "aws_route_table" "main_rt" {
  vpc_id = aws_vpc.vpc-main.id
  tags = {
    Name = "main-route-table-lab3"
  }
}

resource "aws_route" "internet_access" {
  route_table_id         = aws_route_table.main_rt.id
  destination_cidr_block = "0.0.0.0/0"
  gateway_id             = aws_internet_gateway.gateway.id
}

resource "aws_route_table_association" "gateway_route_association_first_subnet" {
  subnet_id     = aws_subnet.subnet-main.id
  route_table_id = aws_route_table.main_rt.id
}

resource "aws_route_table_association" "gateway_route_association_second_subnet" {
  subnet_id     = aws_subnet.subnet-backup.id
  route_table_id = aws_route_table.main_rt.id
}