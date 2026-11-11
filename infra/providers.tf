terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~>5.13.0"
    }
  }

  backend "azurerm" {}
}

provider "azurerm" {
  features {}
}