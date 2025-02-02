module "inventory-deployment" {
  source = "./terraform-aws-ecs-deployment"
  service_name = "inventory"
  container_port = 5000
  desired_count = 2
  image_name = "254499/inventory:latest"
  iam_role_arn = data.aws_iam_role.lab_role.arn
  vpc_id = aws_vpc.vpc-main.id
  subnets_ids = [aws_subnet.subnet-main.id, aws_subnet.subnet-backup.id]
  environment_variables = [ 
    {name = "DB_USER", value = "${aws_db_instance.inventory_db.username}" },
    {name = "DB_PASSWORD", value = "${aws_db_instance.inventory_db.password}" },
    {name = "DB_HOST", value = "${aws_db_instance.inventory_db.endpoint}" },
    {name = "DB_PORT", value = "5432" },
    {name = "DB_NAME", value = "${aws_db_instance.inventory_db.db_name}" }
  ]
}

output "inventory_url" {
  value = module.inventory-deployment.service_dns_name
}