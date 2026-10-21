namespace Comanda.Subscriptions.CrossCutting.Extensions;

public static class UrlExtensions
{
    public static string WithoutTrailingSlash(this string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return url;

        return url.EndsWith("/")
            ? url[..^1]
            : url;
    }
}