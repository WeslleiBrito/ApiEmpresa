# ApiEmpresa

## 📌 Visão Geral

**ApiEmpresa** é uma API REST desenvolvida como projeto acadêmico em **ASP.NET Core (.NET 9)**, com foco em boas práticas de arquitetura, persistência de dados e deploy em ambiente cloud.

A API permite o gerenciamento completo de **empresas**, **funcionários**, **setores**, **habilidades** e **endereços**, aplicando regras de negócio, validações e relacionamentos entre entidades.

---

## 🌐 Deploy em Produção

A aplicação está publicada e acessível na AWS (EC2) utilizando **Docker e Docker Compose**.

🔗 **Swagger – Ambiente de Produção:**

👉 [http://54.207.110.120/swagger](http://54.207.110.120/swagger)

> Através do Swagger é possível testar todos os endpoints da API diretamente no navegador.

---

## 🚀 Tecnologias Utilizadas

* **Linguagem:** C#
* **Framework:** ASP.NET Core Web API (.NET 9)
* **Banco de Dados:** MySQL 8.x
* **ORM:** Entity Framework Core (Pomelo.EntityFrameworkCore.MySql)
* **Mapeamento de Objetos:** AutoMapper
* **Validação:** FluentValidation
* **Documentação:** Swagger / OpenAPI
* **Containerização:** Docker & Docker Compose
* **Cloud:** AWS EC2 (Linux Ubuntu)

---

## 🏗️ Arquitetura do Projeto

O projeto segue uma arquitetura em camadas, garantindo organização, manutenibilidade e separação de responsabilidades:

* **Controllers:** Camada de entrada da API (endpoints HTTP).
* **DTOs:** Objetos de transferência de dados (entrada e saída da API).
* **Services:** Regras de negócio e validações de fluxo.
* **Repositories:** Acesso e manipulação dos dados no banco.
* **Entities / Models:** Representação das tabelas do banco de dados.
* **Validators:** Validações com FluentValidation.
* **Mapping:** Configurações do AutoMapper.

---

## ⚙️ Pré-requisitos (Ambiente Local)

Para executar o projeto localmente, é necessário ter instalado:

* [.NET SDK 9.0](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [Git](https://git-scm.com/)

---

## 🛠️ Executando o Projeto Localmente

### 1️⃣ Clonar o Repositório

```bash
git clone https://github.com/WeslleiBrito/ApiEmpresa.git
cd ApiEmpresa
```

---

### 2️⃣ Subir o Banco de Dados com Docker

O banco de dados MySQL é executado via Docker Compose.

```bash
docker compose up -d
```

**Credenciais do Banco:**

* **Host:** localhost
* **Porta:** 3307
* **Database:** api_empresas
* **Usuário:** apiuser
* **Senha:** apiuser123

---

### 3️⃣ Criar as Tabelas (Migrations)

Com o banco ativo, execute as migrations para criar as tabelas:

```bash
dotnet ef database update
```

---

### 4️⃣ Executar a API

```bash
dotnet run
```

A aplicação será iniciada e ficará disponível em:

```
http://localhost:5235/swagger
```

---

## 📖 Documentação da API

A documentação interativa é disponibilizada via **Swagger**.

### Ambiente Local

```
http://localhost:5235/swagger
```

### Ambiente de Produção (AWS)

```
http://54.207.110.120/swagger
```

---

## 📌 Principais Funcionalidades

### 🏢 Empresas (`/api/empresa`)

* Cadastro, listagem, atualização e remoção
* Associação de funcionários
* Associação de setores

### 👨‍💼 Funcionários (`/api/funcionario`)

* CRUD completo
* Associação de habilidades

### 🏬 Setores (`/api/setor`)

* Gerenciamento de setores/departamentos

### 🧠 Habilidades (`/api/habilidade`)

* Cadastro e gerenciamento de competências

---

## ☁️ Deploy (Resumo Técnico)

* API e MySQL executados em containers Docker
* Docker Compose para orquestração
* Porta **80** exposta para acesso público
* Banco de dados não exposto externamente
* Migrations aplicadas automaticamente na inicialização

---

## 📚 Observações Finais

Este projeto foi desenvolvido com foco educacional, aplicando conceitos modernos de desenvolvimento backend, containerização e deploy em nuvem.

Sugestões e melhorias são bem-vindas!
