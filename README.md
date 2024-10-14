# # Azure Key Vault Integration - PoC

Este projeto é uma Prova de Conceito (PoC) que demonstra como integrar uma API .NET 6 com o **Azure Key Vault** para recuperar segredos (usuário e senha) de maneira segura. Ele utiliza o `DefaultAzureCredential` para autenticação automática, que suporta várias opções de autenticação, como o Visual Studio e a Azure CLI, dependendo do ambiente.

## Tecnologias Utilizadas

- **.NET 6**
- **Azure Key Vault**
- **Azure.Identity** (para autenticação)
- **Azure.Security.KeyVault.Secrets** (para acessar segredos)
- **Visual Studio** para autenticação local

## Funcionalidades

- Recupera segredos armazenados no Azure Key Vault utilizando o `DefaultAzureCredential`.
- Suporte para diferentes métodos de autenticação em desenvolvimento e produção.
- API configurada para autenticar automaticamente no Key Vault sem a necessidade de armazenar credenciais sensíveis diretamente no código.

## Pré-requisitos

- **Conta no Azure** com permissões para criar e gerenciar recursos.
- **Azure Key Vault** criado e configurado com os segredos `ApiUser` e `ApiPassword`.
- **Visual Studio** com a extensão do Azure e conta autenticada.
- **Azure CLI** (opcional para testes locais fora do Visual Studio).

## Configuração do Azure Key Vault

### Criar um Azure Key Vault
1. No portal do Azure, vá para **Criar um Recurso** > **Key Vault**.
2. Preencha as informações necessárias e crie o Key Vault.
3. No Key Vault, vá para **Segredos** e adicione dois segredos:
   - Nome: `ApiUser` | Valor: `<seu-usuario>`
   - Nome: `ApiPassword` | Valor: `<sua-senha>`

### Permissões (RBAC)

1. Vá para a seção **Controle de Acesso (IAM)** do Key Vault.
2. Adicione as seguintes funções:
   - **Usuário de Segredos do Cofre de Chaves**: Permite que a aplicação leia segredos do Key Vault.

## Configuração Local para Desenvolvimento

### Autenticação via Visual Studio

1. Certifique-se de que sua conta do Azure está autenticada no Visual Studio.
2. No Visual Studio, vá para `Tools > Options > Azure Service Authentication` e selecione sua conta.
3. As permissões do Azure Key Vault devem estar configuradas para a identidade usada no Visual Studio.

### Recuperação de Segredos no Código

```csharp
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

public class SecretService
{
    private readonly SecretClient _secretClient;

    public SecretService(IConfiguration configuration)
    {
        string vaultUri = configuration["AzureKeyVault:VaultUri"];
        _secretClient = new SecretClient(new Uri(vaultUri), new DefaultAzureCredential());
    }

    public async Task<string> GetSecretAsync(string secretName)
    {
        KeyVaultSecret secret = await _secretClient.GetSecretAsync(secretName);
        return secret.Value;
    }
}


``` appsettings.json

{
  "AzureKeyVault": {
    "VaultUri": "https://<nome-do-seu-keyvault>.vault.azure.net/"
  }
}



## Autenticação em Produção
Para ambientes de produção, recomenda-se utilizar Managed Identity para autenticar de maneira segura sem a necessidade de armazenar credenciais sensíveis. A DefaultAzureCredential automaticamente tentará usar a Managed Identity quando a aplicação estiver em execução em um serviço do Azure.

Habilite a Managed Identity no serviço Azure onde sua aplicação será executada (por exemplo, Azure App Service).
Configure as permissões do Key Vault para permitir que a Managed Identity acesse os segredos.

## Erros Comuns:

SecretNotFound: O segredo especificado não foi encontrado no Key Vault. Verifique se o nome está correto e se o segredo existe.
Forbidden: A identidade usada não tem permissões suficientes para acessar o Key Vault. Certifique-se de que as funções corretas foram atribuídas.

## Referências:

Documentação do Azure Key Vault
Azure.Identity - Autenticação em .NET
Azure.Security.KeyVault.Secrets

## Contribuindo:

Se quiser contribuir com o projeto, sinta-se à vontade para abrir um pull request ou relatar problemas via GitHub Issues.
