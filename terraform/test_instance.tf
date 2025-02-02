# TEST EC2
resource "aws_instance" "test" {
  ami                    = "ami-0866a3c8686eaeeba"
  instance_type          = "t2.micro"
  vpc_security_group_ids = [aws_security_group.web-server-sg.id]
  key_name = "vockey"
  tags = {
    Name = "TEST"
  }
}

output "test_public_ip" {
  value = aws_instance.test.public_ip
}
