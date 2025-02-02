resource "aws_ecs_task_definition" "service_task" {
  family = "${var.service_name}-task"
  network_mode = "awsvpc"
  requires_compatibilities= ["FARGATE"]
  cpu                      = 1024
  memory                   = 2048
  container_definitions = jsonencode([
    {
      name      = var.service_name
      image     = var.image_name
      cpu       = 1024
      memory    = 2048
      essential = true
      portMappings = [
        {
          containerPort = var.container_port
          hostPort      = var.container_port
        }
      ],   
      environment = var.environment_variables
    }
  ])

  runtime_platform {
    operating_system_family = "LINUX"
    cpu_architecture        = "X86_64"
  }
}

resource "aws_ecs_cluster" "service_cluster" {
  name = "${var.service_name}-cluster"
}

resource "aws_ecs_service" "service" {
  name            = "${var.service_name}-service"
  cluster         = aws_ecs_cluster.service_cluster.id
  task_definition = aws_ecs_task_definition.service_task.arn
  launch_type     = "FARGATE"
  desired_count   = var.desired_count

  network_configuration {
    subnets = var.subnets_ids
    security_groups = [aws_security_group.service_security_group.id]
    assign_public_ip = true
  }

  load_balancer {
    target_group_arn = aws_lb_target_group.service_target_group.arn
    container_name   = var.service_name
    container_port   = var.container_port
  }
}

resource "aws_appautoscaling_target" "service_target" {
  max_capacity       = 1
  min_capacity       = 1
  resource_id        = "service/${aws_ecs_cluster.service_cluster.name}/${aws_ecs_service.service.name}"
  scalable_dimension = "ecs:service:DesiredCount"
  service_namespace  = "ecs"
}
