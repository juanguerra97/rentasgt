# Rentas Gt

## Migrations

InitialCreate migration command:
```
dotnet ef migrations add InitialCreate -v --project src/Infrastructure --startup-project src/WebUI --output-dir Persistence/Migrations
```
> This was executed from the project root directory


Command for generating an sql script to apply migrations
```
dotnet ef migrations script -v --project src/Infrastructure --startup-project src/WebUI --output src/Infrastructure/Persistence/Migrations/MigrationScript.sql
```
> This was executed from the project root directory


Apply migrations command:
```
dotnet ef database update --project src/Infrastructure --startup-project src/WebUI
```
> This was executed from the project root directory
