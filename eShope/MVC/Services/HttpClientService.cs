using System.Text;
using IdentityModel.Client;
using MVC.Services.Interfaces;
using Newtonsoft.Json;

namespace MVC.Services;

public class HttpClientService : IHttpClientService
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<HttpClientService> _logger;

    public HttpClientService(IHttpClientFactory clientFactory,
        ILogger<HttpClientService> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<TResponse> SendAsync<TResponse, TRequest>(string url, HttpMethod method, TRequest? content)
    {
        var client = _clientFactory.CreateClient();
        client.SetBearerToken(await GetClientCredentialsTokenAsync());

        var httpMessage = new HttpRequestMessage();
        httpMessage.RequestUri = new Uri(url);
        httpMessage.Method = method;

        if (content != null)
            httpMessage.Content =
                new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");
        var result = await client.SendAsync(httpMessage);
        if (result.IsSuccessStatusCode)
        {
            var resultContent = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<TResponse>(resultContent);
            return response;
        }

        return default !;
    }

    private async Task<string> GetClientCredentialsTokenAsync()
    {
        var client = _clientFactory.CreateClient();
        var tokenResponse = await client
            .RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = await GetDiscoveryDocumentAsync(),
                ClientId = "client",
                ClientSecret = "secret",
                Scope = "catalogitem"
            });

        if (tokenResponse.IsError)
        {
            throw new Exception(
                $"RequestClientCredentialsTokenAsync faild with the following error: {tokenResponse.Error}", tokenResponse.Exception);
        }

        return tokenResponse.AccessToken;
    }

    private async Task<string>? GetDiscoveryDocumentAsync()
    {
        var client = _clientFactory.CreateClient();
        var discoveryDocument = await client
            .GetDiscoveryDocumentAsync("http://localhost:7001");
        if (discoveryDocument.IsError)
        {
            Console.WriteLine(discoveryDocument.Error);
            return string.Empty;
        }

        return discoveryDocument.TokenEndpoint;
    }
}