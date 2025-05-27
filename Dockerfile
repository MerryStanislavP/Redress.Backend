# --- Build stage ---
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем csproj и восстанавливаем зависимости
COPY Redress.Backend.API/*.csproj ./Redress.Backend.API/
COPY Redress.Backend.Application/*.csproj ./Redress.Backend.Application/
COPY Redress.Backend.Contracts/*.csproj ./Redress.Backend.Contracts/
COPY Redress.Backend.Infrastructure/*.csproj ./Redress.Backend.Infrastructure/
RUN dotnet restore ./Redress.Backend.API/Redress.Backend.API.csproj

# Копируем остальные файлы и публикуем приложение
COPY . .
RUN dotnet publish ./Redress.Backend.API/Redress.Backend.API.csproj -c Release -o /app/publish

# --- Runtime stage ---
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render рекомендует использовать порт 8080
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Redress.Backend.API.dll"] 