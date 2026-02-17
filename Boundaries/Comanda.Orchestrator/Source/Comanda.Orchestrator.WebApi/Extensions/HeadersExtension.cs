using System.Text.Json;
using System.Web;

namespace Comanda.Orchestrator.WebApi.Extensions;

public static class HeadersExtension
{
    public static IHeaderDictionary WithWebLinking<TScheme>(
        this HttpResponse response, PaginationScheme<TScheme> pagination, HttpRequest request)
    {
        var baseUrl = $"{request.Scheme}://{request.Host}{request.Path}";
        var query = HttpUtility.ParseQueryString(request.QueryString.ToString());

        string BuildUrl(int pageNumber)
        {
            query.Set("pagination.pageNumber", pageNumber.ToString());
            query.Set("pagination.pageSize", pagination.PageSize.ToString());

            return $"{baseUrl}?{query}";
        }

        var links = new (bool Condition, string Value)[]
        {
            (pagination.PageNumber > 0, $"<{BuildUrl(0)}>; rel=\"first\""),
            (pagination.HasPreviousPage, $"<{BuildUrl(pagination.PageNumber - 1)}>; rel=\"prev\""),
            (pagination.HasNextPage, $"<{BuildUrl(pagination.PageNumber + 1)}>; rel=\"next\""),
            (pagination.TotalPages > 1, $"<{BuildUrl(pagination.TotalPages - 1)}>; rel=\"last\"")
        };

        var validLinks = links.Where(link => link.Condition).Select(link => link.Value).ToList();
        if (validLinks.Count > 0)
        {
            response.Headers.Append(Headers.Link, string.Join(", ", validLinks));
        }

        return response.Headers;
    }

    public static HttpResponse WithPagination<TScheme>(
        this HttpResponse response, PaginationScheme<TScheme> pagination)
    {
        var metadata = new
        {
            pagination.Total,
            pagination.PageNumber,
            pagination.PageSize,
            pagination.TotalPages,
            pagination.HasNextPage,
            pagination.HasPreviousPage
        };

        response.Headers.Append(Headers.Pagination, JsonSerializer.Serialize(metadata));

        return response;
    }

    public static HttpResponse WithResourceLocation(
        this HttpResponse response, HttpRequest request, string resourceId)
    {
        var baseUrl = $"{request.Scheme}://{request.Host}{request.Path}";
        var query = HttpUtility.ParseQueryString(request.QueryString.ToString());

        query.Set("id", resourceId);
        response.Headers.Append(Headers.Location, $"{baseUrl}?{query}");

        return response;
    }
}
