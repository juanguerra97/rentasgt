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


## APIs
Enable the use of the secret manager in the project executing the next command from the src/WebUI folder
```
dotnet user-secrets init
```

```
dotnet user-secrets set "GoogleMapsApiKey" "value"
```

```
dotnet user-secrets set "GoogleOAuthClientId" "value"
```

```
dotnet user-secrets set "GoogleOAuthClientSecret" "value"
```

```
dotnet user-secrets set "SendGridApiKey" "value"
```

```
dotnet user-secrets set "TwilioAccountSID" "value"
```

```
dotnet user-secrets set "TwilioAuthToken" "value"
```

```
dotnet user-secrets set "TwilioVerificationServiceSID" "value"
```

## Azure Key Vault
It is necessary to set the values for the ClientId, ClientSecret and the Vault URI of the Azure Key Vault service in the file Program.cs inside the src/WebUI folder.

<img alt="Azure Key Vault Configuration" src="https://github.com/juanguerra97/rentasgt/raw/master/docs/img/KeyVaultProgram.cs.PNG" width="47%">

## Time Zone
Dependending on which platform you are running the project, you will need to modify the variable CENTRAL_AMERICA_STANDARD_TIME in the file RentasGtDateTimeExtensions.cs located inside the folder src/Application/Common/Extensions.

<img alt="Azure Key Vault Configuration" src="https://github.com/juanguerra97/rentasgt/raw/master/docs/img/TimeZone.PNG" width="94%">
