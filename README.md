# ApiEmpresa

## 📋 Sobre o Projeto

Este é um projeto de conclusão de matéria desenvolvido em **.NET 9**. O objetivo da API é fornecer um sistema para gerenciamento de empresas, incluindo o controle de funcionários, setores, habilidades e endereços. O sistema implementa relacionamentos complexos entre essas entidades e validações de regras de negócio.

## 🚀 Tecnologias Utilizadas

* **Linguagem:** C# (.NET 9)
* **Framework:** ASP.NET Core Web API
* **Banco de Dados:** MySQL 9.3
* **ORM:** Entity Framework Core (com Pomelo.EntityFrameworkCore.MySql)
* **Mapeamento:** AutoMapper
* **Validação:** FluentValidation
* **Documentação:** Swagger / OpenAPI
* **Containerização:** Docker & Docker Compose

## ⚙️ Pré-requisitos

Para rodar o projeto localmente, você precisará ter instalado:

* [.NET SDK 9.0](https://dotnet.microsoft.com/download)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)
* [Git](https://git-scm.com/)

---

## 🛠️ Como Executar o Projeto

Siga o passo a passo abaixo para configurar o ambiente e rodar a API.

### 1. Clonar o Repositório

Primeiro, faça o clone do projeto para sua máquina local:

```bash
git clone https://github.com/WeslleiBrito/ApiEmpresa.git
cd ApiEmpresa

```

### 2. Configurar o Banco de Dados (Docker)

O projeto utiliza o Docker Compose para subir uma instância do MySQL configurada. O arquivo `docker-compose.yml` já está configurado para expor o banco na porta **3307**.

Execute o comando na raiz onde está o arquivo `docker-compose.yml`:

```bash
docker-compose up -d

```

Isso iniciará o container `mysql_api_empresas` com as seguintes credenciais (definidas no `docker-compose.yml` e `appsettings.json`):

* **Host:** localhost
* **Porta:** 3307
* **Database:** api_empresas
* **Usuário:** apiuser
* **Senha:** apiuser123

### 3. Aplicar as Migrations (Criar Tabelas)

Com o banco de dados rodando, é necessário criar as tabelas utilizando o Entity Framework. Certifique-se de estar na pasta do projeto (onde está o arquivo `.csproj`):

```bash
# Caso esteja na raiz, entre na pasta do projeto
cd ApiEmpresa

# Execute a atualização do banco
dotnet ef database update

```

### 4. Executar a Aplicação

Agora você pode iniciar a API:

```bash
dotnet run

```

A aplicação será iniciada e mostrará no console as URLs de acesso `http://localhost:5235`.

---

## 📖 Documentação da API (Endpoints)

A maneira mais fácil de explorar e testar os endpoints é através do **Swagger**, que já está configurado no projeto.

1. Com a aplicação rodando, abra o navegador.
2. Acesse: `http://localhost:<5235>/swagger`.

### Principais Recursos

* **Empresas** (`/api/empresa`):
* Cadastrar, listar, atualizar e remover empresas.
* Adicionar/Remover funcionários a uma empresa.
* Adicionar/Remover setores de uma empresa.


* **Funcionários** (`/api/funcionario`):
* Gerenciamento de dados de funcionários.
* Associação de habilidades aos funcionários.


* **Setores** (`/api/setor`):
* Gerenciamento dos departamentos/setores.


* **Habilidades** (`/api/habilidade`):
* Cadastro de competências (ex: C#, Java, Vendas).



---

## 📂 Estrutura do Projeto

O projeto segue uma arquitetura em camadas para separar responsabilidades:

* **Controllers:** Pontos de entrada da API (recebem as requisições HTTP).
* **DTOs (Data Transfer Objects):** Objetos usados para trafegar dados entre o cliente e o servidor, garantindo que as entidades de domínio não sejam expostas diretamente.
* **Services:** Contém as regras de negócio.
* **Repositories:** Responsável pelo acesso direto ao banco de dados.
* **Models/Entities:** Representam as tabelas do banco de dados.
* **Validators:** Regras de validação (FluentValidation) para garantir a integridade dos dados recebidos (ex: validação de CPF/CNPJ).
* **Mapping:** Configurações do AutoMapper para converter Models em DTOs e vice-versa.