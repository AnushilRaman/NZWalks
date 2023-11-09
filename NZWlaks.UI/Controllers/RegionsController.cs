using Microsoft.AspNetCore.Mvc;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        public IActionResult Index()
        {
            // Get all region from Web Api
            return View();
        }
    }
}
