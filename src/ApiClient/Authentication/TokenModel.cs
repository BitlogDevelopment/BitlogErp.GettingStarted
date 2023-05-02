using System.Text.Json.Serialization;

namespace ApiClient.Authentication;

public class TokenModel
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }
    [JsonPropertyName("token_type")]
    public string TokenType { get; set; }
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }

    public DateTime IssuedAt { get; } = DateTime.UtcNow;
    public bool HasExpired => IssuedAt.AddSeconds(ExpiresIn) <= DateTime.UtcNow;
}
