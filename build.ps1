# build.ps1

Write-Host "Limpando a solução..."
dotnet clean

Write-Host "Buildando a solução..."
dotnet build

Write-Host "Aplicando Migrations no Banco de Dados..."
dotnet ef database update --project src/TaskManagement.Infrastructure --startup-project src/TaskManagement.Api

Write-Host "Executando Testes Unitários..."
dotnet test tests/TaskManagement.Tests/TaskManagement.Tests.csproj --collect:"XPlat Code Coverage"

Write-Host "Gerando Relatório de Cobertura de Testes..."
reportgenerator -reports:"tests/TaskManagement.Tests/TestResults/*/coverage.cobertura.xml" -targetdir:"coveragereport"

Write-Host "Processo concluído! Relatório de cobertura disponível em: coveragereport/index.html"
