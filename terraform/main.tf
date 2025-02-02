provider "aws" {
  region = "us-east-1"
}

data "aws_iam_role" "lab_role" {
  name = "LabRole"
}