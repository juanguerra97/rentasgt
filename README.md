# Rentas Gt

## Migrations

InitialCreate migration command:
```
dotnet ef migrations add InitialCreate -v --project src/Infrastructure --startup-project src/WebUI --output-dir Persistence/Migrations
```
> This was executed from the project root directory


Apply migrations command:
```
dotnet ef databse update --project src/Infrastructure --startup-project src/WebUI
```
> This was executed from the project root directory
