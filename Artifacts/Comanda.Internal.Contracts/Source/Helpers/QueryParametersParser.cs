using System.Reflection;
using System.Text;

namespace Comanda.Internal.Contracts.Helpers;

public static class QueryParametersParser
{
    public static string ToQueryString<TParameters>(TParameters instance)
    {
        if (instance is null)
            return string.Empty;

        var properties = typeof(TParameters).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var stringBuilder = new StringBuilder();

        bool first = true;

        foreach (var property in properties)
        {
            var value = property.GetValue(instance);
            if (value is null)
                continue;

            string name = ToCamelCase(property.Name);

            if (value.GetType().Name.Contains("Pagination"))
            {
                var paginationProperties = value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var propertyMetadata in paginationProperties)
                {
                    var pagePropName = propertyMetadata.Name;
                    if (pagePropName == "PageNumber" || pagePropName == "PageSize")
                    {
                        var pageValue = propertyMetadata.GetValue(value);
                        if (pageValue is not null)
                        {
                            if (!first)
                            {
                                stringBuilder.Append('&');
                            }
                            else
                            {
                                first = false;
                            }

                            stringBuilder.Append($"{name}.{ToCamelCase(pagePropName)}={pageValue}");
                        }
                    }
                }
            }
            else
            {
                string stringValue = value switch
                {
                    bool builder => builder.ToString().ToLowerInvariant(),
                    _ => value?.ToString() ?? string.Empty
                };

                if (!first)
                {
                    stringBuilder.Append('&');
                }
                else
                {
                    first = false;
                }

                stringBuilder.Append($"{name}={stringValue}");
            }
        }

        return stringBuilder.ToString();
    }

    private static string ToCamelCase(string value)
    {
        if (string.IsNullOrEmpty(value) || char.IsLower(value[0]))
            return value;

        return char.ToLowerInvariant(value[0]) + value.Substring(1);
    }
}
