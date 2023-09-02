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
	public TokenStorage(ILogger logger, string getAccessTokenUrl, string refreshToken)
	{
		Logger = logger;
		GetAccessTokenUrl = getAccessTokenUrl;
		RefreshToken = refreshToken;
		
		HttpClient = new HttpClient();
		Timer = new System.Timers.Timer(TimeSpan.FromMinutes(50)) { AutoReset = true };
		Timer.Elapsed += UpdateSettings;
		UpdateSettings(null, null);
		Timer.Start();
	}
	
	public void UpdateSettings(object? sender, ElapsedEventArgs? e)
	{
		using var request = new HttpRequestMessage(HttpMethod.Post, GetAccessTokenUrl);
		request.Headers.Add("Authorization", "Bearer " + RefreshToken);
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
	
	string RefreshToken { get; set; }
	string GetAccessTokenUrl { get; set; }
	HttpClient HttpClient { get; }
	System.Timers.Timer Timer { get; }
	ILogger Logger { get; }
}
