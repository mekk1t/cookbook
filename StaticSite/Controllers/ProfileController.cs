using Microsoft.AspNetCore.Mvc;

namespace StaticSite.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index() => View();
    }
}
