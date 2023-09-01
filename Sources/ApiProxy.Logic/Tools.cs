using Newtonsoft.Json;
using ApiProxy.Logic.Models;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using ApiProxy.Logic.Database;

namespace ApiProxy.Logic
{
    public static class Tools
    {
        private static HttpClient _httpClient = new HttpClient();

        public static void ErrorProcessing(HttpResponseMessage response)
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
                exception.Data["MessageForClient"] = messages[i];
                exception.Data["MessageFromTheApiServer"] = response.Content.ReadAsStringAsync().Result;
                throw exception;
            }
        }
        public static void SendRequest(HttpMethod method, string url, string? accessToken)
        {
            using var request = new HttpRequestMessage(method, url);
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            try
            {
                using var response = _httpClient.Send(request);
                ErrorProcessing(response);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
        public static T? SendRequest<T>(HttpMethod method, string url, string? accessToken) where T : class
        {
            using var request = new HttpRequestMessage(method, url);
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            T? resJson;
            try
            {
                using var response = _httpClient.Send(request);
                ErrorProcessing(response);
                resJson = JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return resJson;
        }
        public static T2? SendRequest<T1, T2>(T1? body, HttpMethod method, string url, string? accessToken) where T1 : HttpContent where T2 : class
        {
            using var request = new HttpRequestMessage(method, url);
            request.Content = body;
            request.Headers.Add("Authorization", "Bearer " + accessToken);
            T2? resJson;
            try
            {
                using var response = _httpClient.Send(request);
                ErrorProcessing(response);
                resJson = JsonConvert.DeserializeObject<T2>(response.Content.ReadAsStringAsync().Result);
            }
            catch (Exception exception)
            {
                throw exception;
            }
            return resJson;
        }
        public static string ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        public static void SendCallback<T>(object args)
        {
            object[] argList = (object[])args;
            HttpClient httpClient = (HttpClient)argList[2];
            int i = 0;
            using var request = new HttpRequestMessage(HttpMethod.Post, (string)argList[1]);
            request.Headers.Remove("Accept");
            request.Headers.Add("Accept", "application/json");
            request.Content = new StringContent(JsonConvert.SerializeObject((T)argList[0]), Encoding.UTF8, "application/json");
            var response = httpClient.Send(request);
            while (i < 12 && response.StatusCode != HttpStatusCode.OK)
            {
                response.Dispose();
                Thread.Sleep(300000);
                i++;
                response = httpClient.Send(request);
            }
            response.Dispose();
        }
        public static Database.Merchant? FindMerchant(MyDbContext db, string login, string password)
        {
            List<Database.Merchant> merchants = db.Merchants.AsNoTracking().ToList();
            if (merchants == null)
                return null;
            return merchants.FirstOrDefault(m => ComputeSha256Hash(login + m.MerchantGuid + m.MerchantDate) == m.MerchantLogin && ComputeSha256Hash(password + m.MerchantGuid + m.MerchantDate) == m.MerchantPassword);
        }
    }
}