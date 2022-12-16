using GymApplication.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GymApplication.Controllers
{
    public class Account : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string? phone, string? password)
        {
            if ((phone != null) && (password != null))
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        User? user = db.Users.FirstOrDefault(u => (u.Phone == phone)
                        && (u.Password == password));

                        if (user is null)
                        {
                            return Redirect("/Messages/AccountNotFound");
                        }
                        else
                        {
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Phone),
                                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                            };
                            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                            HttpContext.SignInAsync(claimsPrincipal);
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                return RedirectToAction("Error", "Messages");
            }

            return Redirect("/Account/PersonalAccount");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Redirect("/Home/Index");
        }


        [HttpGet]
        [Authorize(Roles = "admin, user")]
        public IActionResult PersonalAccount()
        {
            string? role = HttpContext.User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)?.Value;

            if (role == "admin")
            {
                return Redirect("/Account/AdminAccount");
            }
            else if (role == "user")
            {
                return Redirect("/Account/UserAccount");
            }
            else
            {
                return RedirectToAction("Error", "Messages");
            }
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        public IActionResult UserAccount()
        {
            string? phone = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            if (phone != null)
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        User? user = db.Users.Include(u => u.Membership).FirstOrDefault(u => u.Phone == phone);

                        if (user != null)
                        {
                            ViewBag.User = user;
                            ViewBag.Membership = user.Membership;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                return RedirectToAction("Error", "Messages");
            }

            return View();
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult AdminAccount()
        {
            string? phone = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;

            if (phone != null)
            {
                try
                {
                    using (var db = new GymContext())
                    {
                        User? user = db.Users.Include(u => u.Membership).FirstOrDefault(u => u.Phone == phone);

                        if (user != null)
                        {
                            ViewBag.User = user;
                            ViewBag.Membership = user.Membership;
                        }
                        else
                        {
                            throw new Exception();
                        }
                    }
                }
                catch
                {
                    return RedirectToAction("Error", "Messages");
                }
            }
            else
            {
                return RedirectToAction("Error", "Messages");
            }

            return View();
        }
    }
}
