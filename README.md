# C#-Assessment
E-Commerce API

A simple e-commerce backend built with ASP.NET Core, Entity Framework Core, and PostgreSQL.
This project supports product management and order placement with stock validation.

**SETUP INSTRUCTIONS**

Clone the repository

git clone https://github.com/your-username/ecommerce-api.git
cd ecommerce


Install dependencies

dotnet restore


Configure environment

Add your connection string in appsettings.json or as an environment variable:

"ConnectionStrings": {
  "DefaultConnection": "Host=<your-render-host>;Database=<your-db>;Username=<your-username>;Password=<your-password>;SSL Mode=Require;Trust Server Certificate=true"
}


(The database is already migrated and seeded, so no need to run migrations.)

Run the project

dotnet run


Test endpoints

Swagger UI will be available at:

http://localhost:5143/swagger

**ASSUMPTIONS**

Each product has a unique name.

Orders cannot be placed with quantity ≤ 0.

Orders fail if there is insufficient stock.

Stock is automatically reduced when an order is successfully placed.

Only basic authentication/authorization is assumed (not implemented for this assessment).

Monetary values are stored as decimal to avoid floating-point errors.

🛠️ Tech Stack Choices

.NET 8 (ASP.NET Core Web API)  for building scalable backend APIs.

Entity Framework Core  for ORM and database interactions.

PostgreSQL (Render-hosted)  relational database for persistence.

Swagger / Swashbuckle  for API documentation and testing.

Dependency Injection  for clean architecture and testability.
