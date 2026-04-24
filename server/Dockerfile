FROM mcr.microsoft.com/dotnet/sdk:10.0 AS restore
WORKDIR /src

COPY ["server.sln", "./"]
COPY ["applications/aggregator/API/API.csproj", "applications/aggregator/API/"]
COPY ["applications/aggregator/SharedContracts/SharedContracts.csproj", "applications/aggregator/SharedContracts/"]
COPY ["applications/aggregator/SharedKernel/SharedKernel.csproj", "applications/aggregator/SharedKernel/"]
COPY ["modules/application/Applications/Applications.csproj", "modules/application/Applications/"]
COPY ["modules/application/Applications.Contracts/Applications.Contracts.csproj", "modules/application/Applications.Contracts/"]
COPY ["modules/lease/Leases/Leases.csproj", "modules/lease/Leases/"]
COPY ["modules/lease/Leases.Contracts/Leases.Contracts.csproj", "modules/lease/Leases.Contracts/"]
COPY ["modules/manager/Managers/Managers.csproj", "modules/manager/Managers/"]
COPY ["modules/manager/Managers.Contracts/Managers.Contracts.csproj", "modules/manager/Managers.Contracts/"]
COPY ["modules/payment/Payments/Payments.csproj", "modules/payment/Payments/"]
COPY ["modules/payment/Payments.Contracts/Payments.Contracts.csproj", "modules/payment/Payments.Contracts/"]
COPY ["modules/property/Properties/Properties.csproj", "modules/property/Properties/"]
COPY ["modules/property/Properties.Contracts/Properties.Contracts.csproj", "modules/property/Properties.Contracts/"]
COPY ["modules/tenant/Tenants/Tenants.csproj", "modules/tenant/Tenants/"]
COPY ["modules/tenant/Tenants.Contracts/Tenants.Contracts.csproj", "modules/tenant/Tenants.Contracts/"]

RUN dotnet restore "applications/aggregator/API/API.csproj"

FROM restore AS publish
COPY . .
RUN dotnet publish "applications/aggregator/API/API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_EnableDiagnostics=0 \
    KestrelRuntime__BindAddress=0.0.0.0

EXPOSE 5001
EXPOSE 5168

COPY --from=publish /app/publish .

USER $APP_UID

ENTRYPOINT ["dotnet", "API.dll"]
