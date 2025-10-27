# Copilot - Onde a Linha Desligou Web (Backend C#)

Este documento descreve o status atual do projeto de backend em C# e fornece uma lista de próximos passos recomendados para concluir e melhorar a aplicação.

## Status do Projeto

O backend é uma aplicação Web API em .NET Core que fornece endpoints para gerenciar "Linhas" e "Torres".

-   **`Program.cs`**: Configura a aplicação web, registra os serviços e configura o pipeline de requisições HTTP. O `LinhaService` está registrado como singleton.
-   **`Models/Linha.cs` e `Models/Torre.cs`**: Definem os modelos de dados para a aplicação.
-   **`Controllers/LinhasController.cs`**: Define os endpoints da API:
    -   `GET /api/Linhas`: Retorna todas as linhas.
    -   `GET /api/Linhas/{grupo}`: Retorna as linhas por grupo ("Londrina" ou "Campo Mourão").
    -   `GET /api/Linhas/buscar`: Procura por uma "Torre" com base na chave da linha e valores de KM.
-   **`Services/LinhaService.cs`**: Contém a lógica de negócio principal:
    -   Possui uma lista fixa (hardcoded) de "Linhas" e seus arquivos Excel e GPX correspondentes.
    -   O método `BuscarTorre` lê dados de um arquivo Excel para encontrar a torre mais próxima com base nos valores de KM e, em seguida, recupera as coordenadas da torre de um arquivo GPX.
    -   Inclui lógica para analisar e ajustar os códigos das torres.

## Próximos Passos e Tarefas

Aqui está uma lista de tarefas que podem ser feitas para melhorar e concluir o projeto:

1.  **Tratamento de Erros e Logging**:
    -   Implementar um manipulador de exceções global para capturar exceções não tratadas e retornar uma resposta de erro padronizada.
    -   Adicionar logging estruturado (por exemplo, usando Serilog) para registrar requisições, erros e outras informações importantes. Isso tornará a depuração e o monitoramento muito mais fáceis.

2.  **Gerenciamento de Configuração**:
    -   A lista de "Linhas" e os mapeamentos de arquivos GPX estão fixos no código do `LinhaService`. Isso deve ser movido para um arquivo de configuração como o `appsettings.json`. Isso tornaria mais fácil a atualização dos dados sem alterar o código.

3.  **Refatorar o `LinhaService`**:
    -   O `LinhaService` está fazendo muitas coisas. Ele poderia ser dividido em serviços menores e mais focados. Por exemplo, um serviço para ler arquivos Excel e outro para ler arquivos GPX.
    -   Os métodos `AjustarCodigoTorre` e `ExtrairApenasNumero` têm uma lógica complexa que poderia ser simplificada e melhor documentada.

4.  **Testes Unitários e de Integração**:
    -   Não há testes no projeto. Testes unitários devem ser criados para o `LinhaService` para testar a lógica de negócio, especialmente a lógica de busca de torres e ajuste de código.
    -   Testes de integração devem ser criados para o `LinhasController` para testar os endpoints da API.

5.  **Documentação da API**:
    -   A API está usando Swagger, o que é ótimo. A documentação do Swagger pode ser melhorada adicionando mais detalhes sobre os endpoints, parâmetros e respostas.

6.  **Segurança**:
    -   A API está aberta para todos. Dependendo do caso de uso, você pode querer adicionar autenticação e autorização para proteger os endpoints.

7.  **CI/CD (Integração Contínua/Implantação Contínua)**:
    -   Configurar um pipeline de CI/CD (por exemplo, usando GitHub Actions) para construir, testar e implantar a aplicação automaticamente.

## Estado atual (detalhado) e o que falta

- Recursos ausentes: para que a busca por KM funcione end-to-end é necessário adicionar os arquivos de dados em `static/resources/` (arquivos Excel e GPX). Exemplo de nomes esperados pela configuração atual:
    - `KM LON LNS.xlsx` (planilha Excel com KM e códigos de torre para a linha `lonlns`)
    - `KM LON LNS.gpx` (arquivo GPX com coordenadas das torres para a linha `lonlns`)
    - Para outras linhas, use os nomes conforme configurado em `appsettings.json` sob `LinhasConfiguration`.

- Testes:
    - Foi adicionado um projeto de testes de integração (`backend/tests/OndeALinhaDesligouWeb.Tests`) com um teste que valida `GET /api/linhas` (passou localmente). Ainda faltam testes unitários para a lógica de parsing/seleção de torres.

- Refatoração recomendada (próximos passos de desenvolvimento):
    - Extrair leitura de Excel para um serviço `ExcelReader` e leitura de GPX para um `GpxReader`.
    - Adicionar testes unitários que cubram as funções de parsing (`AjustarCodigoTorre`, `ExtrairApenasNumero`) e as buscas por KM.

- Git/CI:
    - Houve várias modificações locais (adição do middleware de exceção, Serilog, `LinhasOptions`, testes e mudanças em `Program.cs`). Estas alterações serão commitadas e empurradas para `origin/main` quando o push for possível a partir deste ambiente (pode requerer autenticação/configuração de remote se não estiver configurada).

## Como rodar localmente (rápido)

Pré-requisitos:

- .NET SDK 8.0 (o projeto foi ajustado para `net8.0` no ambiente de desenvolvimento atual).
- Node.js (recomendado v18+) e npm

Backend (API):

1. Vá para `backend/OndeALinhaDesligouWeb`
2. Restaurar e rodar:

```bash
dotnet restore
dotnet run --urls "http://localhost:5001"
```

Frontend (Angular):

1. Vá para `frontend/onde-a-linha-desligou-web`
2. Instale dependências e execute:

```bash
npm install
npm start
```

Testes:

```bash
cd backend/tests/OndeALinhaDesligouWeb.Tests
dotnet test
```

## Observações finais

- Se a busca por KM retornar erro 404, verifique os logs do backend (serilog) e confirme se os arquivos esperados existem em `static/resources/`.
- Se houver problemas no push para o repositório remoto, verifique as credenciais git/SSH ou use o GitHub CLI para autenticar (`gh auth login`).

