variable "service_name" {
  type = string
}

variable "container_port" {
  type = number
}

variable "desired_count" {
  type = number
}

variable "image_name" {
  type = string
}

variable "iam_role_arn" {
  type = string
}

variable "environment_variables" {
  type        = list(object({
    name  = string
    value = string
  }))
}

variable "vpc_id" {
  type = string
}

variable "subnets_ids" {
  type = list(string)
}

output "service_dns_name" {
  value = aws_lb.servie_alb.dns_name
}

output "security_group_id" {
  value = aws_security_group.service_security_group.id
}