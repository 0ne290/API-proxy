using System.Net;
using Serilog;

namespace ApiProxy.UI
{
    public static class Tools
    {
        public static void ErrorProcessing(Exception exception)
		{
			if (exception.Message == "ErrorProcessingException")
                Log.Warning($"ErrorProcessingException! StatusCode: {(HttpStatusCode)exception.Data["StatusCode"]}; Messages: {(string)exception.Data["Message1"]}; {(string)exception.Data["Message2"]}; StackTrace: {exception.StackTrace}");
            else
                Log.Error($"AnyException! Message: {exception.Message}; StackTrace: {exception.StackTrace}");
		}
    }
}