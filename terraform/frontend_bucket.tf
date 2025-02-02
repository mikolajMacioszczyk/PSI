resource "aws_s3_bucket" "frontend_bucket" {
  bucket = "frontend-bucket-254499-unique-name"

  # Disable ACLs (S3 Bucket default in Terraform >= 3.75.0)
  object_lock_enabled = false
}

resource "aws_s3_bucket_website_configuration" "example" {
  bucket = aws_s3_bucket.frontend_bucket.id

  index_document {
    suffix = "index.html"
  }
}

output "bucket_website_endpoint" {
  value = aws_s3_bucket.frontend_bucket.website_endpoint
}