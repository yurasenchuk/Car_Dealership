using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Car_Dealership.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
 
namespace Car_Dealership.Controllers
{
    public class AccountController : Controller
    {
        private CarDealershipContext db;
        public AccountController(CarDealershipContext context)
        {
            db = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                Customer user = await db.Customer.FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);
                if (user==null)
                {
                    ViewBag.Str = "Incorrect user or password!";
                    return View(model);
                }
                if (!user.IsActive)
                {
                    ModelState.AddModelError("403", "User is deactivated! Login with another account or create new!");
                }
                else
                {
                    if (user != null)
                    {
                        await Authenticate(user.Role, user.Id);
                        if (user.Role == "admin")
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else if(user.Role == "user")
                        {
                            return RedirectToAction("Index", "Auto");
                        }
                    }
                    ModelState.AddModelError("", "Incorrect!");   
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Registration model)
        {
            if (ModelState.IsValid)
            {
                Customer user = await db.Customer.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    db.Customer.Add(new Customer { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email, Password = model.Password, Role = model.Role, IsActive = true});
                    await db.SaveChangesAsync();
 
                    await Authenticate(model.Role, model.Id);

                    return RedirectToAction("Login", "Account");
                }
                else
                    ModelState.AddModelError("", "Incorrect!");
            }
            return View(model);
        }

        private async Task Authenticate(string role, int id)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, Convert.ToString(id)),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            };
            ClaimsIdentity iD = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(iD));
        }
 
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}