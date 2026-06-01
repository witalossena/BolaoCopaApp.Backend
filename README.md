# BolãoCopaApp - Backend

Este é o backend completo em .NET 8 utilizando a arquitetura Domain-Driven Design (DDD) para o sistema de bolão da Copa do Mundo 2026.

## Estrutura do Projeto

A Solução está dividida nas seguintes camadas (projetos):
* **BolaoCopaApp.Domain**: Contém as Entidades de Domínio, Value Objects, Enums e as Interfaces dos Repositórios e UnitOfWork. 
* **BolaoCopaApp.Application**: Contém os casos de uso (Commands/Queries usando CQRS com MediatR), DTOs, e serviços da aplicação.
* **BolaoCopaApp.Infrastructure**: Contém a persistência com Entity Framework Core (PostgreSQL), Repositórios concretos e os Seeders.
* **BolaoCopaApp.API**: Ponto de entrada (Controllers, configuração do Swagger, JWT, middlewares).

## Tecnologias e Pacotes Utilizados
- **.NET 8**
- **Entity Framework Core** + Npgsql (PostgreSQL)
- **MediatR** (CQRS)
- **BCrypt.Net-Next** (Hashing de senhas)
- **JWT (JwtBearer)** (Autenticação)
- **Swashbuckle / Swagger** (Documentação da API)

## Instruções de Setup

1. **Pré-requisitos**
   - .NET 8 SDK instalado.
   - Banco de dados PostgreSQL rodando (pode ser via Docker ou local).
   - O banco de dados alvo configurado no arquivo `appsettings.json` na raiz da API ou no `BolaoDbContextFactory.cs`. O default é `Host=localhost;Database=bolao_copa;Username=postgres;Password=postgres`.

2. **Aplicar Migrations ao Banco de Dados**
   Navegue até a raiz do projeto e atualize o banco (isso criará as tabelas se não existirem e rodará o Seeder ao iniciar a API):
   ```bash
   dotnet ef database update --project BolaoCopaApp.Infrastructure --startup-project BolaoCopaApp.Infrastructure
   ```

3. **Executar a API**
   Na pasta da Solução ou dentro do projeto API:
   ```bash
   dotnet run --project BolaoCopaApp.API
   ```

4. **Acessar a Documentação e Testar**
   Acesse a URL gerada (por exemplo, `https://localhost:5001/swagger`) no navegador para visualizar todos os endpoints interativos gerados via Swagger.

## Testando a Aplicação
O sistema cria automaticamente as partidas iniciais (`Seed`) ao rodar pela primeira vez. 
O fluxo base é:
1. Cadastrar usuário no `/api/Auth/register`.
2. Fazer Login no `/api/Auth/login` e capturar o Token.
3. Usar o token (`Bearer <token>`) para fazer palpites em `/api/Predictions/match`.
