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

namespace Car_Dealership.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("[controller]")]
    public class AutoController : Controller
    {
        private CarDealershipContext _carDealershipContext;

        public AutoController(CarDealershipContext c)
        {
            _carDealershipContext = c;
        }

        [Microsoft.AspNetCore.Authorization.Authorize]
        public async Task<IActionResult> Index(string searchString, string sortOrder)
        {
            if (User.Identity.IsAuthenticated)
            {
                var auto = from m in _carDealershipContext.Auto
                    select m;
                if (!String.IsNullOrEmpty(searchString))
                {
                    auto = _carDealershipContext.Auto.Where(s =>
                        s.Brand.Contains(searchString) || s.Year.Equals(Int32.Parse(searchString)) ||
                        s.Price.Equals(Double.Parse(searchString)) || s.Color.Contains(searchString) ||
                        s.Capacity.Equals(Int32.Parse(searchString)));
                }

                switch (sortOrder)
                {
                    case "Brand":
                        auto = auto.OrderBy(s => s.Brand);
                        break;
                    case "Year":
                        auto = auto.OrderBy(s => s.Year);
                        break;
                    case "Price":
                        auto = auto.OrderBy(s => s.Price);
                        break;
                    case "Color":
                        auto = auto.OrderBy(s => s.Color);
                        break;
                    case "Capacity":
                        auto = auto.OrderBy(s => s.Capacity);
                        break;
                    default:
                        auto = auto.OrderBy(s => s.Id);
                        break;
                }

                return View(await auto.ToListAsync());
            }
            else return Content("Not authenticated!");
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "admin")]
        [Microsoft.AspNetCore.Mvc.HttpGet("/add")]
        public IActionResult PostAuto()
        {
            return View();
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "admin")]
        [Microsoft.AspNetCore.Mvc.HttpPost("/add"), Microsoft.AspNetCore.Mvc.ActionName("PostAuto")]
        public async Task<IActionResult> PostAuto([Bind("Brand, Year, Price, Color, Capacity")]
            Auto a)
        {
            if (User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value == "admin")
            {
                if (ModelState.IsValid)
                {
                    _carDealershipContext.Auto.Add(a);
                    await _carDealershipContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(a);
            }
            else
            {
                return Content("Not admin!");
            }
        }

        [Authorize(Roles = "admin")]
        [Microsoft.AspNetCore.Mvc.HttpGet("/edit")]
        public IActionResult PutAuto(int id)
        {
            Auto a = _carDealershipContext.Auto.Find(id);
            if (a == null)
            {
                return NotFound();
            }
            else
            {
                return View(a);
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "admin")]
        [Microsoft.AspNetCore.Mvc.HttpPost("/edit")]
        public async Task<IActionResult> PutAuto(int id, [Bind("Id, Brand, Year, Price, Color, Capacity")]
            Auto a)
        {
            if (User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value == "admin")
            {
                if (id != a.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    _carDealershipContext.Auto.Update(a);
                    await _carDealershipContext.SaveChangesAsync();
                    return RedirectToAction("Index");
                }

                return View(a);
            }
            else
            {
                return Content("Not admin!");
            }
        }

        [Microsoft.AspNetCore.Authorization.Authorize(Roles = "admin")]
        [Microsoft.AspNetCore.Mvc.HttpGet("/delete/{id}")]
        public ActionResult<Auto> DeleteAuto(int id)
        {
            if (User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value == "admin")
            {
                var aut = _carDealershipContext.Auto.Find(id);
                if (aut == null)
                {
                    return NotFound();
                }

                _carDealershipContext.Auto.Remove(aut);
                _carDealershipContext.SaveChanges();
                ViewBag.Str = "Car with id = " + aut.Id + " was deleted!";
                return View();
            }
            else
            {
                return Content("Not admin!");
            }
        }
    }
}