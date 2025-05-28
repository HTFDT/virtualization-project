resource "yandex_iam_service_account" "sa_vm_group" {
  name        = "almetov-project-sa-vm-group"
  description = "Сервисный аккаунт для группы вм"
}

resource "yandex_resourcemanager_folder_iam_member" "sa_vm_group_compute_editor" {
  folder_id = var.folder_id
  role      = "editor"
  member    = "serviceAccount:${yandex_iam_service_account.sa_vm_group.id}"
}

resource "yandex_iam_service_account" "sa_vm" {
  name        = "almetov-project-sa-vm"
  description = "Сервисный аккаунт для отдельной вм"
}

resource "yandex_resourcemanager_folder_iam_member" "sa_vm_images_puller" {
  folder_id = var.folder_id
  role      = "container-registry.images.puller"
  member    = "serviceAccount:${yandex_iam_service_account.sa_vm.id}"
}


resource "yandex_iam_service_account" "sa_object_storage" {
  name        = "almetov-project-sa-storage"
  description = "Сервисный аккаунт для доступа к object storage из приложения"
}

resource "yandex_resourcemanager_folder_iam_member" "sa_object_storage_editor" {
  folder_id = var.folder_id
  role      = "storage.editor"
  member    = "serviceAccount:${yandex_iam_service_account.sa_object_storage.id}"
}

resource "yandex_iam_service_account_static_access_key" "sa_object_storage_static_key" {
  service_account_id = yandex_iam_service_account.sa_object_storage.id
}


resource "yandex_iam_service_account" "sa_api_gateway" {
  name        = "almetov-project-sa-api-gateway"
  description = "Сервисный аккаунт для api gateway"
}

resource "yandex_resourcemanager_folder_iam_member" "sa_api_gateway_storage_viewer" {
  folder_id = var.folder_id
  role      = "storage.viewer"
  member    = "serviceAccount:${yandex_iam_service_account.sa_api_gateway.id}"
}
