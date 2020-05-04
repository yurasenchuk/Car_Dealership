using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Car_Dealership.Models;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Car_Dealership.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("/[controller]")]
    public class CustomerController : Controller
    {
        CarDealershipContext db;

        public CustomerController(CarDealershipContext context)
        {
            db = context;
            if (!db.Customer.Any())
            {
                db.Customer.Add(new Customer
                {
                    Id = 1, FirstName = "Admin", LastName = "Admin", Email = "car_admin@gmail.com",
                    Password = "123456_Ad", Role = "admin"
                });
                db.SaveChanges();
            }
        }

        [Authorize(Roles = "admin")]
        [Microsoft.AspNetCore.Mvc.HttpGet]
        public async Task<IActionResult> Index()
        {
            if (User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value == "admin")
            {
                var customer = from m in db.Customer
                    select m;
                return View(await customer.ToListAsync());
            }
            else return Content("Not admin!");
        }

        [Authorize(Roles = "admin")]
        [Microsoft.AspNetCore.Mvc.HttpGet("/deactivate/{id}")]
        public async Task<IActionResult> Deactivate(int id)
        {
            Customer cust = db.Customer.Find(id);
            if (cust == null)
            {
                return NotFound();
            }

            if (cust.IsActive)
            {
                cust.IsActive = false;
                db.Customer.Update(cust);
                await db.SaveChangesAsync();
                ViewBag.Str = "User successfully deactivated";
                return View();
            }
            else if (!cust.IsActive)
            {
                cust.IsActive = true;
                db.Customer.Update(cust);
                await db.SaveChangesAsync();
                ViewBag.Str = "User successfully activated";
                return View();
            }

            return BadRequest();
        }

        [Authorize(Roles = "admin")]
        [Microsoft.AspNetCore.Mvc.HttpGet("/editrole/{id}")]
        public async Task<IActionResult> EditRole(int id)
        {
            Customer cust = db.Customer.Find(id);
            if (cust == null)
            {
                return NotFound();
            }

            if (cust.Role == "admin")
            {
                cust.Role = "user";
                db.Customer.Update(cust);
                await db.SaveChangesAsync();
                ViewBag.Str = "Role successfully edited to user";
                return View();
            }
            else if (cust.Role == "user")
            {
                cust.Role = "admin";
                db.Customer.Update(cust);
                await db.SaveChangesAsync();
                ViewBag.Str = "Role successfully edited to admin";
                return View();
            }

            return BadRequest();
        }
    }
}