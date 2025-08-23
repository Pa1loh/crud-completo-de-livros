# API de Livros

API REST para gerenciamento de livros, autores e gêneros desenvolvida em .NET 9 seguindo princípios SOLID e Clean Architecture.

## 🚀 Tecnologias Utilizadas

- **.NET 9** - Framework principal
- **Entity Framework Core** - ORM para acesso a dados
- **PostgreSQL** - Banco de dados
- **Swagger/OpenAPI** - Documentação da API
- **NUnit** - Framework de testes
- **Entity Framework In-Memory** - Testes unitários
- **Docker & Docker Compose** - Containerização

## 📁 Arquitetura

O projeto segue os princípios da **Clean Architecture** com separação clara de responsabilidades:

```
📦 ApiLivros/
├── ApiLivros/          # Camada de Apresentação (Controllers, ViewModels, Middleware)
├── Dominio/            # Camada de Domínio (Entidades, Regras de Negócio)
├── Servico/            # Camada de Aplicação (Casos de Uso, DTOs, Interfaces)
├── Infraestrutura/     # Camada de Infraestrutura (Repositórios, Contexto, Migrations)
└──  Teste/              # Testes Unitários
```

### ✅ **Requisitos Técnicos**
- **Responsabilidade Única**: Cada classe tem uma única responsabilidade
- **Injeção de Dependência**: Configurada via construtor
- **Versionamento da API**: Rotas versionadas (`api/v1/`)
- **Documentação Swagger**: Configurada com informações completas
- **HTTP Status Codes**: Respostas padronizadas
- **Environments**: Configurações para Development/Production
- **DTOs**: Separação clara entre camadas
- **ViewModels**: Apresentação desacoplada dos DTOs
- **Entidades**: Domain-driven design
- **ORM**: Entity Framework Core
- **Migrations**: Controle de versão do banco

## Como Executar

### Pré-requisitos
- .NET 9 SDK
- Docker e Docker Compose
- PostgreSQL (ou usar o container)

### **Clonar o repositório**
```bash
git clone <url-do-repositorio>
cd ApiLivros
```

### **Executar com Docker Compose**
```bash
# Subir a aplicação e banco de dados
docker-compose up -d

# A API estará disponível em: http://localhost:8080
# Swagger UI: http://localhost:8080/swagger
```

### **Executar localmente**
```bash
# Restaurar dependências
dotnet restore

# Executar migrations
dotnet ef database update --project Infraestrutura

# Executar a aplicação
dotnet run --project ApiLivros
```

## Endpoints da API

### **Livros** (`/api/v1/livros`)
```http
GET    /api/v1/livros              # Listar todos os livros
GET    /api/v1/livros/{id}         # Obter livro por ID
GET    /api/v1/livros/autor/{id}   # Listar livros por autor
GET    /api/v1/livros/genero/{id}  # Listar livros por gênero
POST   /api/v1/livros              # Criar novo livro
PUT    /api/v1/livros/{id}         # Atualizar livro
DELETE /api/v1/livros/{id}         # Excluir livro
```

### **Autores** (`/api/v1/autores`)
```http
GET    /api/v1/autores             # Listar todos os autores
GET    /api/v1/autores/{id}        # Obter autor por ID
POST   /api/v1/autores             # Criar novo autor
PUT    /api/v1/autores/{id}        # Atualizar autor
DELETE /api/v1/autores/{id}        # Excluir autor
```

### **Gêneros** (`/api/v1/generos`)
```http
GET    /api/v1/generos             # Listar todos os gêneros
GET    /api/v1/generos/{id}        # Obter gênero por ID
POST   /api/v1/generos             # Criar novo gênero
PUT    /api/v1/generos/{id}        # Atualizar gênero
DELETE /api/v1/generos/{id}        # Excluir gênero
```

## 🐳 Docker

### Dockerfile
A aplicação possui Dockerfile otimizado com multi-stage build.

### Docker Compose
Configuração completa com API e banco PostgreSQL:

```bash
# Subir os serviços
docker-compose up -d

# Ver logs
docker-compose logs -f

# Parar os serviços
docker-compose down
```

## 🔧 Configurações

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=ApiLivros;Username=postgres;Password=postgres"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```
