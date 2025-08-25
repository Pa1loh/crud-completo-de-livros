# API de Livros

## Índice

- [Requisitos para Rodar o Projeto](#requisitos-para-rodar-o-projeto)
- [Configurações de Banco e Connection String](#configurações-de-banco-e-connection-string)
- [Rodando com Docker Compose](#rodando-com-docker-compose)
- [Rodando sem Docker Compose](#rodando-sem-docker-compose)
- [Backend](#backend)
  - [Camada API](#camada-api)
  - [Camada Dominio](#camada-dominio)
  - [Camada Servico](#camada-servico)
  - [Camada Infraestrutura](#camada-infraestrutura)
  - [Camada Teste](#camada-teste)
- [Explicação de Algumas Escolhas](#explicação-de-algumas-escolhas)
- [Frontend](#frontend)

## Requisitos para Rodar o Projeto

### Com Docker
- Docker
- Docker Compose

### Sem Docker
- .NET 9 SDK
- Node.js 18+
- PostgreSQL 15+

## Configurações de Banco e Connection String

### Docker Compose
As configurações de banco estão centralizadas no arquivo `compose.yaml`:
- Host: database (nome do serviço)
- Port: 5432
- Database: livros_db
- Username: postgres
- Password: postgres123

A connection string é configurada automaticamente via variável de ambiente:
```yaml
ConnectionStrings__DefaultConnection: Host=database;Port=5432;Database=livros_db;Username=postgres;Password=postgres123
```

### Configuração Local
Para rodar sem Docker, configure a connection string no `appsettings.Development.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=livros_db;Username=postgres;Password=sua_senha"
  }
}
```

## Rodando com Docker Compose

1. Clone o repositório
```bash
git clone [url-do-repo]
cd ApiLivros
```

2. Execute o comando
```bash
docker-compose up -d --build
```

3. Acesse as aplicações:
- Frontend: http://localhost:4200
- Backend API: http://localhost:5000
- Swagger: http://localhost:5000/swagger

## Rodando sem Docker Compose

### 1. Configurar PostgreSQL
Instale e configure o PostgreSQL com:
- Database: livros_db
- Username: postgres
- Password: sua escolha

### 2. Backend
```bash
cd ApiLivros
dotnet restore
dotnet ef database update
dotnet run
```

### 3. Frontend
```bash
cd frontend
npm install
npm start
```

- Backend: http://localhost:5000
- Frontend: http://localhost:4200

## Backend

### Camada API

Responsável pela exposição dos endpoints REST. Contém:
- **Controllers**: AutoresController, GenerosController, LivrosController
- **Middleware**: TratamentoGlobalErros para captura de exceções
- **Configuracao**: DependenciasDaAplicacao para injeção de dependência
- **Response**: RespostaPadronizada para formatação das respostas
- **Program.cs**: Configuração da aplicação, CORS, Swagger

### Camada Dominio

Contém as regras de negócio e entidades principais:
- **Entidades**: Autor, Genero, Livro com comportamentos e validações
- **Excecoes**: Exceções específicas do domínio (ExcecaoDominio, RecursoDuplicadoException, etc.)
- Implementa encapsulamento com propriedades privadas e métodos para alteração controlada

### Camada Servico

Orquestra as operações entre API e Infraestrutura:
- **Interfaces**: Contratos dos serviços (IGeneroServico, IAutorServico, ILivroServico)
- **Servicos**: Implementação das regras de aplicação
- **Dtos**: Objetos de transferência de dados
- **Constantes**: Valores fixos da aplicação

### Camada Infraestrutura

Gerencia persistência e acesso a dados:
- **Contexto**: LivrosContexto com configuração do Entity Framework
- **Mapeamento**: Configuração das entidades no banco (AutorMapeamento, GeneroMapeamento, LivroMapeamento)
- **Migrations**: Versionamento do schema do banco

### Camada Teste

Testes automatizados da aplicação:
- **Base**: Classes base para testes
- **Servicos**: Testes unitários dos serviços
- Estrutura para garantir qualidade do código

## Explicação de Algumas Escolhas

### Arquitetura Clean Architecture
- Separação clara de responsabilidades em camadas
- Domínio independente de frameworks externos
- Facilita manutenção e testes

### Entity Framework Core com PostgreSQL
- ORM com migrations para versionamento do banco
- PostgreSQL 

### Middleware Global de Erros
- Tratamento centralizado de exceções
- Respostas padronizadas para o frontend
- Logs estruturados

### Sem repository pattner
- Como o projeto e pequeno e para nao ter redundancia com o entity framework, decidi optar por chamar o contexto direto na service

## Frontend

**Localização:** `/frontend`
**Framework:** Angular 17

### Estrutura das Camadas

#### Componentes
- Responsáveis pela apresentação e interação com usuário
- Comunicação com serviços para operações de dados

#### Serviços  
- Fazem requisições HTTP para a API
- Centralizam lógica de comunicação com backend
- Gerenciam estado da aplicação

#### Models/Interfaces
- Definem tipos TypeScript
- Garantem tipagem forte na comunicação com API

#### Roteamento
- Navegação entre páginas
- Configuração de rotas da SPA

A aplicação frontend consome a API REST do backend, oferecendo interface para gerenciamento de livros, autores e gêneros.
