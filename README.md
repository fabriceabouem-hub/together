# together

App, die Sportler für gemeinsame Aktivitäten zusammenbringt.

## Stack
- C# / ASP.NET Core 10 (Web API)
- Entity Framework Core + SQLite

### Run des Projekts
cd src/together.Api
dotnet ef database update
dotnet run

### Endpoints 
- GET /api/sports
- GET /api/groups?sportId=&city=
- POST /api/groups
- POST /api/groups/{id}/join
