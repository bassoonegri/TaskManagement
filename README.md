# TaskManagement API

API para gerenciamento de projetos e tarefas.

---

![Coverage](https://img.shields.io/badge/coverage-84%25-brightgreen)

---

## Acessar

- Swagger: [http://localhost:8080/swagger](http://localhost:8080/swagger)

---

## Funcionalidades

- Criar, listar e gerenciar Projetos
- Criar, listar, atualizar e remover Tarefas dentro de projetos
- Adicionar Comentários em Tarefas
- Registrar Histórico de alterações em Tarefas
- Gerar Relatório de Desempenho (acesso restrito a Gerentes)

---

## Restrições de negócio

- Cada projeto pode ter no máximo **20 tarefas**
- A prioridade da tarefa não pode ser alterada após a criação
- Não é possível excluir projetos com tarefas pendentes
- Cada comentário gera registro no histórico da tarefa
- Apenas usuários com função **gerente** podem gerar relatórios

---

## Principais Endpoints

| Método | Rota | Descrição |
|:---|:---|:---|
| GET | /projects?userId={userId} | Listar projetos do usuário |
| POST | /projects | Criar novo projeto |
| GET | /projects/{projectId}/tasks | Listar tarefas de um projeto |
| POST | /projects/{projectId}/tasks | Criar nova tarefa |
| DELETE | /projects/{taskId} | Remover uma projeto |
| PUT | /tasks/{taskId} | Atualizar uma tarefa |
| DELETE | /tasks/{taskId} | Remover uma tarefa |
| POST | /tasks/{taskId}/comments | Adicionar comentário em uma tarefa |
| GET | /reports/performance?role=manager | Gerar relatório de desempenho |

---

## Rodar Testes Unitários

Para executar todos os testes:

```bash
dotnet test
```

Para executar testes com cobertura de código:

```bash
dotnet test tests/TaskManagement.Tests/TaskManagement.Tests.csproj --collect:"XPlat Code Coverage"
```

Para gerar o relatório visual:

```bash
reportgenerator -reports:"tests/TaskManagement.Tests/TestResults/*/coverage.cobertura.xml" -targetdir:"coveragereport"
```

Para abrir o relatório:

```bash
start coveragereport/index.html
```

✅ Cobertura mínima exigida: **80%**

---

## Buildar

Para buildar a aplicação, resetar banco e popular dados:

```bash
./build.ps1
```

---

## Estrutura do Projeto

```plaintext
src/
  TaskManagement.Api/
  TaskManagement.Application/
    Entities/
  TaskManagement.Infrastructure/
    Context/
    Services/
tests/
  TaskManagement.Tests/
    Services/
    Controllers/
db/
  Insert_initiate.sql
docker-compose.yml
docker-compose.override.yml
.gitignore
.dockerignore
build.ps1
README.md
```

---

## Fase 2: Refinamento

**Perguntas para o Product Owner (PO):**

- Será necessário autenticação e autorização de usuários?
- Projetos poderão ser compartilhados entre usuários ou sempre serão privados?
- As tarefas terão prazos para lembretes automáticos de vencimento?
- Será possível adicionar anexos (documentos, imagens) nas tarefas?
- Haverá integração com outras ferramentas (como e-mail ou notificações push)?
- Pretende-se implementar Kanban (mudança de status visual)?
- O histórico de alterações deve guardar todos os detalhes de cada campo modificado?

---

## Fase 3: Melhorias sugeridas

**Visão para evolução futura do projeto:**

- Implementar autenticação e autorização usando JWT
- Criar controle de papéis (usuário padrão e gerente)
- Aplicar arquitetura CQRS + Mediatr para melhor escalabilidade
- Dividir a aplicação em camadas Domain, Application, Infrastructure seguindo DDD
- Implementar logs estruturados (Serilog) e monitoramento via Application Insights
- Utilizar Docker Compose para orquestração de ambientes (dev, staging, prod)
- Implementar CI/CD com GitHub Actions
- Subir o sistema em ambiente Kubernetes
- Uso de mensageria com RabbitMQ para eventos assíncronos (ex.: novos comentários)
- Evoluir para um frontend SPA (React ou Angular) conectado à API

---

## Tecnologias Utilizadas

- ASP.NET Core 8
- Entity Framework Core 8
- PostgreSQL
- xUnit, FluentAssertions, Moq (Testes)
- Docker
- Azure Data Studio
- ReportGenerator (Relatório de cobertura)
