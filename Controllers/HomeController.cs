using Microsoft.AspNetCore.Mvc;
using Trellol.Models;

namespace Trellol.Controllers
{
    public class HomeController : Controller
    {   
        //Just view, no logic
        public IActionResult Index()
        {
            return View();
        }
    }
}
