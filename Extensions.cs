using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;

namespace PublicApi
{
    /// <summary>
    /// То же, что и JsonResult, но с возможностью устанавливать код ответа
    /// </summary>
    public class CustomJsonResult : JsonResult
    {
        private readonly HttpStatusCode _statusCode;

        public CustomJsonResult(object json) : this(json, HttpStatusCode.InternalServerError) { }

        public CustomJsonResult(object json, HttpStatusCode statusCode) : base(json)
        {
            _statusCode = statusCode;
        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)_statusCode;
            base.ExecuteResult(context);
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)_statusCode;
            return base.ExecuteResultAsync(context);
        }
    }
}
