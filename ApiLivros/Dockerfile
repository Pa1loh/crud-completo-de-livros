FROM mcr.microsoft.com/dotnet/sdk:9.0 AS development

WORKDIR /app

COPY *.sln ./
COPY ApiLivros/*.csproj ApiLivros/
COPY Dominio/*.csproj Dominio/
COPY Servico/*.csproj Servico/
COPY Infraestrutura/*.csproj Infraestrutura/
COPY Teste/*.csproj Teste/

RUN dotnet restore ApiLivros/ApiLivros.csproj

COPY . .

RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

EXPOSE 8080

WORKDIR /app/ApiLivros

CMD ["dotnet", "watch", "run", "--urls", "http://0.0.0.0:8080"]