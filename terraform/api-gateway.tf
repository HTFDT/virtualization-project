resource "yandex_api_gateway" "api_gateway" {
  name = "almetov-project-api-gw"
  log_options {
    min_level = "TRACE"
  }
  execution_timeout = "300"
  spec              = <<-EOT
openapi: 3.0.0
info:
  title: Almetov project API
  version: 1.0.0
x-yc-apigateway:
  service_account: ${yandex_iam_service_account.sa_api_gateway.id}
paths:
  /{path+}:
    x-yc-apigateway-any-method:
      parameters:
      - name: path
        in: path
        required: false
        schema:
          type: string
      x-yc-apigateway-integration:
        type: http
        url: http://${yandex_vpc_address.load_balancer_ip.external_ipv4_address[0].address}/{path}
        headers:
          '*': '*'
        query:
          '*': '*'
        timeouts:
          connect: 0.5
          read: 5
  /api/files:
    get:
      parameters:
        - name: file
          in: query
          required: true
          schema:
            type: string
      x-yc-apigateway-integration:
        type: object-storage
        bucket: almetov-project-bucket
        object: '{file}'
EOT
}