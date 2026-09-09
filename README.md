GameStore

A full-stack game management application built with ASP.NET Core / .NET 10, Entity Framework Core, SQLite, React, TypeScript, and Vite.

The project demonstrates a complete CRUD workflow: users can view games, retrieve a game by ID, add a new game, update an existing game, and delete a game through a React interface backed by an ASP.NET Core Minimal API.

Features

View all games

View a single game by ID

Add new games

Update existing games

Delete games

Genre support and game/genre relationships

DTO-based request and response models

Server-side validation

Entity Framework Core database access

SQLite database

EF Core migrations

React + TypeScript frontend

Vite development environment

RESTful API endpoints

Tech Stack

Backend

.NET 10

ASP.NET Core Minimal APIs

Entity Framework Core 10

SQLite

C#

Data Annotations validation

Frontend

React

TypeScript

Vite

Bootstrap-style UI/components

Project Structure

DotNetProject/
├── GameStore.Api/
│   ├── Data/
│   │   ├── GameStoreContext.cs
│   │   ├── GameStoreContextFactory.cs
│   │   └── Migrations/
│   ├── Dtos (Data Transfer Object)/
│   │   ├── CreateGameDtos.cs
│   │   ├── GameDetailsDto.cs
│   │   ├── GameSummaryDtos.cs
│   │   ├── GenreDto.cs
│   │   └── UpdateGameDtos.cs
│   ├── Endpoints/
│   │   ├── GamesEndpoints.cs
│   │   └── GenresEndpoints.cs
│   ├── Models/
│   ├── Program.cs
│   ├── appsettings.json
│   └── GAMESTORE.Api.csproj
│
└── GameStore.React/
    ├── src/
    │   ├── components/
    │   ├── models/
    │   └── ...
    ├── package.json
    └── vite.config.ts

How the Application Works

1. Application startup

Program.cs creates the ASP.NET Core application, registers validation and the GameStore database services, maps the game and genre endpoints, applies database migrations, and starts the web server.

The high-level flow is:

Program.cs
   ↓
Register services / database
   ↓
MapGamesEndpoints()
MapGenresEndpoints()
   ↓
Migrate database
   ↓
Run API

2. Models

The model classes represent the data stored in the database.

A Game contains values such as:

ID

Name

Genre ID

Price

Release date

A Genre represents the category associated with a game.

Entity Framework Core uses these model classes to map C# objects to SQLite tables.

3. DbContext

GameStoreContext inherits from DbContext and exposes the application's database tables:

public DbSet<Game> Games => Set<Game>();
public DbSet<Genre> Genres => Set<Genre>();

Instead of manually writing SQL for normal CRUD operations, the API works with C# objects and EF Core translates those operations into database queries.

4. DTOs

DTOs (Data Transfer Objects) define the data exchanged between the client and the API.

Different DTOs are used for different operations. For example:

CreateGameDtos — data required to create a game

UpdateGameDtos — data required to update a game

GameDetailsDto — detailed API response

GameSummaryDtos — game summary returned to the frontend

GenreDto — genre information

This separates the public API contract from the internal database entities.

5. API Endpoints

The game endpoints are grouped under:

/games

Typical operations are:

Method

Endpoint

Purpose

GET

/games

Return all games

GET

/games/{id}

Return one game

POST

/games

Create a game

PUT

/games/{id}

Update a game

DELETE

/games/{id}

Delete a game

Genres are exposed through their own endpoint group.

6. Creating a Game

The create flow looks like this:

React form
   ↓
POST /games
   ↓
CreateGameDtos
   ↓
Game entity
   ↓
dbContext.Games.Add(...)
   ↓
SaveChangesAsync()
   ↓
SQLite
   ↓
201 Created + GameDetailsDto

Entity Framework assigns the generated game ID after SaveChangesAsync() completes.

7. Reading Games

For a list of games, EF Core queries the database and projects each entity into a DTO. Genre information can be included through the relationship between Game and Genre.

For a single game, the ID in /games/{id} is used to locate the matching database record. If no record exists, the API returns 404 Not Found.

8. Updating a Game

The API finds the existing game by ID, changes its properties using the values from UpdateGameDtos, and calls:

await dbContext.SaveChangesAsync();

A successful update returns 204 No Content.

9. Deleting a Game

The API locates the game, removes it from the EF Core DbSet, and saves the changes.

A successful delete returns 204 No Content; an unknown ID returns 404 Not Found.

10. React Frontend

The React frontend provides the user interface. React components send HTTP requests to the ASP.NET Core API and update the UI based on the returned data.

During local development, Vite can proxy requests beginning with /api to the ASP.NET backend:

Browser / React
   ↓
/api/games
   ↓
Vite development proxy
   ↓
ASP.NET Core API
   ↓
SQLite

This keeps frontend requests simple while developing locally.

Running Locally

Prerequisites

For local development you need:

.NET 10 SDK

Node.js and npm

EF Core CLI tools

Backend

From the API directory:

cd GameStore.Api
dotnet restore
dotnet ef database update
dotnet run

Frontend

From the React directory:

cd GameStore.React
npm install
npm run dev

Open the Vite URL shown in the terminal.

Making It Available on Any Device

For other people to use the project without installing .NET, Node.js, SQLite, or any development tools, the application must be deployed to the internet.

After deployment, users only need a web browser:

Phone / Laptop / Tablet
        ↓
   Public website URL
        ↓
   React frontend
        ↓
   Hosted ASP.NET API
        ↓
   Hosted database

A production deployment should:

Host the React frontend on a static/web hosting service.

Host the ASP.NET Core API on a .NET-compatible cloud service.

Configure the frontend to use the public API URL instead of the local Vite proxy.

Use a persistent production database or persistent storage.

Store connection strings and other environment-specific values in environment variables rather than source code.

Note: SQLite is excellent for learning and local development, but when deploying to infrastructure with an ephemeral filesystem, use persistent storage or migrate to a hosted database such as PostgreSQL or SQL Server.

Production Frontend Configuration

Instead of hardcoding localhost, use a Vite environment variable:

const apiUrl = import.meta.env.VITE_API_URL;

Example production environment value:

VITE_API_URL=https://your-api.example.com

Then requests can be made with:

fetch(`${apiUrl}/games`);

For local development you can keep the Vite proxy if preferred.

Database Migrations

Create a migration:

dotnet ef migrations add MigrationName --output-dir Data\Migrations

Apply migrations:

dotnet ef database update

The project also contains startup migration support so the database schema can be updated as the application starts.

API Design

The project follows several useful backend practices:

Endpoint grouping

Async database operations

DTOs for API boundaries

Entity Framework Core for persistence

Proper HTTP status codes

Separation between models, DTOs, endpoints, and data access

Database migrations

Validation

What I Learned

This project helped reinforce:

Building REST APIs with ASP.NET Core Minimal APIs

Structuring a .NET backend

CRUD operations

Entity Framework Core

SQLite and relational data

Database migrations

DTO design

Async/await

HTTP status codes

Connecting a React frontend to a .NET backend

React and TypeScript component development

Debugging frontend/backend integration

Git and GitHub workflows

Repository

GitHub: https://github.com/kawsarAhmed2211/DotNetProject

Author

Kawsar Ahmed

Full-stack web/app developer
