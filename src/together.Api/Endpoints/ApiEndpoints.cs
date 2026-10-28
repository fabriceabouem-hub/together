using Microsoft.EntityFrameworkCore;
using together.Api.Data;
using together.Api.Model;

namespace together.Api.Endpoints;

    public record CreateUserDto(string Username, string Email, string? City);
    public record CreateGroupDto(string Name, string? Description, string City,
        string Level, int SportId, int CreatorId);
    public record JoinDto(int UserId);

    public static class ApiEndpoints
    {
        public static void MapApiEndpoints(this WebApplication app)
        {
            var api = app.MapGroup("/api");

            api.MapGet("/sports", async (AppDbContext db) =>
                await db.Sports.ToListAsync());

            // Temporaire : sera remplacé par l'inscription avec mot de passe (étape 5)
            api.MapPost("/users", async (CreateUserDto dto, AppDbContext db) =>
            {
                var user = new AppUser
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    City = dto.City,
                    PasswordHash = "temp"
                };
                db.Users.Add(user);
                await db.SaveChangesAsync();
                return Results.Created($"/api/users/{user.Id}",
                    new { user.Id, user.Username, user.Email, user.City });
            });

            api.MapGet("/groups", async (AppDbContext db, int? sportId, string? city) =>
            {
                var query = db.Groups.AsQueryable();
                if (sportId is not null) query = query.Where(g => g.SportId == sportId);
                if (!string.IsNullOrWhiteSpace(city)) query = query.Where(g => g.City == city);
                return await query.Select(g => new
                {
                    g.Id,
                    g.Name,
                    g.Description,
                    g.City,
                    g.Level,
                    Sport = g.Sport!.Name,
                    Members = g.Memberships.Count
                }).ToListAsync();
            });

            api.MapPost("/groups", async (CreateGroupDto dto, AppDbContext db) =>
            {
                if (!await db.Sports.AnyAsync(s => s.Id == dto.SportId) ||
                    !await db.Users.AnyAsync(u => u.Id == dto.CreatorId))
                    return Results.BadRequest("Sport ou utilisateur inconnu.");

                var group = new SportGroup
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    City = dto.City,
                    Level = dto.Level,
                    SportId = dto.SportId,
                    CreatorId = dto.CreatorId
                };
                group.Memberships.Add(new Membership { UserId = dto.CreatorId });
                db.Groups.Add(group);
                await db.SaveChangesAsync();
                return Results.Created($"/api/groups/{group.Id}", new { group.Id, group.Name });
            });

            api.MapPost("/groups/{id}/join", async (int id, JoinDto dto, AppDbContext db) =>
            {
                if (!await db.Groups.AnyAsync(g => g.Id == id) ||
                    !await db.Users.AnyAsync(u => u.Id == dto.UserId))
                    return Results.NotFound();
                if (await db.Memberships.AnyAsync(m => m.GroupId == id && m.UserId == dto.UserId))
                    return Results.Conflict("Déjà membre de ce groupe.");

                db.Memberships.Add(new Membership { GroupId = id, UserId = dto.UserId });
                await db.SaveChangesAsync();
                return Results.Ok();
            });
        }
    }

