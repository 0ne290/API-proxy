using System.Net;
using ApiProxy.Logic;

namespace ApiProxy.UI;

public class Tools:ITools
{
    public Tools(IServiceLocator serviceLocator)
    {
        ServiceLocator = serviceLocator;
    }

    public void ErrorProcessing(Exception exception)
    {
        var log = ServiceLocator.Resolve<Serilog.ILogger>();
        if (exception.Message == "ErrorProcessingException")
            log.Warning($"ErrorProcessingException! StatusCode: {(HttpStatusCode)exception.Data["StatusCode"]}; Messages: {(string)exception.Data["Message1"]}; {(string)exception.Data["Message2"]}; StackTrace: {exception.StackTrace}");
        else
            log.Error($"AnyException! Message: {exception.Message}; StackTrace: {exception.StackTrace}");
    }

    IServiceLocator ServiceLocator { get; set; }
}