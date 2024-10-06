using Microsoft.AspNetCore.Mvc;

namespace LigaWeb.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult ErrorNotFound()
        {
            return View();
        }
    }
}
