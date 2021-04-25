using Microsoft.AspNetCore.Mvc;

namespace StaticSite.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index() => View("Login");
    }
}
