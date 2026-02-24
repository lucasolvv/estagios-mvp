Plataforma de Estágios – MVP

Sistema desenvolvido em .NET 8 para gerenciamento de estágios, utilizando Blazor Server no frontend e ASP.NET Core Web API no backend, com arquitetura baseada em Clean Architecture e boas práticas de engenharia de software.

📐 Arquitetura

O projeto é organizado em camadas com responsabilidades bem definidas:

📦 PlataformaEstagios
 ├── PlataformaEstagios.Presentation   → API / Blazor Server
 ├── PlataformaEstagios.Application    → Casos de Uso
 ├── PlataformaEstagios.Domain         → Entidades e Regras de Negócio
 ├── PlataformaEstagios.Infrastructure → Persistência e Implementações
 └── Shared
      ├── Communication
      └── Exceptions
🔹 Domain

Entidades

Regras de negócio

Objetos de valor

Independente de frameworks

🔹 Application

Implementação dos Use Cases

Validações isoladas por responsabilidade

Interfaces (ports)

Orquestração do domínio

🔹 Infrastructure

Entity Framework Core

Implementação de Repositories

Unit of Work

Configuração de banco de dados

🔹 Presentation

Controllers da API

Configuração de DI

Blazor Server (UI)

Middlewares

🛠 Stack Tecnológica

.NET 8

ASP.NET Core Web API

Blazor Server

Entity Framework Core

PostgreSQL

NUnit (Testes Unitários)

Injeção de Dependência nativa

🔧 Padrões e Princípios Aplicados

Clean Architecture

SOLID

Repository Pattern

Unit of Work

Dependency Injection

Separação de validações

Encapsulamento das regras de negócio no domínio

🗄 Banco de Dados

Banco utilizado: PostgreSQL

Criação do banco local:
CREATE USER estagios WITH PASSWORD 'estagiospwd';
CREATE DATABASE plataforma_estagios OWNER estagios;
GRANT ALL PRIVILEGES ON DATABASE plataforma_estagios TO estagios;
Connection String (appsettings.json)
"ConnectionStrings": {
  "PlataformaEstagios": "Host=localhost;Port=5432;Database=plataforma_estagios;Username=estagios;Password=estagiospwd"
}
🚀 Como Executar o Projeto
1️⃣ Pré-requisitos

.NET SDK 8+

PostgreSQL 16+

Visual Studio 2022 (17.8+ recomendado)

Verificar SDK instalado:

dotnet --list-sdks
2️⃣ Aplicar Migrations

Dentro do projeto da API:

dotnet ef database update
3️⃣ Executar aplicação
dotnet run

Ou definir o projeto Presentation como Startup no Visual Studio.

🧪 Testes

Framework utilizado: NUnit

Executar testes:

dotnet test

Os testes cobrem:

Casos de uso

Fluxos de sucesso e falha

Validações

Comportamento esperado dos controllers

📌 Funcionalidades do MVP

Cadastro de estudantes

Cadastro de empresas

Cadastro de vagas

Aplicação em vagas

Listagem e gerenciamento de candidaturas

🎯 Objetivo Técnico

O projeto foi desenvolvido com foco em:

Estruturação arquitetural escalável

Separação clara de responsabilidades

Manutenção facilitada

Evolução futura para autenticação, logging estruturado e deploy em ambiente cloud
