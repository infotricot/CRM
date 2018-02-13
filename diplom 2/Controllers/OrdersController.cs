﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using diplom_2.Models;
using System.IO;

namespace diplom_2.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Counterparty).Include(o => o.StatusOrder);
            return View(orders.ToList());
        }


        public ActionResult UploadImage()
        {
            //Request - вся информация о запросе, пришедшая от клиента (форма, файлы, служебная информация и т.д.)
            //Request.Files - даёт доступ к файлам, присланным от клиента
            //Проверяем, есть ли файлы
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];
                //Узнать по какому физическому пути (physical path) находится нужная нам папка, в которую будет сохраняться файл:
                string path = Server.MapPath("~/OrdersImages/");
                //string pathfile = path + "\\" + file.FileName;                
                string filename = Guid.NewGuid() + file.FileName;
                //Комбинируем путь к папке и имя файла, чтобы получить путь к файлу.
                string pathname = Path.Combine(path, filename); 
                //Сохраняем файл по полученному пути
                file.SaveAs(pathname);
                return Json(filename);
            }
            return Json("error");
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create(int Id)
        {
            var Counterparty = db.Counterparties.Find(Id);
            ViewBag.CounterpartyId = Counterparty.Id;
            ViewBag.Counterparty = Counterparty;
            ViewBag.FirstContact = Counterparty.Contacts.FirstOrDefault();
            ViewBag.Counterparty_Id = new SelectList(db.Counterparties, "Id", "Name");
            ViewBag.StatusOrder_Id = new SelectList(db.StatusOrders, "Id", "Name");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,InvoiceUrl,CreatedDate,ChangeDate,ReadyDate,Counterparty_Id,StatusOrder_Id")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Counterparty_Id = new SelectList(db.Counterparties, "Id", "Name", order.Counterparty_Id);
            ViewBag.StatusOrder_Id = new SelectList(db.StatusOrders, "Id", "Name", order.StatusOrder_Id);
            return View(order);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.Counterparty_Id = new SelectList(db.Counterparties, "Id", "Name", order.Counterparty_Id);
            ViewBag.StatusOrder_Id = new SelectList(db.StatusOrders, "Id", "Name", order.StatusOrder_Id);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,InvoiceUrl,CreatedDate,ChangeDate,ReadyDate,Counterparty_Id,StatusOrder_Id")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Counterparty_Id = new SelectList(db.Counterparties, "Id", "Name", order.Counterparty_Id);
            ViewBag.StatusOrder_Id = new SelectList(db.StatusOrders, "Id", "Name", order.StatusOrder_Id);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
