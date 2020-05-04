using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Car_Dealership.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Car_Dealership.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private CarDealershipContext _carDealershipContext;

        public OrderController(CarDealershipContext c)
        {
            _carDealershipContext = c;
        }

        // GET
        [Authorize(Roles = "admin")]
        public IActionResult Index()
        {
            var cont = _carDealershipContext.Order.ToList();
            foreach (Order order in cont)
            {
                order.Auto = _carDealershipContext.Auto.Find(order.AutoId);
                order.Customer = _carDealershipContext.Customer.Find(order.CustomerId);
            }

            return View(cont);
        }

        [Authorize]
        [HttpGet("/my_orders")]
        public IActionResult MyOrders()
        {
            if (User.Identity.IsAuthenticated)
            {
                var order = _carDealershipContext.Order.ToList();
                int iD = Int32.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
                List<Order> result = new List<Order>();
                foreach (Order ord in order)
                {
                    if (ord.CustomerId == iD)
                    {
                        ord.Auto = _carDealershipContext.Auto.Find(ord.AutoId);
                        ord.Customer = _carDealershipContext.Customer.Find(ord.CustomerId);
                        result.Add(ord);
                    }
                }

                return View(result);
            }
            else
            {
                return Content("Not authenticated!");
            }
        }

        [Authorize]
        [HttpGet("/buy")]
        public IActionResult Buy(int id)
        {
            if (id == null) return NotFound();
            Order a = new Order();
            a.AutoId = id;
            int iD = Int32.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value);
            a.CustomerId = iD;
            a.Auto = _carDealershipContext.Auto.Find(id);
            a.Customer = _carDealershipContext.Customer.Find(iD);
            _carDealershipContext.Order.Add(a);
            _carDealershipContext.SaveChanges();
            ViewBag.Str = "Thanks, " + a.Customer.FirstName + ", for your purchasing!";
            return View();
        }

        [Authorize]
        [HttpGet("/delete")]
        public ActionResult<Auto> Delete(int id)
        {
            if (User.FindFirst(x => x.Type == ClaimsIdentity.DefaultRoleClaimType).Value == "admin" ||
                Int32.Parse(User.FindFirst(x => x.Type == ClaimsIdentity.DefaultNameClaimType).Value) ==
                _carDealershipContext.Order.Find(id).CustomerId)
            {
                var ord = _carDealershipContext.Order.Find(id);
                if (ord == null)
                {
                    return NotFound();
                }

                _carDealershipContext.Order.Remove(ord);
                _carDealershipContext.SaveChanges();
                ViewBag.Str = "Order with id = " + ord.Id + " was deleted!";
                return View();
            }
            else
            {
                return Content("Not admin or owner!");
            }
        }
    }
}