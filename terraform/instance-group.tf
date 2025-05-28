data "yandex_compute_image" "container_optimized_image" {
  family = "container-optimized-image"
}

resource "yandex_compute_instance_group" "vm_group" {
  name                = "almetov-project-vm-group"
  service_account_id  = yandex_iam_service_account.sa_vm_group.id
  deletion_protection = false
  depends_on          = [yandex_resourcemanager_folder_iam_member.sa_vm_group_compute_editor]

  instance_template {
    platform_id        = "standard-v2"
    service_account_id = yandex_iam_service_account.sa_vm.id
    resources {
      memory = 4
      cores  = 4
    }
    boot_disk {
      mode = "READ_WRITE"
      initialize_params {
        image_id = data.yandex_compute_image.container_optimized_image.id
        size     = 20
      }
    }
    network_interface {
      network_id = yandex_vpc_network.vpc_network.id
      subnet_ids = ["${yandex_vpc_subnet.subnet_a.id}", "${yandex_vpc_subnet.subnet_b.id}", "${yandex_vpc_subnet.subnet_d.id}"]
      nat        = true
    }
    metadata = {
      docker-compose = <<-EOT
services:
  app:
    environment:
      - S3_KEY_ID=${yandex_iam_service_account_static_access_key.sa_object_storage_static_key.access_key}
      - S3_SECRET_KEY=${yandex_iam_service_account_static_access_key.sa_object_storage_static_key.secret_key}
      - S3_BUCKET_NAME=almetov-project-bucket
      - ASPNETCORE_ENVIRONMENT=Development
      - CONNECTION_STRING=Host=c-${yandex_mdb_postgresql_cluster.postgresql_cluster.id}.rw.mdb.yandexcloud.net;Port=6432;Database=almetov_project_db;Username=almetov;Password=qwerty123;Ssl Mode=VerifyFull;Root Certificate=/app/root.crt
      - APPLY_MIGRATIONS=true

    image: ${var.app_image}
    ports:
      - "80:80"
    restart: unless-stopped
EOT
      user-data      = <<-EOT
#cloud-config
ssh_pwauth: no
users:
  - name: user
    groups: sudo
    shell: /bin/bash
    sudo: 'ALL=(ALL) NOPASSWD:ALL'
    ssh_authorized_keys:
      - ${file("~/.ssh/id_ed25519.pub")}
EOT
    }
    network_settings {
      type = "STANDARD"
    }
  }

  scale_policy {
    fixed_scale {
      size = 3
    }
  }

  allocation_policy {
    zones = ["ru-central1-a", "ru-central1-b", "ru-central1-d"]
  }

  deploy_policy {
    max_unavailable = 2
    max_creating    = 2
    max_expansion   = 2
    max_deleting    = 2
  }

  load_balancer {
    target_group_name            = "almetov-project-balancer-group"
    max_opening_traffic_duration = 600
  }
}