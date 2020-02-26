using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers
{
    [Route("weddings")]
    public class WeddingController : Controller
    {
         private HomeContext dbContext;
        public WeddingController(HomeContext context)
        {
            dbContext =  context;
        }

        [HttpGet("")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                User userInDb = dbContext.Users.Include(u => u.PlannedWeddings ).FirstOrDefault( u =>  u.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userInDb;

                List<Wedding> AllWeddings = dbContext.Weddings.Include(w => w.GuestList ).ThenInclude(r => r.Guest).ToList();
                return View(AllWeddings);
            }
        }

        [HttpGet("new")]
        public IActionResult New()
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                User userInDb = dbContext.Users.Include(u => u.PlannedWeddings ).FirstOrDefault( u =>  u.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userInDb;


                return View();
            }

        }

        [HttpPost("create")]
        public IActionResult Create(Wedding plan)
        {
             if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                if(ModelState.IsValid)
                {
                    dbContext.Weddings.Add(plan);
                    dbContext.SaveChanges();
                    return Redirect($"show/{plan.WeddingId}");
                }
                else
                {
                    User userInDb = dbContext.Users.Include(u => u.PlannedWeddings ).FirstOrDefault( u =>  u.Email == HttpContext.Session.GetString("UserEmail"));
                    ViewBag.User = userInDb;
                    return View("New");
                }
            }

        }

        [HttpGet("show/{weddingId}")]
        public IActionResult Show(int weddingId)
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                User userInDb = dbContext.Users.Include(u => u.PlannedWeddings ).FirstOrDefault( u =>  u.Email == HttpContext.Session.GetString("UserEmail"));
                ViewBag.User = userInDb;
                Wedding flapjack = dbContext.Weddings.Include(w => w.GuestList).ThenInclude(r => r.Guest).Include(w => w.Planner).FirstOrDefault(w => w.WeddingId == weddingId);

                return View(flapjack);
            }

        }

        [HttpGet("destroy/{weddingId}")]
        public IActionResult Destroy(int weddingId)
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else
            {
                Wedding remove = dbContext.Weddings.FirstOrDefault(w => w.WeddingId == weddingId);
                dbContext.Weddings.Remove(remove);
                dbContext.SaveChanges();
                return RedirectToAction("Dashboard");
            }

        }

        [HttpGet("rsvp/{weddingId}/{userId}/{status}")]
        public IActionResult RSVP(int weddingId, int userId, string status)
        {
            if(HttpContext.Session.GetString("UserEmail") == null)
            {
                return RedirectToAction("LogOut", "Home");
            }
            else{

            
                if(status == "add")
                {
                    Rsvp response = new Rsvp();
                    response.UserId = userId;
                    response.WeddingId = weddingId;
                    dbContext.Rsvps.Add(response);
                    dbContext.SaveChanges();

                }
                if(status == "remove")
                {
                    Rsvp remove = dbContext.Rsvps.FirstOrDefault(r => r.WeddingId == weddingId && r.UserId==userId);
                    dbContext.Rsvps.Remove(remove);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("Dashboard");
            }

        }

        
    }
}