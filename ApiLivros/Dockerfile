FROM mcr.microsoft.com/dotnet/sdk:9.0
WORKDIR /app

RUN apt-get update && apt-get install -y netcat-openbsd && rm -rf /var/lib/apt/lists/*

COPY ApiLivros.sln ./
COPY ApiLivros/*.csproj ./ApiLivros/
COPY Dominio/*.csproj ./Dominio/
COPY Infraestrutura/*.csproj ./Infraestrutura/
COPY Servico/*.csproj ./Servico/
COPY Teste/*.csproj ./Teste/

RUN dotnet restore
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY . .
RUN chmod +x ApiLivros/wait-for-postgres.sh

WORKDIR /app/ApiLivros
EXPOSE 8080

CMD ["./wait-for-postgres.sh"]
