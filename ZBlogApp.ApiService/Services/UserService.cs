using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ZBlogApp.BlazorServer.Data;

namespace ZBlogApp.ApiService.Services;

public class UserService
{
    private readonly ApplicationDbContext dbContext;

    public UserService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<List<ApplicationUser>> GetAllUsersAsync()
    {
        return await dbContext.Users
            .ToListAsync();
    }

    public async Task<ApplicationUser> GetUserByIdAsync(string Id)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u=>u.Id == Id);
        if (user == null)
        {
            throw new KeyNotFoundException("The user was not found.");
        }
        return user;
    }

    public async Task<ApplicationUser> GetUserByEmail(string email)
    {
        var user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            throw new KeyNotFoundException("The user was not found.");
        }
        return user;
    }

    public async Task<List<ApplicationUser>> GetUsersByFirstNameAsync(string firstName)
    {
        var users = await dbContext.Users
            .Where(user => user.FirstName == firstName)
            .ToListAsync();

        if (!users.Any())
        {
            throw new KeyNotFoundException($"The user with first name {firstName} was not found.");
        }
        return users;
    }

    public async Task<List<ApplicationUser>> GetUsersByLastNameAsync(string lastName)
    {
        var users = await dbContext.Users
            .Where(user => user.LastName == lastName)
            .ToListAsync();

        if (!users.Any())
        {
            throw new KeyNotFoundException($"The user with last name {lastName} was not found.");
        }
        return users;
    }
    
    public async Task<ApplicationUser> CreateUserAsync(ApplicationUser user)
    {
        var result = await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<ApplicationUser> UpdateUserAsync(ApplicationUser user)
    {
        var result = dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<ApplicationUser> DeleteUserAsync(string id)
    {
        var user = await dbContext.Users.FindAsync(id);
        if (user == null)
        {
            throw new KeyNotFoundException("The user was not found.");
        }
        var result = dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
        return result.Entity;
    }

    // Register end points
    public static void RegisterRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/users", async (UserService service) =>
        {
            var users = await service.GetAllUsersAsync();
            return Results.Ok(users);
        });

        app.MapGet("/api/users/{id}", async (UserService service, string id) =>
        {
            var user = await service.GetUserByIdAsync(id);
            return Results.Ok(user);
        });

        app.MapGet("/api/users/email/{email}", async (UserService service, string email) =>
        {
            var user = await service.GetUserByEmail(email);
            return Results.Ok(user);
        });

        app.MapGet("/api/users/firstname/{firstName}", async (UserService service, string firstName) =>
        {
            var users = await service.GetUsersByFirstNameAsync(firstName);
            return Results.Ok(users);
        });

        app.MapGet("/api/users/lastname/{lastName}", async (UserService service, string lastName) =>
        {
            var users = await service.GetUsersByLastNameAsync(lastName);
            return Results.Ok(users);
        });

        app.MapPost("/api/users/create", async (UserService service, ApplicationUser user) =>
        {
            var result = await service.CreateUserAsync(user);
            return Results.Created($"/api/users/{result.Id}", result);
        });

        app.MapPut("/api/users/edit/{id}", async (UserService service, ApplicationUser user) =>
        {
            var result = await service.UpdateUserAsync(user);
            return Results.Ok(result);
        });

        app.MapDelete("/api/users/delete/{id}", async (UserService service, string id) =>
        {
            await service.DeleteUserAsync(id);
            return Results.NoContent();
        });
    }
}
