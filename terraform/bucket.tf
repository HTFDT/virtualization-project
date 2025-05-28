resource "yandex_storage_bucket" "bucket" {
  folder_id     = var.folder_id
  bucket        = "almetov-project-bucket"
  force_destroy = true
}