using Microsoft.AspNetCore.Mvc;

namespace StaticSite.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        [Route("")]
        public IActionResult Index() => View();

        [Route("ingredients")]
        public IActionResult Ingredients() => View();

        [Route("categories")]
        public IActionResult Categories() => View();

        [Route("recipes")]
        public IActionResult Recipes() => View();
    }
}
