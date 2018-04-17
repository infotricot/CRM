using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using diplom_2.Models;

namespace diplom_2.Controllers
{
    [Authorize]
    public class ContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Contacts
        public ActionResult Index()
        {
            return View(db.Contacts.ToList());
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        //public PartialViewResult Test()
        //{
        //    //
        //    return PartialView();
        //}

        public ActionResult CreateById(int id)
        {          
            return RedirectToAction("Create", db.Counterparties.Find(id));
        }


        // GET: Contacts/Create
        public ActionResult Create(Counterparty counterparty)
        {
            ViewBag.Counterparty = counterparty;
            return View();
        }

        // POST: Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FIO,Phones,Email,Skype,Viber,Facebook,Notes,Deleted,Position")] Contact contact, int Counterparty_Id, string submitButton)
        {
            if (submitButton == "Отменить (добавить контакт позже)")
            {
                return RedirectToAction("Index", "Counterparties");
            }
                
            if (ModelState.IsValid)
            {
                contact.Counterparty = db.Counterparties.Find(Counterparty_Id);
                db.Contacts.Add(contact);
                db.SaveChanges();
                if (submitButton == "Сохранить и закрыть")
                    return RedirectToAction("Index", "Counterparties");
                if (submitButton == "Сохранить и добавить ещё один контакт")
                {
                    //ViewBag.Counterparty = contact.Counterparty;
                    //Contact empty = new Contact();
                    //return View(empty);
                    return RedirectToAction("Create", contact.Counterparty);
                }
            }

            return View(contact);
        }



        // GET: Contacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FIO,Phones,Email,Skype,Viber,Facebook,Notes,Deleted,Position")] Contact contact, string submitButton)
        {
            if (submitButton == "Отменить")
            {
                return RedirectToAction("Index", "Counterparties");
            }
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Counterparties");
            }
            return View(contact);
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            contact.Deleted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Counterparties");
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
