using Microsoft.AspNetCore.Mvc;

namespace ApiProxy.UI.Controllers
{
    [Route("{controller}")]
    public class CallbackController : Controller// Заглушка
    {
        public IActionResult Invoices(string id)
        {
            return Ok();
        }
    }
}
