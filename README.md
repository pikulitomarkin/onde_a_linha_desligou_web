# Onde a Linha Desligou Web

Resumo rápido

Este repositório contém o backend (ASP.NET Core Web API) e o frontend (Angular) do projeto "Onde a Linha Desligou Web" — uma aplicação para localizar torres/linhas por quilômetro (KM) a partir de planilhas Excel e arquivos GPX com coordenadas.

Estrutura principal

- `backend/OndeALinhaDesligouWeb` — API em C# (.NET 8) que provê endpoints REST para listar linhas e buscar torres por KM.
- `frontend/onde-a-linha-desligou-web` — aplicação Angular que consome a API e exibe resultados no navegador.
- `static/resources` — pasta esperada para arquivos de dados (Excel + GPX). Esta pasta pode estar ausente no repositório e deve ser preenchida localmente.

Pré-requisitos

- .NET SDK 8.0
- Node.js (v18+ recomendado) e npm
- Git configurado com acesso ao remote (para push)

Como rodar (local)

1. Backend

```bash
cd backend/OndeALinhaDesligouWeb
dotnet restore
dotnet run --urls "http://localhost:5001"
```

2. Frontend

```bash
cd frontend/onde-a-linha-desligou-web
npm install
npm start
```

3. Testes (backend)

```bash
cd backend/tests/OndeALinhaDesligouWeb.Tests
dotnet test
```

Onde colocar os arquivos de dados (Excel/GPX)

- Os arquivos Excel e GPX devem ser colocados em `static/resources/` (caminho relativo à raiz do repositório). O `LinhaService` constrói o caminho com base em `ContentRootPath` + `LinhasOptions.ResourcesPath` (configurado em `appsettings.json`).
- Exemplos de nomes esperados (conforme `appsettings.json`):
  - `KM LON LNS.xlsx`
  - `KM LON LNS.gpx`

Expansão recomendada (próximos passos de desenvolvimento)

1. Refatoração do backend
   - Extrair `ExcelReader` e `GpxReader` para isolar I/O e parsing.
   - Criar um serviço `LinhaRepository` que encapsula a busca por torres.
   - Adicionar testes unitários abrangentes para o parsing e a lógica de seleção.

2. Melhoria da configuração
   - Permitir configuração de `ResourcesPath` via variável de ambiente ou segredos de CI.
   - Validar a existência dos arquivos e prover mensagens de erro claras no startup.

3. Observabilidade e operações
   - Expandir a configuração do Serilog (sinks para Seq/Elastic/Kusto se necessário).
   - Configurar rotação e retenção de logs.

4. CI/CD
   - Criar workflow do GitHub Actions para: build (backend), test (unit + integration), lint do frontend, e deploy (opcional).

5. UX/Frontend
   - Melhorar feedback para o usuário quando recursos estiverem ausentes (ex.: instrução clara para adicionar arquivo ou carregar um dataset de exemplo).
   - Adicionar páginas de administração para gerenciar linhas/arquivos (upload de excel/gpx).

Contribuindo

- Abra issues descrevendo bugs ou features.
- Crie PRs com branches curtos e commits atômicos.

Problemas conhecidos

- A busca por KM pode retornar 404 se os arquivos Excel/GPX não estiverem presentes em `static/resources/`.
- O projeto foi ajustado para `.NET 8` no ambiente atual; se sua máquina tiver apenas .NET 9, adapte o TargetFramework conforme necessário.

Contato

Para dúvidas sobre o repositório, abra uma issue ou entre em contato com o responsável pelo projeto.
