using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diplom_2.Controllers
{
    [Authorize]
    public class DirectorController : Controller
    {
        // GET: Director
        public ActionResult Reports()
        {
            return View();
        }
    }
}