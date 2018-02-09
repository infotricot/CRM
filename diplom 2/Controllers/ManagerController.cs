using diplom_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace diplom_2.Controllers
{
    [Authorize]
    public class ManagerController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Manager
        public ActionResult Index()
        {
            //var MainManager = db.Users.Find("241dc87d-b0fc-4c89-a529-cd1aa0905472");
            //var SubManager = db.Users.Find("6a7beb0e-382c-4bbd-bc1d-84ba9cf181d0");
            //MainManager.SubManagers.Add(SubManager);
            //db.SaveChanges();
            return View(ApplicationUser.GetUsersInRole("Manager").Where(a=>a.Deleted==false));
        }
    }
}