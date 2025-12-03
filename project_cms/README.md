# project_cms

## 🧾 Description

API **.NET 8** pour la gestion d’articles avec :

* CRUD complet
* Authentification **Identity** + **JWT**
* Base de données **PostgreSQL** via Entity Framework Core (Npgsql)
* **Swagger** configuré avec authentification Bearer (JWT) en environnement de développement

Ce projet est conçu comme une base solide pour construire un CMS léger ou une API de blog. 
L’objectif est de proposer une architecture claire, facilement extensible, avec séparation des 
responsabilités et un système d’authentification prêt à l’emploi. Que vous souhaitiez ajouter des 
catégories, stocker des images, gérer des utilisateurs ou exposer des endpoints supplémentaires, 
le code est structuré pour évoluer rapidement et proprement. 

---

## 🚀 Prérequis

* **.NET 8 SDK**
* **PostgreSQL**
* (Optionnel) Outil global pour EF Core :

  ```bash
  dotnet tool install --global dotnet-ef
  ```
* Packages principaux :

  * `Microsoft.EntityFrameworkCore`
  * `Npgsql.EntityFrameworkCore.PostgreSQL`
  * `Microsoft.EntityFrameworkCore.Tools`
  * `Microsoft.AspNetCore.Authentication.JwtBearer`
  * `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
  * `Swashbuckle.AspNetCore`

---

## ⚙️ Configuration (`appsettings.json`)

> 📌 Le fichier `appsettings.json` n’est **pas versionné**, vous devez le créer localement.

#### Clés obligatoires

* `ConnectionStrings: bdd` : chaîne de connexion PostgreSQL pour `AppDbContext`
* `Jwt: Key`, `Jwt: Issuer`, `Jwt: Audience` : configuration du JWT
* `Logging` (facultatif)

#### Exemple de configuration

```json
{
  "ConnectionStrings": {
    "bdd": "Host=localhost;Port=5432;Database=project_cms_db;Username=postgres;Password=your_password"
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

> 🔐 En production : stocker `Jwt:Key` dans une variable d’environnement ou un secret manager.

---

## 📄 Swagger

* UI activée **en mode Development**
* Schéma de sécurité **Bearer** configuré

### Utilisation

1. Lancer l’application et ouvrir l’URL Swagger (ex : `https://localhost:5001/swagger`)
2. Cliquer sur **Authorize**
3. Entrer :

   ```
   Bearer eyJ...
   ```

### Activer Swagger en production (⚠️)

Déplacer :

```csharp
app.UseSwagger();
app.UseSwaggerUI();
```

hors du :

```csharp
if (app.Environment.IsDevelopment())
```

---

## 🔑 Authentification

* Utilisation d’`IdentityUser` et `IdentityRole`
* JWT généré et validé via les valeurs `Jwt:*`
* Pensé pour une configuration simple et sécurisée

---

## 🧱 Architecture

Organisation en couches simples :

* `Controllers`
* `Services` / `Mappers`
* `Interfaces`
* `Data`
* `Models`
* `DTOs`
* `Program.cs`

### Arborescence simplifiée

```
project_cms/
├─ Controllers/
│  ├─ AuthController.cs
│  └─ ArticleController.cs
├─ Data/
│  └─ AppDbContext.cs
├─ Interfaces/
│  └─ IArticleRepository.cs
├─ Services/
│  └─ ArticleMapper.cs
├─ Models/
│  └─ Article.cs
├─ DTOs/
│  ├─ ArticleRequestDTO.cs
│  └─ ArticleResponseDTO.cs
├─ Program.cs
└─ appsettings.json (local)
```

---

## 🛠️ Migrations (Entity Framework Core)

### Avec CLI .NET

```bash
# Ajouter une migration
dotnet ef migrations add InitialCreate

# Appliquer la migration
dotnet ef database update
```

### Multi-projets

```bash
dotnet ef migrations add InitialCreate -p project_cms -s project_cms
dotnet ef database update -p project_cms -s project_cms
```

### Visual Studio

Package Manager Console :

```powershell
Add-Migration InitialCreate
Update-Database
```

> ⚠️ Vérifier que le **Default project** est bien `project_cms`.

---

## 📡 Endpoints principaux

### Base routes

* Auth : `/user`
* Articles : `/article`

---

### 🔐 Authentification (public)

#### `POST /user/register`

* Crée un utilisateur Identity

Body (JSON) :

```json
{
  "username": "user@example.com",
  "password": "Password123!"
}
```

#### `POST /user/login`

Réponse :

```json
{
  "token": "eyJ..."
}
```

---

### 📝 Articles (JWT obligatoire)

#### `GET /article`

Récupère tous les articles.

#### `GET /article/{id}`

Récupère un article par id.

#### `POST /article/add`

Body :

```json
{
  "title": "Titre",
  "content": "Contenu"
}
```

#### `PUT /article/update/{id}`

#### `DELETE /article/delete/{id}`

Réponse attendue : `204 NoContent` si suppression OK.

---

## 💡 Conseils rapides

* Utiliser `appsettings.Development.json` pour les secrets en local.
* Installer `dotnet-ef` si la commande EF n'est pas reconnue :

  ```bash
  dotnet tool install --global dotnet-ef
  ```
* Si erreur PostgreSQL : vérifier user, mot de passe, port (par défaut `5432`).

---

## 🆘 Support

* Fichier principal : `Program.cs`
* Endpoints articles : `Controllers/ArticleController.cs`

---
