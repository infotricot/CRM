using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using diplom_2.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace diplom_2.Controllers
{
    [Authorize]
    public class CounterpartiesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Counterparties
        [Authorize(Roles= "admin,director,manager,remove manager")]
        public ActionResult Index()
        {
            var currentManager = db.Users.Find(User.Identity.GetUserId());
            ViewBag.currentManager = currentManager;
            if (User.IsInRole("director") || User.IsInRole("admin"))
                return View(db.Counterparties.ToList());
            else              
                return View(db.Counterparties.ToList().Where(a => a.Managers.Contains(currentManager)).ToList());            
        }

        // GET: Counterparties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counterparty counterparty = db.Counterparties.Find(id);
            if (counterparty == null)
            {
                return HttpNotFound();
            }
            return PartialView(counterparty);
        }


        // GET: Counterparties/Details/5
        public ActionResult ShowDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counterparty counterparty = db.Counterparties.Find(id);
            if (counterparty == null)
            {
                return HttpNotFound();
            }
            return PartialView(counterparty);
        }

        // POST: Counterparties/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Details([Bind(Include = "Id,Name,Description,City,Address,Requisites,Web,CounterpartyType,Created,LastModify,Region")] Counterparty counterparty)
        {
            if (ModelState.IsValid)
            {
                counterparty.LastModify = DateTime.Now;
                db.Entry(counterparty).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(counterparty);
        }



        // GET: Counterparties/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: Counterparties/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,City,Address,Requisites,Web,CounterpartyType,Created,LastModify, Region")] Counterparty counterparty)
        {
            if (ModelState.IsValid)
            {
                counterparty.CounterpartyType = CounterpartyType.Клиент;
                counterparty.Created = DateTime.Now;
                counterparty.LastModify = DateTime.Now;

                //Получаем текущего авторизированного пользователя в системе
                //ApplicationUser currentUser = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId());
                ApplicationUser currentUser = db.Users.Where(a => a.UserName == User.Identity.Name).FirstOrDefault();
                if (counterparty.Managers == null)
                    counterparty.Managers = new List<ApplicationUser>();
                counterparty.Managers.Add(currentUser);                
                db.Counterparties.Add(counterparty);
                db.SaveChanges();
                return RedirectToAction("Create", "Contacts", counterparty);
            }

            return View(counterparty);
        }

 



        // GET: Counterparties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Counterparty counterparty = db.Counterparties.Find(id);
            if (counterparty == null)
            {
                return HttpNotFound();
            }
            return View(counterparty);
        }

        // POST: Counterparties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Counterparty counterparty = db.Counterparties.Find(id);
            db.Counterparties.Remove(counterparty);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
