using ApiProxy.Logic.Boundaries;
using ApiProxy.Logic.Dto;
using Newtonsoft.Json;

namespace ApiProxy.Logic
{
    public class Api
    {
        private readonly string _apiUrl;
        private readonly string _innerCallbackInvoices;
		private readonly IRepository _repository;
		private readonly HttpClient _httpClient;
		private readonly Tools _tools;
        private readonly string _refreshToken;
        private string _accessToken;

        public Api(IServiceLocator serviceLocator, string apiUrl, string innerCallbackInvoices, string refreshToken)
        {
            _repository = serviceLocator.Resolve<IRepository>();
			_httpClient = serviceLocator.Resolve<HttpClient>();
			_tools = serviceLocator.Resolve<Tools>();
			_apiUrl = apiUrl;
			_innerCallbackInvoices = innerCallbackInvoices;
			_refreshToken = refreshToken;
        }

        private void GetAccessToken()
        {
			try
			{
				using var request = new HttpRequestMessage(HttpMethod.Post, _apiUrl + "/v1/auth/access-token");
				request.Headers.Add("Authorization", "Bearer " + _refreshToken);
				using var response = _httpClient.Send(request);
				AccessTokenResponse? resJson = JsonConvert.DeserializeObject<AccessTokenResponse>(response.Content.ReadAsStringAsync().Result);
				if (resJson == null)
					throw new Exception("Failed to get access token. Most likely, refresh token is not correct. Immediately replace the refresh token value with the correct one");
				else
					_accessToken = resJson.AccessToken;
			}
			catch (Exception e)
			{
				throw e;
			}
        }
        private Dictionary<string, string> GetRedirectAndCallbackUrls(string id)
        {
			try
			{
				var merchant = _repository.FindMerchant(id);
				var res = new Dictionary<string, string>();
				res["RedirectUrl"] = merchant.RedirectUrl;
				res["CallbackUrl"] = merchant.CallbackUrl;
				return res;
			}
			catch (Exception e)
			{
				throw e;
			}
        }
		
		public List<Fiat>? Fiats(string fiatsUrl)
        {
			try
			{
				GetAccessToken();
				var resUrl = $"{_apiUrl}{fiatsUrl}";
				return _tools.SendRequest<List<Fiat>>(HttpMethod.Get, resUrl, _accessToken); 
			}
			catch (Exception e)
			{
				throw e;
			}
        }
        public InvoiceResponse InvoicesCryptocurrency(string? nameCoin, decimal? amount, string invoicesUrl, string id)
        {
			try
			{
				if (string.IsNullOrEmpty(nameCoin) || amount == null)
					throw new Exception("Set correct values");
	
				GetAccessToken();
				var redirectAndCallbackUrls = GetRedirectAndCallbackUrls(id);
				var resUrl = $"{_apiUrl}{invoicesUrl}";
				var body = new InvoiceCryptocurrencyCreate(redirectAndCallbackUrls["RedirectUrl"], $"{_innerCallbackInvoices}{id}", nameCoin, $"{amount}");
				var content = body.ToStringContent();
				var invoice = _tools.SendRequest<StringContent, Dto.Invoice>(content, HttpMethod.Post, resUrl, _accessToken);
				return InvoiceResponse.ToConvert(invoice, redirectAndCallbackUrls["CallbackUrl"]);
			}
			catch (Exception e)
			{
				throw e;
			}
        }
        public InvoiceResponse InvoicesFiat(string? nameFiat, decimal? amount, string invoicesUrl, string id)
        {
			try
			{
				if (string.IsNullOrEmpty(nameFiat) || amount == null)
					throw new Exception("Set correct values");
	
				GetAccessToken();
				var redirectAndCallbackUrls = GetRedirectAndCallbackUrls(id);
				var resUrl = $"{_apiUrl}{invoicesUrl}";
				var body = new InvoiceFiatCreate(redirectAndCallbackUrls["RedirectUrl"], $"{_innerCallbackInvoices}{id}", nameFiat, $"{amount}");
				var content = body.ToStringContent();
				var invoice = _tools.SendRequest<StringContent, Dto.Invoice>(content, HttpMethod.Post, resUrl, _accessToken);
				return InvoiceResponse.ToConvert(invoice, redirectAndCallbackUrls["CallbackUrl"]);
			}
			catch (Exception e)
			{
				throw e;
			}
        }
    }
}