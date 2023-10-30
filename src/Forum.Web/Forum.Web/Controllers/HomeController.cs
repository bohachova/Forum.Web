using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("ForumMainPage");
            }
            return View();
        }
        [Authorize]
        public IActionResult ForumMainPage()
        {
            return View();
        }
    }
}
