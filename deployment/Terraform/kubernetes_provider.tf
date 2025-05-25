provider "google" {
  project = var.project_id
  region  = var.region
}

data "google_client_config" "default" {}

resource "google_container_cluster" "primary" {
  name     = var.cluster_name
  location = var.zone

  initial_node_count = 1

  node_config {
    machine_type = "e2-medium"
    oauth_scopes = [
      "https://www.googleapis.com/auth/cloud-platform"
    ]
  }
}

resource "google_container_node_pool" "primary_nodes" {
  name       = "primary-node-pool"
  location   = var.zone
  cluster    = google_container_cluster.primary.name
  node_count = 1

  node_config {
    preemptible  = false
    machine_type = "e2-medium"
    oauth_scopes = [
      "https://www.googleapis.com/auth/cloud-platform"
    ]
  }
}

resource "google_container_registry" "default" {}

provider "kubernetes" {
  host                   = google_container_cluster.primary.endpoint
  token                  = data.google_client_config.default.access_token
  cluster_ca_certificate = base64decode(google_container_cluster.primary.master_auth[0].cluster_ca_certificate)
}

resource "kubernetes_deployment" "webapi" {
  metadata {
    name = "meeting-webapi"
    labels = {
      app = "meeting-webapi"
    }
  }
  spec {
    replicas = 1
    selector {
      match_labels = {
        app = "meeting-webapi"
      }
    }
    template {
      metadata {
        labels = {
          app = "meeting-webapi"
        }
      }
      spec {
        container {
          image = "gcr.io/${var.project_id}/meeting-webapi:${var.image_tag}"
          name  = "meeting-webapi"
          port {
            container_port = 5000
          }
        }
      }
    }
  }
}

resource "kubernetes_service" "webapi" {
  metadata {
    name = "meeting-webapi-service"
  }
  spec {
    selector = {
      app = kubernetes_deployment.webapi.metadata[0].labels.app
    }
    port {
      port        = 80
      target_port = 5000
    }
    type = "LoadBalancer"
  }
}