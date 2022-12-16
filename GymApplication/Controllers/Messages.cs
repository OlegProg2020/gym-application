using Microsoft.AspNetCore.Mvc;

namespace GymApplication.Controllers
{
    public class Messages : Controller
    {
        [HttpGet]
        public IActionResult Message(string? href)
        {
            ViewBag.Href = href ?? "/Home/Index";

            return View();
        }

        [HttpGet]
        public IActionResult NotFound(string? href)
        {
            ViewBag.Href = href ?? "/Home/Index";

            return View();
        }

        [HttpGet]
        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccountNotFound()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Notice(string? href)
        {
            ViewBag.Href = href ?? "/Home/Index";

            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
