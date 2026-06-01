FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos os arquivos de projeto primeiro para restaurar dependências (otimização de cache)
COPY ["BolaoCopaApp.API/BolaoCopaApp.API.csproj", "BolaoCopaApp.API/"]
COPY ["BolaoCopaApp.Application/BolaoCopaApp.Application.csproj", "BolaoCopaApp.Application/"]
COPY ["BolaoCopaApp.Domain/BolaoCopaApp.Domain.csproj", "BolaoCopaApp.Domain/"]
COPY ["BolaoCopaApp.Infrastructure/BolaoCopaApp.Infrastructure.csproj", "BolaoCopaApp.Infrastructure/"]

RUN dotnet restore "BolaoCopaApp.API/BolaoCopaApp.API.csproj"

# Copia o restante dos arquivos
COPY . .

RUN dotnet build "BolaoCopaApp.API/BolaoCopaApp.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BolaoCopaApp.API/BolaoCopaApp.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BolaoCopaApp.API.dll"]
