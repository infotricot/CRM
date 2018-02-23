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

namespace diplom_2.Controllers
{
    public class ProcessesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Processes
        public ActionResult Index()
        {
            return View(db.Proceses.ToList());
        }

        // GET: Processes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Process process = db.Proceses.Find(id);
            if (process == null)
            {
                return HttpNotFound();
            }
            return View(process);
        }

        public ActionResult Complete(int id)
        {           
            return View(db.Proceses.Find(id));
        }

        [HttpPost]
        public ActionResult Complete([Bind(Include = "Id,CreateDate,ExecuteDate,PlaningDate,Description")] Process process)
        {
            Process editProcess = db.Proceses.Find(process.Id);
            editProcess.ExecuteDescription = process.ExecuteDescription;
            editProcess.ExecuteDate = process.ExecuteDate;
            db.Entry(editProcess).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Processes/Create
        public ActionResult Create(int ProcessTypeId, int CounterpatyId)
        {
            ViewBag.ProcessType = db.ProcessTypes.Find(ProcessTypeId);
            ViewBag.CounterpatyId = CounterpatyId;

            return View();
        }

        // POST: Processes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
    
        public ActionResult Create([Bind(Include = "Id,CreateDate,ExecuteDate,PlaningDate,Description")] Process process, int ProcessTypeId, int CounterpatyId)
        {
            process.Counterparty = db.Counterparties.Find(CounterpatyId);
            process.ProcessType = db.ProcessTypes.Find(ProcessTypeId);
            process.CreateDate = DateTime.Now;
            process.Manager = db.Users.Find(User.Identity.GetUserId());
            process.CreatedBy = db.Users.Find(User.Identity.GetUserId());

            if (ModelState.IsValid)
            {
                db.Proceses.Add(process);
                db.SaveChanges();
                return RedirectToAction("Index", "Counterparties");
            }
            ViewBag.ProcessType = db.ProcessTypes.Find(ProcessTypeId);
            ViewBag.CounterpatyId = CounterpatyId;
            return View(process);
        }

        // GET: Processes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Process process = db.Proceses.Find(id);
            if (process == null)
            {
                return HttpNotFound();
            }
            return View(process);
        }

        // POST: Processes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CreateDate,ExecuteDate,PlaningDate,Description")] Process process)
        {
            if (ModelState.IsValid)
            {
                db.Entry(process).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(process);
        }

        // GET: Processes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Process process = db.Proceses.Find(id);
            if (process == null)
            {
                return HttpNotFound();
            }
            return View(process);
        }

        // POST: Processes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Process process = db.Proceses.Find(id);
            db.Proceses.Remove(process);
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
