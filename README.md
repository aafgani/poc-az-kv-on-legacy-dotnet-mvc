# Azure Key Vault Integration on Legacy .NET Framework 4.6 (POC)

This project demonstrates how to integrate Azure Key Vault into a legacy ASP.NET MVC application targeting .NET Framework 4.6. The goal is to securely retrieve secrets (such as connection strings or app settings) from Azure Key Vault at runtime.

## Motivation

This approach addresses a critical security issue flagged by SonarQube:  
**"Hard-coded credentials are security-sensitive."**  
Hard-coding credentials in source code or binaries makes it easy for attackers to extract sensitive information, especially in distributed or open-source applications. This practice has led to real-world vulnerabilities, such as:

- [CVE-2019-13466](https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2019-13466)
- [CVE-2018-15389](https://cve.mitre.org/cgi-bin/cvename.cgi?name=CVE-2018-15389)

**Best practice:**  
Credentials should be stored outside of the code, in a configuration file, a database, or a dedicated secrets management service like Azure Key Vault.

## Approaches

Two main approaches are implemented for accessing secrets from Azure Key Vault. The workflow is the same:
- Secrets are retrieved from Azure Key Vault during application startup.
- Retrieved values are used to replace the ConfigurationManager values, covering both AppSettings and ConnectionStrings.

### 1. `KeyVaultServiceWithClient`
- Uses the official Azure SDK (`KeyVaultClient` and `AzureServiceTokenProvider`).
- Supports Managed Identity (recommended for production in Azure environments). Managed Identity is a feature of Azure Active Directory (Azure AD) that provides an automatically managed identity in Azure. It allows you to authenticate with Azure services without needing to manage credentials.
- Can also use Azure CLI authentication for local development.
- **Pros:**  
  - More secure and robust for cloud deployments.
  - Supports Managed Identity (via AzureServiceTokenProvider) and Service Principal (via client id/secret or certificate). Best suited for workloads running on Azure.
  - Handles authentication and token management internally. 
- **Cons:**  
  - Depends on deprecated Azure SDK libraries, e.g Microsoft.Azure.KeyVault + Microsoft.Azure.Services.AppAuthentication is the classic SDK (works fine with .NET Framework 4.6).
- See Implementation [here](https://github.com/aafgani/poc-az-kv-on-legacy-dotnet-mvc/blob/7453812387bef057870b09f3d2a4b443fad78d9e/poc-az-kv-on-legacy-dotnet-mvc/Services/KeyVault/KeyVaultServiceWithClient.cs#L1)

### 2. `KeyVaultServiceWithHttp`
- Implements direct HTTP calls to the Azure Key Vault REST API.
- Handles authentication manually via OAuth2 and token caching.
- Uses a Service Principal (an application identity in Azure AD) to authenticate and access Azure resources.
- **Pros:**  
  - No dependency on Azure SDK, uses only HTTP requests. 
  - Full control over HTTP requests and authentication flow.
  - REST API is stable and documented: (example : `GET https://{vault-name}.vault.azure.net/secrets/{secret-name}?api-version=7.3`)
- **Cons:**  
  - More code to maintain (manual token acquisition and caching).
  - Slightly less secure and robust compared to SDK-based approach.
  - Requires managing client secrets yourself (storage, rotation, expiration handling). 
- See Implementation [here](https://github.com/aafgani/poc-az-kv-on-legacy-dotnet-mvc/blob/7453812387bef057870b09f3d2a4b443fad78d9e/poc-az-kv-on-legacy-dotnet-mvc/Services/KeyVault/KeyVaultServiceWithHttp.cs#L1)

## Summary

This POC helps to choose the best method for legacy .NET Framework 4.6 application to access Azure Key Vault, depending on the deployment environment and dependency constraints.