using ZBlogApp.Library;

namespace ZBlogApp.Web;

public class ArticleApiClient(HttpClient httpClient)
{
    public async Task<Article[]> GetArticleAsync(int maxItems = 20, CancellationToken cancellationToken = default)
    {
        List<Article>? articles = null;

        await foreach (var article in httpClient.GetFromJsonAsAsyncEnumerable<Article>("/api/articles", cancellationToken))
        {
            if (articles?.Count >= maxItems)
            {
                break;
            }
            if (article is not null)
            {
                articles ??= [];
                articles.Add(article);
            }
        }

        return articles?.ToArray() ?? [];
    }


    public async Task<Article[]> GetArticlesInRangeAsync(DateTime start, DateTime end, int maxItems = 20, CancellationToken cancellationToken = default)
    {
        List<Article>? articles = null;

        string url = $"/api/articles/range/{start:O}/{end:O}"; // ISO 8601 format

        await foreach (var article in httpClient.GetFromJsonAsAsyncEnumerable<Article>(url, cancellationToken))
        {
            if (articles?.Count >= maxItems)
            {
                break;
            }
            if (article is not null)
            {
                articles ??= [];
                articles.Add(article);
            }
        }

        return articles?.ToArray() ?? [];
    }

    public async Task<Article> GetArticleAsync(string id, CancellationToken cancellationToken = default)
    {
        var article = await httpClient.GetFromJsonAsync<Article>($"/api/articles/{id}", cancellationToken);
        if (article is null)
        {
            throw new InvalidOperationException("Article not found.");
        }
        return article;
    }

}
