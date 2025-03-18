using Microsoft.EntityFrameworkCore;
using ZBlogApp.BlazorServer.Data;

namespace ZBlogApp.ApiService.Services;

public class RoleService
{
    private readonly ApplicationDbContext dbContext;

    public RoleService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<ApplicationRole>> GetAllRolesAsync()
    {
        return await dbContext.Roles.ToListAsync();
    }

    public async Task<ApplicationRole> GetRoleByIdAsync(string id)
    {
        var role = await dbContext.Roles.FindAsync(id);
        if (role == null)
        {
            throw new KeyNotFoundException("The role was not found.");
        }
        return role;
    }

    public async Task<ApplicationRole> GetRoleByNameAsync(string name)
    {
        var role = await dbContext.Roles.FirstOrDefaultAsync(r => r.Name == name);
        if (role == null)
        {
            throw new KeyNotFoundException("The role was not found.");
        }
        return role;
    }

    public async Task<ApplicationRole> CreateRoleAsync(ApplicationRole role)
    {
        var result = await dbContext.Roles.AddAsync(role);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<ApplicationRole> UpdateRoleAsync(ApplicationRole role)
    {
        var result = dbContext.Roles.Update(role);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<ApplicationRole> DeleteRoleAsync(string id)
    {
        var role = await dbContext.Roles.FindAsync(id);
        if (role == null)
        {
            throw new KeyNotFoundException("The role was not found.");
        }
        var result = dbContext.Roles.Remove(role);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    // Register end points
    public static void RegisterRoutes (IEndpointRouteBuilder app)
    {
        app.MapGet("/api/roles", async (RoleService service) =>
        {
            var roles = await service.GetAllRolesAsync();
            return Results.Ok(roles);
        });

        app.MapGet("/api/roles/{id}", async (RoleService service, string id) =>
        {
            var role = await service.GetRoleByIdAsync(id);
            return Results.Ok(role);
        });

        app.MapGet("/api/roles/name/{name}", async (RoleService service, string name) =>
        {
            var role = await service.GetRoleByNameAsync(name);
            return Results.Ok(role);
        });

        app.MapPost("/api/roles", async (RoleService service, ApplicationRole role) =>
        {
            var newRole = await service.CreateRoleAsync(role);
            return Results.Created($"/api/roles/{newRole.Id}", newRole);
        });

        app.MapPut("/api/roles", async (RoleService service, ApplicationRole role) =>
        {
            var updatedRole = await service.UpdateRoleAsync(role);
            return Results.Ok(updatedRole);
        });

        app.MapDelete("/api/roles/{id}", async (RoleService service, string id) =>
        {
            await service.DeleteRoleAsync(id);
            return Results.NoContent();
        });
    }
}
