API de Livros 
📋 Índice
Pré-requisitos

Como Executar a Aplicação

Opção 1: Com Docker (Recomendado)

Opção 2: Configuração Manual

Segurança: Variáveis de Ambiente e User Secrets

Endpoints da API (Swagger)

Arquitetura e Tecnologias

Testes

Como Contribuir

🔧 Pré-requisitos
Antes de começar, garanta que você tenha as ferramentas necessárias para a abordagem escolhida.

Para Execução com Docker
Docker e Docker Compose

Para Execução Manual
.NET 9 SDK

Node.js v18+ e npm

PostgreSQL (ou Docker para rodar o contêiner do banco)

🚀 Como Executar a Aplicação
Opção 1: Execução Rápida com Docker (Recomendado)
Esta é a forma mais simples e rápida de ter todo o ambiente rodando.

1. Clone o repositório
   Bash

git clone https://github.com/seu-usuario/seu-repositorio.git
cd seu-repositorio
2. Crie o arquivo de ambiente (Opcional, mas recomendado)
   Por segurança, as credenciais do banco não ficam no código. Crie um arquivo chamado .env na raiz do projeto.

<details>
<summary><strong>🔒 Clique para ver o aviso sobre o arquivo .env</strong></summary>

Importante: O arquivo .env armazena dados sensíveis, como senhas de banco de dados. Ele nunca deve ser enviado para o repositório do Git. Certifique-se de que o nome .env está no seu arquivo .gitignore. Usar este arquivo evita expor suas credenciais diretamente no docker-compose.yaml.

</details>

Copie o conteúdo abaixo para o seu arquivo .env:

Bash

# Arquivo .env
DB_HOST=postgres
DB_PORT=5432
DB_NAME=livros_dev
DB_USER=postgres
DB_PASSWORD=postgres # Troque por uma senha forte se desejar
API_PORT=7239
FRONTEND_PORT=4200
3. Execute com Docker Compose
   Bash

docker-compose up -d --build
Pronto! A aplicação estará disponível nos seguintes endereços:

🌐 Frontend (Angular): http://localhost:4200

⚙️ Backend API (.NET): http://localhost:7239

📖 Documentação Swagger: http://localhost:7239/swagger