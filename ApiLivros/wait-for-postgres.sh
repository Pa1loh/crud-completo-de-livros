#!/bin/sh
set -e

echo "Aguardando PostgreSQL..."
while ! nc -z database 5432; do
  sleep 2
done
echo "PostgreSQL disponível!"

echo "Executando build..."
dotnet build --configuration Release

echo "Executando migrations..."
dotnet ef database update

echo "Iniciando aplicação..."
dotnet run --configuration Release --urls http://0.0.0.0:8080
