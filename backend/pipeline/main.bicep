param location string = 'westeurope'
@secure()
param sqlAdmin string 
@secure()
param sqlPassword string

var sqlServerName = 'crud-play-sqlserver-${uniqueString(resourceGroup().id)}'
var sqlDatabaseName = 'CrudPlay'
var keyVaultName = 'crud-play-keyvault'
var appServicePlanName = 'crud-play-serviceplan'
var webAppName = 'crud-play-api'

resource sqlServer 'Microsoft.Sql/servers@2023-08-01' = {
  name: sqlServerName
  location: location
  properties: {
    administratorLogin: sqlAdmin
    administratorLoginPassword: sqlPassword
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-08-01' = {
  name: sqlDatabaseName
  parent: sqlServer
  location: location
  properties: {
    status: 'Online'
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2024-11-01' = {
  name: keyVaultName
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    accessPolicies: []
  }
}

resource appServicePlan 'Microsoft.Web/serverFarms@2024-04-01' = {
  name: appServicePlanName
  location: location
  sku: {
    tier: 'Basic'
    name: 'B1'
  }
}

resource webApp 'Microsoft.Web/sites@2024-04-01' = {
  name: webAppName
  location: location
  properties: {
    serverFarmId: appServicePlan.id
  }
}

resource webAppConfig 'Microsoft.Web/sites/config@2024-04-01' = {
  name: 'web'
  parent: webApp
  properties: {
    appSettings: [
      {
        name: 'DOTNET_ENVIRONMENT'
        value: 'Production'
      }
      {
        name: 'PersistenceOptions__ConnectionString'
        value: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/PersistenceOptions-ConnectionString/)'
      }
      {
        name: 'PersistenceOptions-Implementation'
        value: 'EntityFramework'
      }
      {
        name: 'JwtConfiguration__Audience'
        value: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/JwtConfiguration-Audience/)'
      }
      {
        name: 'JwtConfiguration__Authority'
        value: 'https://${webAppName}.azurewebsites.net'
      }
      {
        name: 'JwtConfiguration__Issuer'
        value: 'https://${webAppName}.azurewebsites.net'
      }
      {
        name: 'JwtConfiguration__SecretKey'
        value: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/JwtConfiguration-SecretKey/)'
      }
      {
        name: 'CorsOptions__AllowedOrigins'
        value: '@Microsoft.KeyVault(SecretUri=https://${keyVaultName}.vault.azure.net/secrets/CorsOptions-AllowedOrigins/)'
      }
    ]
    cors: {
      allowedOrigins: ['http://localhost:4200','https://yourgithubpagesurl.github.io']
    }
  }
}
