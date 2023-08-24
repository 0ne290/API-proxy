using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;

namespace ApiProxy.Logic
{
    public class ApiRepository
    {
        //public IActionResult Fiats()
        //{
        //    Init();
        //    HttpStatusCode code;
        //    string error;
        //    string resUrl = _apiUrl + "/v1/fiats";
        //    List<Fiat>? res = Tools.SendRequest<List<Fiat>>(_httpClient, HttpMethod.Get, resUrl, out error, _accessToken, out code);
        //    ResponseJson resJson = new ResponseJson();
        //    resJson.Response = res;
        //    resJson.Message = error;
        //    return new CustomJsonResult(resJson, code);
        //}

        //public void Init()
        //{
        //    _id = ((ClaimsIdentity)(HttpContext.User.Identity)).Name;
        //    DatabaseContext.Merchant? merchant = _db.Merchants.Find(_id);
        //    _db.Dispose();
        //    _redirectUrl = merchant.MerchantRedirectUrl;
        //    _callbackUrl = merchant.MerchantCallbackUrl;
        //}

        //private static IConfiguration _appConfig = null;
        //private static HttpClient _httpClient = new HttpClient();
    }
}