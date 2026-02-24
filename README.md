# 🎓 Plataforma de Estágios – MVP

Sistema desenvolvido em **.NET 8** para gerenciamento de estágios, utilizando **Blazor Server** no frontend e **ASP.NET Core Web API** no backend. O projeto conta com uma arquitetura baseada em **Clean Architecture** e adota as melhores práticas de engenharia de software.

---

## 📐 Arquitetura

O projeto é organizado em camadas com responsabilidades bem definidas, visando o isolamento e a facilidade de manutenção:

```text
📦 PlataformaEstagios
 ├── 📁 PlataformaEstagios.Presentation   → API / Blazor Server
 ├── 📁 PlataformaEstagios.Application    → Casos de Uso
 ├── 📁 PlataformaEstagios.Domain         → Entidades e Regras de Negócio
 ├── 📁 PlataformaEstagios.Infrastructure → Persistência e Implementações
 └── 📁 Shared
      ├── 📁 Communication
      └── 📁 Exceptions
```

### 🔹 Domain
* **Entidades:** Representação dos modelos de dados fundamentais.
* **Regras de negócio:** Lógica central da aplicação.
* **Objetos de valor:** Estruturas imutáveis para compor o domínio.
* **Independência:** Totalmente isolado de frameworks externos.

### 🔹 Application
* **Implementação dos Use Cases:** Fluxos de execução da aplicação.
* **Validações isoladas:** Tratamento de dados separado por responsabilidade.
* **Interfaces (Ports):** Contratos para comunicação com camadas externas.
* **Orquestração do domínio:** Coordenação das regras de negócio.

### 🔹 Infrastructure
* **Entity Framework Core:** ORM utilizado.
* **Implementação de Repositories:** Acesso a dados abstrato.
* **Unit of Work:** Controle transacional das operações.
* **Configuração de banco de dados:** Mapeamentos e integrações.

### 🔹 Presentation
* **Controllers da API:** Exposição de endpoints (REST).
* **Configuração de DI:** Registro da injeção de dependência.
* **Blazor Server (UI):** Interface do usuário.
* **Middlewares:** Interceptadores de requisições e exceções.

---

## 🛠 Stack Tecnológica

* **.NET 8**
* **ASP.NET Core Web API**
* **Blazor Server**
* **Entity Framework Core**
* **PostgreSQL**
* **NUnit** (Testes Unitários)
* **Injeção de Dependência** (Nativa do .NET)

---

## 🔧 Padrões e Princípios Aplicados

* **Clean Architecture**
* **SOLID**
* **Repository Pattern**
* **Unit of Work**
* **Dependency Injection (DI)**
* **Separação de validações**
* **Encapsulamento das regras de negócio** no domínio

---

## 🗄 Banco de Dados

**Banco utilizado:** PostgreSQL

### Criação do banco local

Execute o script abaixo no seu servidor PostgreSQL para provisionar o banco de dados e o usuário da aplicação:

```sql
CREATE USER estagios WITH PASSWORD 'estagiospwd';
CREATE DATABASE plataforma_estagios OWNER estagios;
GRANT ALL PRIVILEGES ON DATABASE plataforma_estagios TO estagios;
```

### Connection String (`appsettings.json`)

Configure a sua string de conexão no arquivo de configurações da aplicação:

```json
"ConnectionStrings": {
  "PlataformaEstagios": "Host=localhost;Port=5432;Database=plataforma_estagios;Username=estagios;Password=estagiospwd"
}
```

---

## 🚀 Como Executar o Projeto

### 1️⃣ Pré-requisitos

Certifique-se de ter as seguintes ferramentas instaladas:
* **.NET SDK 8+**
* **PostgreSQL 16+**
* **Visual Studio 2022** (17.8+ recomendado) ou VS Code

Para verificar o SDK instalado, execute no terminal:
```bash
dotnet --list-sdks
```

### 2️⃣ Aplicar Migrations

Dentro do diretório do projeto da API (ou via Package Manager Console apontando para o projeto de Infrastructure), aplique as migrações para criar as tabelas no banco:
```bash
dotnet ef database update
```

### 3️⃣ Executar aplicação

Inicie o projeto através do terminal:
```bash
dotnet run
```
> **Nota:** Alternativamente, você pode definir o projeto `Presentation` como *Startup Project* no Visual Studio e iniciar com `F5` ou `Ctrl+F5`.

---

## 📌 Funcionalidades do MVP

* ✅ Cadastro de estudantes
* ✅ Cadastro de empresas
* ✅ Cadastro de vagas
* ✅ Aplicação em vagas
* ✅ Listagem e gerenciamento de candidaturas

---

## 🎯 Objetivo Técnico

O projeto foi desenvolvido com foco primário em:
* **Estruturação arquitetural escalável** para facilitar o crescimento do software.
* **Separação clara de responsabilidades**, reduzindo o acoplamento.
* **Manutenção facilitada**, com código limpo e testável.
* **Evolução futura**, preparando o terreno para implementação de autenticação (ex: JWT/Identity), logging estruturado e deploy em ambientes Cloud.
