variable "region" {
  type = string
}

variable "environment_name" {
  type = string
}

variable "application_name" {
  type = string
}

variable "owner" {
  type = string
}

variable "managed_by" {
  type = string
}

variable "cost_center" {
  type = string
}

variable "sql_admin_login" {
  type      = string
  sensitive = true
}

variable "sql_admin_password" {
  type      = string
  sensitive = true
}
