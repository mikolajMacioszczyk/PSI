resource "aws_lb" "servie_alb" {
  name               = "${var.service_name}-alb"
  internal           = false
  load_balancer_type = "application"
  security_groups    = [aws_security_group.service_security_group.id]
  subnets            = var.subnets_ids
}

resource "aws_lb_target_group" "service_target_group" {
  name        = "${var.service_name}-tg"
  port        = var.container_port
  protocol    = "HTTP"
  target_type = "ip"
  vpc_id      = var.vpc_id
  health_check {
    path = "/health"
  }
}

resource "aws_lb_listener" "http_listener" {
  load_balancer_arn = aws_lb.servie_alb.arn
  port              = var.container_port
  protocol          = "HTTP"

  default_action {
    type             = "forward"
    target_group_arn = aws_lb_target_group.service_target_group.arn
  }
}