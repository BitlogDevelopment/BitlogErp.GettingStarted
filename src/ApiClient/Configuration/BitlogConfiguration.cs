namespace ApiClient.Configuration;

public class BitlogConfiguration
{
    public const string SectionName = "Bitlog";

    public string ApiHost { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string ResetKey { get; set; }
    public string TokenHost { get; set; }
}

