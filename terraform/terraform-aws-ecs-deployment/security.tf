resource "aws_security_group" "service_security_group" {
  name = "allow_access_to_service_${var.service_name}"
  description = "Allow access access to service"
  vpc_id = var.vpc_id

  ingress {
    from_port = var.container_port
    to_port = var.container_port
    protocol = "tcp"
    cidr_blocks = ["0.0.0.0/0"]
  }

  egress {
    from_port   = 0
    to_port     = 0
    protocol    = "-1"
    cidr_blocks = ["0.0.0.0/0"]
  }
}