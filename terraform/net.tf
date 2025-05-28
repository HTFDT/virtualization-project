resource "yandex_vpc_network" "vpc_network" {
  name = "almetov-project-network"
}

resource "yandex_vpc_subnet" "subnet_a" {
  name           = "almetov-project-ru-central1-a"
  network_id     = yandex_vpc_network.vpc_network.id
  v4_cidr_blocks = ["192.168.1.0/24"]
  zone           = "ru-central1-a"
}

resource "yandex_vpc_subnet" "subnet_b" {
  name           = "almetov-project-ru-central1-b"
  network_id     = yandex_vpc_network.vpc_network.id
  v4_cidr_blocks = ["192.168.2.0/24"]
  zone           = "ru-central1-b"
}

resource "yandex_vpc_subnet" "subnet_d" {
  name           = "almetov-project-ru-central1-d"
  network_id     = yandex_vpc_network.vpc_network.id
  v4_cidr_blocks = ["192.168.3.0/24"]
  zone           = "ru-central1-d"
}