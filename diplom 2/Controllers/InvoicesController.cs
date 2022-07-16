using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using diplom_2.Models;
using OfficeOpenXml;
using System.IO;
using ExcelDataReader;

namespace diplom_2.Controllers
{
    public class InvoicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invoices
        public ActionResult Index()
        {
            return View(db.Invoices.ToList());
        }

        public PartialViewResult InvoiceRowPartial(Invoice invoce)
        {

            return PartialView(invoce);
        }

        [HttpPost]
        public PartialViewResult UploadExcelFile(HttpPostedFileBase upload, int Id)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                // ExcelDataReader works with the binary Excel file, so it needs a FileStream
                // to get started. This is how we avoid dependencies on ACE or Interop:
                Stream stream = upload.InputStream;
                // We return the interface, so that
                IExcelDataReader reader = null;
                string filename = Guid.NewGuid().ToString() + upload.FileName;
                var path = Path.Combine(Server.MapPath("~/InvoiceFiles/"),
                                      System.IO.Path.GetFileName(filename));

                upload.SaveAs(path);

                if (upload.FileName.EndsWith(".xls"))
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (upload.FileName.EndsWith(".xlsx"))
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                else
                {
                    ModelState.AddModelError("File", "This file format is not supported");
                   
                }
                try
                {
                    //Пропускаем первую строку в накладной
                    reader.Read();
                    //Читаем строку с заголовком
                    reader.Read();
                    //Читаем заголовок в первой ячейке                   
                    string Title = reader.GetValue(1).ToString();
                    //Создаём новый объект класса накладной
                    Invoice invoice = new Invoice();
                    //Находим заявку, к которой будет принадлежать накладная
                    invoice.Order = db.Orders.Find(Id);

                    //Разделяем заголовок накладной на отдельные слова
                    string[] wordsInTitle = Title.Split(new char[] { ' ' });
                    //Получаем номер накладной из 3-го слова
                    invoice.Number = Convert.ToInt32(wordsInTitle[3]);

                    //Получаем день из 5-го слова
                    int day = Convert.ToInt32(wordsInTitle[5]);
                    //Получаем месяц из 6-го слова и преобразуем в номер месяца
                    int month = -1;
                    switch (wordsInTitle[6])
                    {
                        case "января": month = 1; break;
                        case "февраля": month = 2; break;
                        case "марта": month = 3; break;
                        case "апреля": month = 4; break;
                        case "мая": month = 5; break;
                        case "июня": month = 6; break;
                        case "июля": month = 7; break;
                        case "августа": month = 8; break;
                        case "сентября": month = 9; break;
                        case "октября": month = 10; break;
                        case "ноября": month = 11; break;
                        case "декабря": month = 12; break;
                        default:
                            new Exception("Не определён месяц, указанный в накладной");
                            break;
                    }


                    //Получаем год из 7-го слова
                    int year = Convert.ToInt32(wordsInTitle[7]);
                    //Создаём общую дату накладной
                    invoice.Date = new DateTime(year, month, day);
                    invoice.FileName = filename;                    

                    List<InvoiceItem> InvoiceItemes = new List<InvoiceItem>();
                    //Считываем и пропускаем 10 строк до начала списка товаров
                    int skipRow = 11;
                    while (skipRow!=0)
                    {
                        reader.Read();
                        skipRow--;
                    }
                    reader.Read();
                    //Считываем номер первого товара
                    var currentNumber = reader.GetValue(1);
                    //пока есть номера товаров в первой ячейке, продолжаем их считывание и создание
                    while (currentNumber != null)
                    {
                        //Создаём новый товар накладной
                        object articulObj = reader.GetValue(3);
                        string articul = "";
                        if (articulObj != null)
                            articul = articulObj.ToString();
                        InvoiceItem item = new InvoiceItem()
                        {
                            Articul = articul,
                            Product = reader.GetValue(6).ToString(),
                            Amount = Convert.ToInt32(reader.GetValue(26)),
                            Price = Convert.ToDecimal(reader.GetValue(31)),
                            Invoice = invoice
                        };



                        InvoiceItemes.Add(item);
                        db.InvoiceItems.Add(item);
                        reader.Read();
                        currentNumber = reader.GetValue(1);
                    }
                    db.Invoices.Add(invoice);
                    db.SaveChanges();
                    return PartialView("InvoiceRowPartial", invoice);
                }
                catch (Exception ex)
                {
                    return PartialView("InvoiceRowPartial", null);
                }
            }
            return PartialView("InvoiceRowPartial", null);
        }

        public void UpdatePayment(int id, decimal value, int PaymentNumber)
        {
            //var invoice = db.Invoices.Find(id);
            //invoice.Payed = value;
            //db.SaveChanges();
        }

        // GET: Invoices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Number,Date")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        // GET: Invoices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Number,Date")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public JsonResult Delete(string filename)
        {           
            string fullPath = Request.MapPath("~/InvoiceFiles/" + filename);
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                var invoice = db.Invoices.Where(a => a.FileName == filename).FirstOrDefault();
                invoice.InvoiceItems.Clear();
                db.Invoices.Remove(invoice);
                db.SaveChanges();
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
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
