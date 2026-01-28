FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["LoanManagementSystem.Api/LoanManagementSystem.Api.csproj", "LoanManagementSystem.Api/"]
COPY ["LoanManagementSystem.Application/LoanManagementSystem.Application.csproj", "LoanManagementSystem.Application/"]
COPY ["LoanManagementSystem.Domain/LoanManagementSystem.Domain.csproj", "LoanManagementSystem.Domain/"]
COPY ["LoanManagementSystem.Infrastructure/LoanManagementSystem.Infrastructure.csproj", "LoanManagementSystem.Infrastructure/"]

RUN dotnet restore "LoanManagementSystem.Api/LoanManagementSystem.Api.csproj"

COPY . .

WORKDIR "/src/LoanManagementSystem.Api"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "LoanManagementSystem.Api.dll"]
