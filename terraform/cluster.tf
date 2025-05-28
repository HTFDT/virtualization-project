resource "yandex_mdb_postgresql_cluster" "postgresql_cluster" {
  name        = "almetov-project-postgresql-cluster"
  environment = "PRODUCTION"
  network_id  = yandex_vpc_network.vpc_network.id

  config {
    version = 17
    resources {
      resource_preset_id = "s3-c2-m8"
      disk_type_id       = "network-ssd"
      disk_size          = 20
    }
  }

  maintenance_window {
    type = "WEEKLY"
    day  = "SAT"
    hour = 12
  }

  host {
    zone             = "ru-central1-a"
    subnet_id        = yandex_vpc_subnet.subnet_a.id
    assign_public_ip = true
  }

  host {
    zone             = "ru-central1-b"
    subnet_id        = yandex_vpc_subnet.subnet_b.id
    assign_public_ip = true
  }

  host {
    zone             = "ru-central1-d"
    subnet_id        = yandex_vpc_subnet.subnet_d.id
    assign_public_ip = true
  }
}

resource "yandex_mdb_postgresql_user" "db_user" {
  cluster_id = yandex_mdb_postgresql_cluster.postgresql_cluster.id
  name       = "almetov"
  password   = "qwerty123"
  conn_limit = 50
}

resource "yandex_mdb_postgresql_database" "db" {
  cluster_id = yandex_mdb_postgresql_cluster.postgresql_cluster.id
  name       = "almetov_project_db"
  owner      = yandex_mdb_postgresql_user.db_user.name
  lc_collate = "en_US.UTF-8"
  lc_type    = "en_US.UTF-8"
}
