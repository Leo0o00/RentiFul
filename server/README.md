### Run Migrations for Managers Module

- From the project root directory:

    1- Move to Manager project folder:
    ```
    cd .\modules\
    cd .\manager\
    cd .\Managers\
    ```
  
    2- Execute the migration command:
    ```
    dotnet ef migrations add AddedManagerSchema -c ManagerDbContext -p ..\Managers\Managers.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
    ```
    3- Execute the update database command: 
    ```
    dotnet ef database update -c ManagerDbContext -p ..\Managers\Managers.csproj -s ..\..\..\applications\aggregator\API\API.csproj
    ```

### Run Migrations for Properties Module

- From the project root directory:

    1- Move to Property project folder:
    ```
    cd .\modules\
    cd .\property\
    cd .\Properties\
    ```
  
    2- Execute the migration command:
    ```
    dotnet ef migrations add AddedPropertySchema -c PropertyDbContext -p ..\Properties\Properties.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
    ```
    3- Execute the update database command: 
    ```
    dotnet ef database update -c PropertyDbContext -p ..\Properties\Properties.csproj -s ..\..\..\applications\aggregator\API\API.csproj
    ```

### Run Migrations for Leases Module

- From the project root directory:

  1- Move to Property project folder:
    ```
    cd .\modules\
    cd .\lease\
    cd .\Leases\
    ```

  2- Execute the migration command:
    ```
    dotnet ef migrations add AddedLeaseSchema -c LeaseDbContext -p ..\Leases\Leases.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
    ```
  3- Execute the update database command:
    ```
    dotnet ef database update -c LeaseDbContext -p ..\Leases\Leases.csproj -s ..\..\..\applications\aggregator\API\API.csproj
    ```
  
### Run Migrations for Payments Module

- From the project root directory:

  1- Move to Property project folder:
    ```
    cd .\modules\
    cd .\payment\
    cd .\Payments\
    ```

  2- Execute the migration command:
    ```
    dotnet ef migrations add AddedPaymentSchema -c PaymentDbContext -p ..\Payments\Payments.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
    ```
  3- Execute the update database command:
    ```
    dotnet ef database update -c PaymentDbContext -p ..\Payments\Payments.csproj -s ..\..\..\applications\aggregator\API\API.csproj
    ```

### Run Migrations for Applications Module

- From the project root directory:

  1- Move to Property project folder:
    ```
    cd .\modules\
    cd .\application\
    cd .\Applications\
    ```

  2- Execute the migration command:
    ```
    dotnet ef migrations add AddedApplicationSchema -c ApplicationDbContext -p ..\Applications\Applications.csproj -s ..\..\..\applications\aggregator\API\API.csproj -o Data/Migrations
    ```
  3- Execute the update database command:
    ```
    dotnet ef database update -c ApplicationDbContext -p ..\Applications\Applications.csproj -s ..\..\..\applications\aggregator\API\API.csproj
    ```