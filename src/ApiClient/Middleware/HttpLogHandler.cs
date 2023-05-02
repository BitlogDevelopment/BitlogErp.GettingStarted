using Serilog;

namespace ApiClient.Middleware;

public class HttpLogHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        await LogRequest(request);
        var response = await base.SendAsync(request, cancellationToken);
        await LogResponse(response);

        return response;
    }

    private static async Task LogRequest(HttpRequestMessage request)
    {
        Log.Logger.Debug("Metod: {Method}", request.Method.Method);
        Log.Logger.Debug("URI:   {URI}", request.RequestUri?.ToString());

        if (request.Content is null)
            return;

        var content = await request.Content.ReadAsStringAsync();
        Log.Logger.Debug("Data: {Data}", content.Length > 100 ? content.Substring(0, 100) : content);
    }
    private static async Task LogResponse(HttpResponseMessage response)
    {
        Log.Logger.Debug("Status code: {StatusCode}", response.StatusCode);

        if (response.Content is null)
            return;

        var content = await response.Content.ReadAsStringAsync();
        Log.Logger.Debug("Body: {Content}...", content.Length > 100 ? content.Substring(0, 100) : content);
    }
}
