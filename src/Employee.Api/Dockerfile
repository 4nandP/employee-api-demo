FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Employee.Api/Employee.Api.csproj", "Employee.Api/"]
RUN dotnet restore "Employee.Api/Employee.Api.csproj"
COPY . .
WORKDIR "/src/Employee.Api"
RUN dotnet build "Employee.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Employee.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Employee.Api.dll"]