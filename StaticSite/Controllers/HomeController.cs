using Microsoft.AspNetCore.Mvc;

namespace StaticSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
    }
}
