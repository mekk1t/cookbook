using Microsoft.AspNetCore.Mvc;

namespace StaticSite.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index() => View();
    }
}
