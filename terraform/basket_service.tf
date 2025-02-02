module "basket-deployment" {
  source = "./terraform-aws-ecs-deployment"
  service_name = "basket"
  container_port = 8080
  desired_count = 2
  image_name = "254499/basket-and-wishlist:latest"
  iam_role_arn = data.aws_iam_role.lab_role.arn
  vpc_id = aws_vpc.vpc-main.id
  subnets_ids = [aws_subnet.subnet-main.id, aws_subnet.subnet-backup.id]
  environment_variables = [ 
    { name = "ASPNETCORE_ENVIRONMENT", value = "Development" },
    { name = "ConnectionStrings__Db", value= "Server=${aws_db_instance.shop_db.endpoint};Port=5432;Database=${aws_db_instance.shop_db.db_name};User Id=${aws_db_instance.shop_db.username};Password=${aws_db_instance.shop_db.password}"},
    { name = "ConnectionStrings__Catalog", value = "http://${module.catalog-deployment.service_dns_name}:8080/" },
    { name = "KeycloakServiceConfig__AuthServerUrl", value = "http://ec2-34-238-151-170.compute-1.amazonaws.com:8001/" },
  ]
}

output "basket_url" {
  value = module.basket-deployment.service_dns_name
}