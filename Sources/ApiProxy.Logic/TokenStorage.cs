using ApiProxy.Logic.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System.Net.Http;
using System.Timers;

namespace ApiProxy.Logic;

public interface ITokenStorage:IDisposable
{
    string GetToken();
}

public class TokenStorage:ITokenStorage
{
    public TokenStorage(IConfiguration appConfig, ILogger logger)
    {
        AppConfig = appConfig;
        Logger = logger;
        AccessToken = string.Empty;

        HttpClient  = new HttpClient();
        Timer = new System.Timers.Timer(TimeSpan.FromMinutes(50)) { AutoReset = true };
        Timer.Elapsed += UpdateSettings;
        UpdateSettings(null, null);
        Timer.Start();
    }

    public void UpdateSettings(object? sender, ElapsedEventArgs? e)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, (AppConfig["Api:RootUrl"] ?? "https://api.staging.pay2play.cash") + (AppConfig["Api:GetAccessTokenUrl"] ?? "/v1/auth/access-token"));
        var refreshToken = AppConfig["Api:RefreshToken"] ?? "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiIxMjQxMWYyNS1kN2IzLTQ1ZTQtOGJhNC0yNmUyZGFkMzU3N2MiLCJpYXQiOjE2OTA5NzUxMTQsImlzcyI6ImNvcmUtaWQiLCJzdWIiOiJ1c2VyIiwidWlkIjoxNTcwNywidmVyIjoxLCJyZXMiOlsxXSwidHlwIjoxLCJzY29wZXMiOlsiYXV0aDphY3Rpb24iLCJjb3JlOnJlYWQiLCJleGNoYW5nZTphY3Rpb24iLCJleGNoYW5nZTpyZWFkIiwibWVyY2hhbnQ6YWN0aW9uIiwibWVyY2hhbnQ6cmVhZCIsIm5vdGlmaWNhdGlvbnM6YWN0aW9uIiwibm90aWZpY2F0aW9uczpyZWFkIiwicGF5b3V0czphY3Rpb24iLCJwYXlvdXRzOnJlYWQiLCJwcm9maWxlOmFjdGlvbiIsInByb2ZpbGU6cmVhZCIsIndhbGxldDphY3Rpb24iLCJ3YWxsZXQ6cmVhZCJdLCJpc18yZmFfZGlzYWJsZWQiOmZhbHNlLCJuYW1lIjoiTWF4QXBpVG9rZW4iLCJpc19kaXNhYmxlX29ubGluZSI6dHJ1ZX0.K0OxFBt4SgrAGCNlGrAQ2krbBtfr1eM45Ph_MsMcuOEzRu1fZHCCL9O59EpdMzHkU72pj3E8G9tWiTPblZFsEw";
        request.Headers.Add("Authorization", "Bearer " + refreshToken);
        using var response = HttpClient.Send(request);
        var resJson = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content.ReadAsStringAsync().Result);
        if (resJson == null||string.IsNullOrEmpty(resJson.AccessToken))
            Logger.Fatal("From Api/UpdateSettings. Failed to get access token. Most likely, the appsettings.json file contains an incorrect refresh token. Immediately replace the refresh token value with the correct one");
        else
            AccessToken = resJson.AccessToken;
    }

    public string GetToken() => AccessToken;

    public void Dispose()
    {
        HttpClient.Dispose();
        Timer.Dispose();
    }

    string AccessToken { get; set; }
    HttpClient HttpClient { get; }
    System.Timers.Timer Timer { get; }
    IConfiguration AppConfig { get; }
    ILogger Logger { get; }
}