using Microsoft.AspNetCore.Mvc;

namespace Mamba.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {   
            return View();
        }
    }
}
