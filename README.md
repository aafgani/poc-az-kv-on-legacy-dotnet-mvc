# Azure Key Vault Integration on Legacy .NET Framework 4.6 (POC)

This project demonstrates how to integrate Azure Key Vault into a legacy ASP.NET MVC application targeting .NET Framework 4.6. The goal is to securely retrieve secrets (such as connection strings or app settings) from Azure Key Vault at runtime.

## Approaches

Two main approaches are implemented for accessing secrets from Azure Key Vault. The workflow is the same:
- Secrets are retrieved from Azure Key Vault during application startup.
- Retrieved values are used to replace the ConfigurationManager values, covering both AppSettings and ConnectionStrings.

### 1. `KeyVaultServiceWithClient`
- Uses the official Azure SDK (`KeyVaultClient` and `AzureServiceTokenProvider`).
- Supports Managed Identity (recommended for production in Azure environments).
- Can also use Azure CLI authentication for local development.
- **Pros:**  
  - More secure and robust for cloud deployments.
  - Supports Managed Identity (via AzureServiceTokenProvider) and Service Principal (via client id/secret or certificate). Best for workload on Azure
  - Handles authentication and token management internally. 
- **Cons:**  
  - Depends on deprecated Azure SDK libraries, e.g Microsoft.Azure.KeyVault + Microsoft.Azure.Services.AppAuthentication is the classic SDK (works fine with .NET Framework 4.6).\
- see [here](https://github.com/aafgani/poc-az-kv-on-legacy-dotnet-mvc/blob/7453812387bef057870b09f3d2a4b443fad78d9e/poc-az-kv-on-legacy-dotnet-mvc/Services/KeyVault/KeyVaultServiceWithClient.cs#L1)

### 2. `KeyVaultServiceWithHttp`
- Uses direct HTTP calls to Azure Key Vault REST API.
- Manually handles authentication via OAuth2 and token caching.
- **Pros:**  
  - No dependency on Azure SDK, just talk HTTP. 
  - Full control over HTTP requests and authentication
  - REST API is stable and documented: `GET https://{vault-name}.vault.azure.net/secrets/{secret-name}?api-version=7.3` 
- **Cons:**  
  - More code to maintain (manual token management).
  - Slightly less secure and robust compared to SDK approach.
  - Requires managing client secrets yourself (storage, rotation, expiration handling). 
- see [here](https://github.com/aafgani/poc-az-kv-on-legacy-dotnet-mvc/blob/7453812387bef057870b09f3d2a4b443fad78d9e/poc-az-kv-on-legacy-dotnet-mvc/Services/KeyVault/KeyVaultServiceWithHttp.cs#L1)

## Summary

This POC helps to choose the best method for legacy .NET Framework 4.6 application to access Azure Key Vault, depending on the deployment environment and dependency constraints.