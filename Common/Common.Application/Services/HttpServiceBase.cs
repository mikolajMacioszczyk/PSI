using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Common.Application.Services;

public abstract class HttpServiceBase
{
    protected readonly HttpClient _httpClient;

    public HttpServiceBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<T?> Get<T>(string relativePath)
        where T : class
    {
        var response = await _httpClient.GetAsync(relativePath);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseContent, SerializerOptions.Value);
        }
        else if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
    }

    protected static readonly Lazy<JsonSerializerOptions> SerializerOptions = new(InitializeSerializerOptions());

    protected static JsonSerializerOptions InitializeSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            PropertyNameCaseInsensitive = true,
        };
        options.Converters.Add(new JsonStringEnumConverter());
        return options;
    }
}
