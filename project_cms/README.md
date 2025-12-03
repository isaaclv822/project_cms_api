# project_cms

Description
- API .NET 8 pour la gestion d'articles (CRUD) avec Identity et JWT.
- Base de données PostgreSQL via Entity Framework Core (Npgsql).
- Swagger activé en environnement de développement avec support Bearer (JWT).

Prérequis
- .NET 8 SDK
- PostgreSQL
- (Optionnel) __dotnet-ef__ global tool : `dotnet tool install --global dotnet-ef`
- Packages NuGet : `Microsoft.EntityFrameworkCore`, `Npgsql.EntityFrameworkCore.PostgreSQL`, `Microsoft.EntityFrameworkCore.Tools`, `Microsoft.AspNetCore.Authentication.JwtBearer`, `Swashbuckle.AspNetCore`, `Microsoft.AspNetCore.Identity.EntityFrameworkCore`

Configuration (appsettings.json)
- Le fichier `appsettings.json` n'est pas versionné — vous devez le créer localement.
- Clés requises :
  - `ConnectionStrings: bdd` — chaîne de connexion PostgreSQL utilisée par `AppDbContext`.
  - `Jwt: Key`, `Jwt: Issuer`, `Jwt: Audience` — paramètres pour la génération/validation des tokens JWT.
  - `Logging` (optionnel)

Exemple de `appsettings.json` (à adapter) :
```
{
  "ConnectionStrings": {
    "bdd": "Host=localhost;Port=5432;Database=project_cms_db;Username=postgres;Password=your
    _password"
    },
    "Jwt": {
    "Key": "votre_cle_secrete_longue_pour_jwt",
      "Issuer": "votre_issuer",
      "Audience": "votre_audience"
    },
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    }
    }
```

Swagger — documentation et utilisation
- Swagger est configuré dans `Program.cs` et exposera l'UI en mode Development.
- Le schéma de sécurité Bearer est déclaré ; pour appeler les endpoints protégés depuis Swagger :
  1. Ouvrir `https://{host}:{port}/swagger` (ou l'URL affichée à l'exécution).
  2. Cliquer sur le bouton "Authorize".
  3. Entrer le token sous la forme : `Bearer {votre_token_JWT}` (ex. `Bearer eyJ...`).
- En production, si vous souhaitez activer Swagger hors-dev, déplacer `app.UseSwagger()` / `app.UseSwaggerUI()` hors du check `if (app.Environment.IsDevelopment())` en prenant en compte les risques d'exposition.

Authentification
- Identity est configuré avec `IdentityUser` et `IdentityRole`.
- JWT : la validation utilise `Jwt:Key`, `Jwt:Issuer` et `Jwt:Audience` depuis `appsettings.json`.
- Assurez-vous que la clé (`Jwt:Key`) est suffisamment longue et stockée de façon sécurisée (secrets, variables d'environnement ou Azure Key Vault en production).

Migrations et mise à jour de la base de données (EF Core)
- Depuis le dossier du projet (ex. `cd project_cms`) :
  - Ajouter une migration :
    - CLI .NET : `dotnet ef migrations add InitialCreate`
  - Appliquer les migrations (mise à jour de la base) :
    - CLI .NET : `dotnet ef database update`
- Depuis Visual Studio (__Package Manager Console__) :
  - S'assurer que le __Default project__ est le projet `project_cms`.
  - Exécuter : `__Add-Migration__ InitialCreate`
  - Puis : `__Update-Database__`
- Si vous avez une solution multi-projets ou que vous exécutez depuis la racine, utilisez les options `-p` (projet) et `-s` (startup) :
  - `dotnet ef migrations add InitialCreate -p project_cms -s project_cms`
  - `dotnet ef database update -p project_cms -s project_cms`

Conseils rapides
- Pour le développement local, `Jwt:Key` peut être dans `appsettings.Development.json` ou stocké via `dotnet user-secrets`.
- Vérifier la chaîne de connexion `ConnectionStrings: bdd` si les migrations échouent (authentification PostgreSQL, ports, valeurs).
- Si `dotnet ef` n'est pas reconnu, installez l'outil : `dotnet tool install --global dotnet-ef` et ajoutez les packages EF Tools au projet.

Support
- Fichier d'entrée : `Program.cs`
- Endpoints articles : contrôleur `Controllers/ArticleController.cs` (routes sous `/article`)
