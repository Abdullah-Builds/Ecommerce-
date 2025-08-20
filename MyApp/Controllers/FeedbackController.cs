using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers
{
    public class FeedbackController : Controller
    {
        public IActionResult Feedback()
        {
            return View();
        }
    }
}
