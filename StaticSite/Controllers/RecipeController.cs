using Microsoft.AspNetCore.Mvc;

namespace StaticSite.Controllers
{
    public class RecipeController : Controller
    {
        public IActionResult Index() => View();
    }
}
