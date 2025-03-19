using ZBlogApp.Library;
using ZBlogApp.Web.Components.Pages;

namespace ZBlogApp.Web;

public class ArticleApiClient(HttpClient httpClient)
{
    public async Task<IQueryable<Article>> GetArticleAsync(CancellationToken cancellationToken = default)
    {

        var articles = await httpClient.GetFromJsonAsync<Article[]>("/api/articles", cancellationToken);
        return articles?.AsQueryable()!;
    }


    public async Task<Article> GetArticleWithIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var article = await httpClient.GetFromJsonAsync<Article>($"/api/articles/{id}", cancellationToken);
        return article!;
    }

    public async Task<IQueryable<Article>> GetArticlesInRangeAsync(DateTime start, DateTime end, CancellationToken cancellationToken = default)
    {
        string url = $"/api/articles/range/{start:O}/{end:O}"; // ISO 8601 format

        var articles = await httpClient.GetFromJsonAsync<Article[]>(url, cancellationToken);

        return articles?.AsQueryable()!;

    }

}
