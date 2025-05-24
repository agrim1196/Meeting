variable "project_id" {
  description = "golden-system-279417"
  type        = string
}

variable "region" {
  description = "GCP region"
  type        = string
  default     = "us-central1"
}

variable "zone" {
  description = "GCP zone"
  type        = string
  default     = "us-central1-a"
}

variable "cluster_name" {
  description = "GKE cluster name"
  type        = string
  default     = "meeting-cluster"
}

variable "image_tag" {
  description = "Docker image tag"
  type        = string
  default     = "latest"
}