FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["LoanManagementSystem.Web/LoanManagementSystem.Web.csproj", "LoanManagementSystem.Web/"]
RUN dotnet restore "LoanManagementSystem.Web/LoanManagementSystem.Web.csproj"

COPY . .

WORKDIR "/src/LoanManagementSystem.Web"
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "LoanManagementSystem.Web.dll"]
