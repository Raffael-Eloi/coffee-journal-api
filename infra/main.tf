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
  sku_name            = "F1"
  tags                = local.tags
}

resource "azurerm_linux_web_app" "main" {
  name                = "app-${var.application_name}"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_service_plan.main.location
  service_plan_id     = azurerm_service_plan.main.id
  tags                = local.tags

  app_settings = {
    WEBSITES_PORT = "8080"
  }

  site_config {
    always_on = false

    application_stack {
      docker_image_name   = var.docker_image_name
      docker_registry_url = "https://index.docker.io"
    }
  }

  lifecycle {
    ignore_changes = [site_config[0].application_stack]
  }
}