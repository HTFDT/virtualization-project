resource "yandex_lb_network_load_balancer" "load_balancer" {
  name = "almetov-project-load-balancer"

  listener {
    name        = "almetov-project-listener"
    port        = 80
    target_port = 80
    external_address_spec {
      ip_version = "ipv4"
      address    = yandex_vpc_address.load_balancer_ip.external_ipv4_address[0].address
    }
  }

  attached_target_group {
    target_group_id = yandex_compute_instance_group.vm_group.load_balancer[0].target_group_id

    healthcheck {
      name = "almetov-project-healthcheck"
      tcp_options {
        port = 80
      }
    }
  }
}

resource "yandex_vpc_address" "load_balancer_ip" {
  name = "almetov-project-load-balancer-address"

  external_ipv4_address {
    zone_id = "ru-central1-a"
  }
}