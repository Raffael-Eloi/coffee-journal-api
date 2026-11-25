resource "azurerm_resource_group" "main" {
  name     = "rg-${var.application_name}-${var.environment_name}"
  location = var.region
  tags     = local.tags
}

resource "azurerm_service_plan" "main" {
  name                = "asp-${var.application_name}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  os_type             = "Linux"
  sku_name            = "B1"
  tags                = local.tags
}

resource "azurerm_linux_web_app" "main" {
  name                = "app-${var.application_name}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_service_plan.main.location
  service_plan_id     = azurerm_service_plan.main.id
  tags                = local.tags

  app_settings = {
    WEBSITES_PORT                   = "8080"
    "BlobStorage__ConnectionString" = azurerm_storage_account.main.primary_connection_string
    "BlobStorage__ContainerName"    = azurerm_storage_container.main.name
  }

  connection_string {
    name  = "CoffeeJournal"
    type  = "SQLAzure"
    value = "Server=tcp:${azurerm_mssql_server.main.fully_qualified_domain_name},1433;Initial Catalog=${azurerm_mssql_database.main.name};User ID=${var.sql_admin_login};Password=${var.sql_admin_password};Encrypt=True;"
  }

  site_config {
    always_on = false

    application_stack {
      docker_image_name   = "nginxinc/nginx-unprivileged:latest"
      docker_registry_url = "https://index.docker.io"
    }
  }

  lifecycle {
    ignore_changes = [site_config[0].application_stack, app_settings]
  }
}

# Database
resource "azurerm_mssql_server" "main" {
  name                         = "coffee-journal-sqlserver"
  resource_group_name          = azurerm_resource_group.main.name
  location                     = azurerm_resource_group.main.location
  version                      = "12.0"
  administrator_login          = var.sql_admin_login
  administrator_login_password = var.sql_admin_password
  tags                         = local.tags
}

resource "azurerm_mssql_database" "main" {
  name        = "coffee-journal-db"
  server_id   = azurerm_mssql_server.main.id
  collation   = "SQL_Latin1_General_CP1_CI_AS"
  max_size_gb = 2
  sku_name    = "Basic"

  tags = local.tags

  # prevent the possibility of accidental data loss
  lifecycle {
    prevent_destroy = true
  }
}

# Allow Azure services (such as the web app) to reach the SQL server
resource "azurerm_mssql_firewall_rule" "allow_azure_services" {
  name             = "AllowAzureServices"
  server_id        = azurerm_mssql_server.main.id
  start_ip_address = "0.0.0.0"
  end_ip_address   = "0.0.0.0"
}

# Azure blob storage

resource "azurerm_storage_account" "main" {
  name                            = "stcoffeejournal${var.environment_name}"
  resource_group_name             = azurerm_resource_group.main.name
  location                        = azurerm_resource_group.main.location
  account_tier                    = "Standard"
  account_replication_type        = "LRS"
  allow_nested_items_to_be_public = true
  tags                            = local.tags
}

resource "azurerm_storage_container" "main" {
  name                  = "coffee-journal-images"
  storage_account_id    = azurerm_storage_account.main.id
  container_access_type = "blob"
}
