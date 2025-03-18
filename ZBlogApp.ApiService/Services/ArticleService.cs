using Microsoft.EntityFrameworkCore;
using ZBlogApp.BlazorServer.Data;
using ZBlogApp.Library;

namespace ZBlogApp.ApiService.Services;

public class ArticleService
{
    private readonly ApplicationDbContext dbContext;

    public ArticleService(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    // Read
    public async Task<List<Article>> GetArticlesAsync()
    {
        return await dbContext.Articles
            .OrderBy(a => a.Id)
            .ToListAsync();
    }

    public async Task<Article> GetArticleAsync(string id)
    {
        var article = await dbContext.Articles.FindAsync(id);
        if (article == null)
        {
            throw new KeyNotFoundException("The article was not found.");
        }
        return article;
    }

    public async Task<List<Article>> GetArticlesHasWordInTitleAsync(string word)
    {
        var articles = await dbContext.Articles.Where(a => a.Title != null && a.Title.ToLower().Contains(word)).ToListAsync();
        return articles;
    }

    public async Task<List<Article>> GetArticlesHasWordInBodyAsync(string word)
    {
        var articles = await dbContext.Articles.Where(a => a.Body != null && a.Body.ToLower().Contains(word)).ToListAsync();
        return articles;
    }

    public async Task<List<Article>> GetArticlesByAuthorAsync(string author)
    {
        var articles = await dbContext.Articles.Where(a => a.ContributorUserName != null && a.ContributorUserName.ToLower().Contains(author)).ToListAsync();
        return articles;
    }

    public async Task<List<Article>> GetArticlesInRangeAsync(DateTime start, DateTime end)
    {
        var articles = await dbContext.Articles
            .Where(a => a.StartDate >= start && a.EndDate <= end)
            .ToListAsync();
        return articles;
    }

    //Create, Update, Delete
    public async Task<Article> CreateArticleAsync(Article article)
    {
        dbContext.Articles.Add(article);
        await dbContext.SaveChangesAsync();
        return article;
    }

    public async Task<Article> UpdateArticleAsync(Article article)
    {
        dbContext.Entry(article).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return article;
    }

    public async Task DeleteArticleAsync(string id)
    {
        var article = await dbContext.Articles.FindAsync(id);
        if (article == null)
        {
            throw new KeyNotFoundException("The article was not found.");
        }
        dbContext.Articles.Remove(article);
        await dbContext.SaveChangesAsync();
    }

    // Register endpoints
    public static void RegisterRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/articles", async (ArticleService service) =>
        {
            return Results.Ok(await service.GetArticlesAsync());
        });

        app.MapGet("/api/articles/{id}", async (ArticleService service, string id) =>
        {
            return Results.Ok(await service.GetArticleAsync(id));
        });

        app.MapGet("/api/articles/title/{word}", async (ArticleService service, string word) =>
        {
            return Results.Ok(await service.GetArticlesHasWordInTitleAsync(word));
        });

        app.MapGet("/api/articles/body/{word}", async (ArticleService service, string word) =>
        {
            return Results.Ok(await service.GetArticlesHasWordInBodyAsync(word));
        });

        app.MapGet("/api/articles/author/{author}", async (ArticleService service, string author) =>
        {
            return Results.Ok(await service.GetArticlesByAuthorAsync(author));
        });

        app.MapGet("/api/articles/range/{start}/{end}", async (ArticleService service, DateTime start, DateTime end) =>
        {
            return Results.Ok(await service.GetArticlesInRangeAsync(start, end));
        });

        app.MapPost("/api/articles", async (ArticleService service, Article article) =>
        {
            return Results.Created($"/api/articles/{(await service.CreateArticleAsync(article)).Id}", article);
        });

        app.MapPut("/api/articles/{id}", async (ArticleService service, string id, Article article) =>
        {
            if (id != article.Id)
            {
                throw new ArgumentException("The ID in the URL does not match the ID in the body.");
            }
            return Results.Ok(await service.UpdateArticleAsync(article));
        });

        app.MapDelete("/api/articles/{id}", async (ArticleService service, string id) =>
        {
            await service.DeleteArticleAsync(id);
            return Results.NoContent();
        });
    }
}
