using PAW.Architecture.Providers;
using System.Text.Json.Serialization;

namespace PAW.Web.Services;

public interface IExchangeRateService
{
    Task<TipoCambioResponse> GetExchangeRateAsync(CancellationToken ct = default);
}

public class ExchangeRateService : IExchangeRateService
{
    public async Task<TipoCambioResponse> GetExchangeRateAsync(CancellationToken ct = default)
    {
        var client = new TipoCambioClient();
        return await client.GetTipoCambioAsync(ct);
    }
}

// DTO Data transfer Objects for the API response
public class TipoCambioResponse
{
    [JsonPropertyName("dolar")]
    public Dolar Dolar { get; set; } = new();

    [JsonPropertyName("euro")]
    public Euro Euro { get; set; } = new();
}

public class Dolar
{
    [JsonPropertyName("venta")]
    public ValorFecha Venta { get; set; } = new();

    [JsonPropertyName("compra")]
    public ValorFecha Compra { get; set; } = new();
}

public class ValorFecha
{
    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("valor")]
    public decimal Valor { get; set; }
}

public class Euro
{
    [JsonPropertyName("fecha")]
    public DateTime Fecha { get; set; }

    [JsonPropertyName("dolares")]
    public decimal Dolares { get; set; }

    [JsonPropertyName("colones")]
    public decimal Colones { get; set; }
}

public class TipoCambioClient(HttpClient? http = null)
{
    private const string Url = "https://api.hacienda.go.cr/indicadores/tc";

    private readonly HttpClient _http = http ?? new HttpClient();

    public async Task<TipoCambioResponse> GetTipoCambioAsync(CancellationToken ct = default)
    {
        using var httpClient = new HttpClient();
        using var req = new HttpRequestMessage(HttpMethod.Get, "https://api.hacienda.go.cr/indicadores/tc")
        {
            Version = System.Net.HttpVersion.Version11,
            VersionPolicy = HttpVersionPolicy.RequestVersionExact
        };
        req.Headers.UserAgent.ParseAdd("curl/8.7.1");
        req.Headers.Accept.ParseAdd("*/*");

        var res = await httpClient.SendAsync(req, ct);
        var content = await res.Content.ReadAsStringAsync(ct);
        var result = await JsonProvider.DeserializeAsync<TipoCambioResponse>(content)
            ?? throw new InvalidOperationException("Empty response from tipo de cambio API.");
        return result;
    }
}
