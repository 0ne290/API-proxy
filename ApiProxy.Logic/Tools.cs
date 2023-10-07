using Newtonsoft.Json;
using System.Net;

namespace ApiProxy.Logic
{
    public class Tools
    {
        private readonly HttpClient _httpClient;
		
		public Tools(IServiceLocator serviceLocator)
		{
			_httpClient = serviceLocator.Resolve<HttpClient>();
		}

        public void ErrorProcessing(HttpResponseMessage response)
        {
            int i;
            HttpStatusCode code = response.StatusCode;
            string[] messages = new string[7] { "Authorization token not passed. Please try again or contact support", "The transferred token is invalid, expired, or transferred incorrectly. Contact technical support", "The requested resource does not exist. Check if the URL is correct or contact support", "Error on the API server side. Contact technical support", "Incorrect request to the API server. Contact technical support", "The specified object cannot be processed here and now (invalid status, invoice accepted by the buyer, etc.). Make sure the request is up to date or contact technical support", "Request successful" };
            HttpStatusCode[] codes = new HttpStatusCode[6] { HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden, HttpStatusCode.NotFound, HttpStatusCode.InternalServerError, HttpStatusCode.BadRequest, HttpStatusCode.UnprocessableEntity };
            for (i = 0; i < 6; i++)
                if (codes[i] == code)
                    break;
            if (i < 6)
            {
                Exception exception = new Exception("ErrorProcessingException");
                exception.Data["StatusCode"] = code;
                exception.Data["Message1"] = messages[i];
                exception.Data["Message2"] = response.Content.ReadAsStringAsync().Result;
                throw exception;
            }
        }
        public void SendRequest(HttpMethod method, string url, string? accessToken)
        {
			try
			{
				using var request = new HttpRequestMessage(method, url);
				request.Headers.Add("Authorization", "Bearer " + accessToken);
				using var response = _httpClient.Send(request);
				ErrorProcessing(response);
			}
			catch (Exception e)
			{
				throw e;
			}
        }
        public T? SendRequest<T>(HttpMethod method, string url, string? accessToken) where T : class
        {
			try
			{
				using var request = new HttpRequestMessage(method, url);
				request.Headers.Add("Authorization", "Bearer " + accessToken);
				using var response = _httpClient.Send(request);
				ErrorProcessing(response);
				var resJson = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
				return resJson;
			}
			catch (Exception e)
			{
				throw e;
			}
        }
        public T2? SendRequest<T1, T2>(T1? body, HttpMethod method, string url, string? accessToken) where T1 : HttpContent where T2 : class
        {
			try
			{
				using var request = new HttpRequestMessage(method, url);
				request.Content = body;
				request.Headers.Add("Authorization", "Bearer " + accessToken);
				using var response = _httpClient.Send(request);
				ErrorProcessing(response);
				var resJson = JsonConvert.DeserializeObject<T2>(response.Content.ReadAsStringAsync().Result);
				return resJson;
			}
			catch (Exception e)
			{
				throw e;
			}
        }
    }
}