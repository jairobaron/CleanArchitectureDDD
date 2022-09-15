# CleanArchitecture
This is a solution template for creating API in ASP.NET Core following the principles of Clean Architecture. Create a new project based on this template by clicking the above Use this template button or by installing and running the associated NuGet package (see Getting Started for full details). This is based on jason taylor template: jasontaylordev/CleanArchitecture of gibhub.com

## Getting Started

### Database Configuration

The template is configured to use an in-memory database by default. This ensures that all users will be able to run the solution without needing to set up additional infrastructure (e.g. SQL Server).

If you would like to use SQL Server, you will need to update **CleanArchitectureDDD.API/appsettings.json** as follows:

```json  
"UseInMemoryDatabase": false,
```

Verify that the **Default** connection string within **appsettings.json** points to a valid SQL Server instance.

When you run the application the database will be automatically created (if necessary) and the latest migrations will be applied.

### Database Migrations
To use ```dotnet-ef``` for your migrations please add the following flags to your command (values assume you are executing from repository root)

* ```--project src/CleanArchitectureDDD.Infrastructure (optional if in this folder)```
* ```--startup-project src/CleanArchitectureDDD.API```
* ```--output-dir Persistence/Migrations```

For example, to add a new migration from the root folder:

```dotnet ef migrations add "SampleMigration" --project src\CleanArchitectureDDD.Infrastructure --startup-project src\CleanArchitectureDDD.API --output-dir Persistence\Migrations```

## Overview

### Domain

This will contain all entities, enums, exceptions, interfaces, types and logic specific to the domain layer.

### Application
This layer contains all application logic. It is dependent on the domain layer, but has no dependencies on any other layer or project. This layer defines interfaces that are implemented by outside layers. For example, if the application need to access a notification service, a new interface would be added to application and an implementation would be created within infrastructure.

### Infrastructure
This layer contains classes for accessing external resources such as file systems, web services, smtp, and so on. These classes should be based on interfaces defined within the application layer.

### API
This layer contains controllers for frontend. This layer depends on both the Application and Infrastructure layers, however, the dependency on Infrastructure is only to support dependency injection. Therefore only Startup.cs should reference Infrastructure.

## Support
If you are having problems, please let us know by [raising a new issue.](https://github.com/jairobaron/CleanArchitectureDDD/issues/new/choose)

## License
This project is licensed with the MIT license.
