# Copilot Project Instructions

## Contexte
API REST en C# (.NET 8) pour un CMS minimal. 
Fonctionnalités : CRUD articles, authentification JWT, Swagger, tests automatisés. 
Architecture : Controllers, Data, DTOs, Interfaces, Models, Repositories, Services.

Langue : Français
Commentaires de code en français.

## Conventions
- Utiliser async/await partout.
- IDs de type Guid.
- EF Core pour la persistance.
- DTO séparés des entités.
- Dependency Injection obligatoire.

## Architecture du projet
- Controllers : gérer les requêtes HTTP.
- Data : DbContext et configurations EF Core.
- DTOs : objets de transfert de données.
- Interfaces : définir les contrats pour services et repositories.
- Models : entités de la base de données.
- Repositories : implémentations des accès aux données.
- Services : mappers de DTOs.

## Patterns services et repository
Méthodes à générer :
- CreateAsync()
- GetAllAsync()
- GetByIdAsync()
- UpdateAsync()
- DeleteAsync()

## Exceptions
Gérer les erreurs via exceptions personnalisées :
- NotFoundException
- ValidationException
Middleware global pour réponse JSON.

## Authentification
- JWT
- Endpoint POST /api/auth/login
- GET public, POST/PUT/DELETE protégés

## Swagger
- Activer AddSwaggerGen()
- Documenter tous les endpoints

## Tests
- xUnit
- Tests pour services sans base de données
- Tests intégration controllers
