resource "aws_security_group" "web-server-sg" {
  name = "web-server_lab10"

  tags = {
    Name = "security-group-lab10"
  }
}

resource "aws_vpc_security_group_ingress_rule" "allow_ssh_ipv4" {
  security_group_id = aws_security_group.web-server-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  from_port         = 22
  to_port           = 22
  ip_protocol       = "tcp"
}

# orders
resource "aws_vpc_security_group_ingress_rule" "allow_api_8081_ipv4" {
  security_group_id = aws_security_group.web-server-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  from_port         = 8081
  to_port           = 8081
  ip_protocol       = "tcp"
}

# catalog
resource "aws_vpc_security_group_ingress_rule" "allow_api_8082_ipv4" {
  security_group_id = aws_security_group.web-server-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  from_port         = 8082
  to_port           = 8082
  ip_protocol       = "tcp"
}

# backet-and-whishlist
resource "aws_vpc_security_group_ingress_rule" "allow_api_8083_ipv4" {
  security_group_id = aws_security_group.web-server-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  from_port         = 8083
  to_port           = 8083
  ip_protocol       = "tcp"
}

# inventory
resource "aws_vpc_security_group_ingress_rule" "allow_api_8084_ipv4" {
  security_group_id = aws_security_group.web-server-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  from_port         = 8084
  to_port           = 8084
  ip_protocol       = "tcp"
}

# Locust (performance tests)
resource "aws_vpc_security_group_ingress_rule" "allow_api_8089_ipv4" {
  security_group_id = aws_security_group.web-server-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  from_port         = 8089
  to_port           = 8089
  ip_protocol       = "tcp"
}

# Keycloak
resource "aws_vpc_security_group_ingress_rule" "allow_api_8001_ipv4" {
  security_group_id = aws_security_group.web-server-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  from_port         = 8001
  to_port           = 8001
  ip_protocol       = "tcp"
}

resource "aws_vpc_security_group_egress_rule" "allow_all_ipv4" {
  security_group_id = aws_security_group.web-server-sg.id
  cidr_ipv4         = "0.0.0.0/0"
  ip_protocol       = "-1"
}