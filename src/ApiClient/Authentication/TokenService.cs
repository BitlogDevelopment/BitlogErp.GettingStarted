using System.Net.Http.Json;
using ApiClient.Configuration;
using Microsoft.Extensions.Options;

namespace ApiClient.Authentication;

public class TokenService : ITokenService
{
    private readonly string _clientId;
    private readonly string _clientSecret;
    private readonly HttpClient _httpClient;
    private TokenModel _token;

    public TokenService(HttpClient httpClient, IOptions<BitlogConfiguration> options)
    {
        var configuration = options.Value ??
            throw new ArgumentNullException(nameof(options));

        _clientId = configuration.ClientId;
        _clientSecret = configuration.ClientSecret;
        _httpClient = httpClient;

        _httpClient.BaseAddress = new Uri(configuration.TokenHost);
    }

    public async Task<string> GetTokenAsync()
    {
        if (_token is null || _token.HasExpired)
        {
            var payload = new Dictionary<string, string>();

            payload.Add("grant_type", "client_credentials");
            payload.Add("client_id", _clientId);
            payload.Add("client_secret", _clientSecret);
            payload.Add("scope", "erp-api");

            var response = await _httpClient.PostAsync("/connect/token", new FormUrlEncodedContent(payload));
            response.EnsureSuccessStatusCode();

            _token = await response.Content.ReadFromJsonAsync<TokenModel>();
        }

        return _token.AccessToken;
    }
}

public interface ITokenService
{
    Task<string> GetTokenAsync();
}
