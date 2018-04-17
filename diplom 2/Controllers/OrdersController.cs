using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using diplom_2.Models;
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;

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
        public PartialViewResult Create(int Id)
        {
            var Counterparty = db.Counterparties.Find(Id);
            ViewBag.CounterpartyId = Counterparty.Id;
            ViewBag.Counterparty = Counterparty;
            ViewBag.FirstContact = Counterparty.Contacts.FirstOrDefault();
            ViewBag.Counterparty_Id = new SelectList(db.Counterparties, "Id", "Name");
            ViewBag.StatusOrder_Id = new SelectList(db.StatusOrders, "Id", "Name");
            return PartialView();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<string> Create([Bind(Include = "Id,InvoiceUrl,CreateDate,ChangeDate,ReadyDate,Counterparty_Id,StatusOrder_Id,Comments, amount, MatColor, Size, ProductId, ProductName")] Order order, 
            string[] amount, 
            string[] MatColor, 
            string[] Size, 
            string[] ProductId, 
            string[] ProductName)
        {

            if(amount.Length==0)
            {
                ModelState.AddModelError("Products", "В заявке должен быть хотя бы один товар");
            }
            else            
                foreach (var item in amount)
                {
                    int temp;
                    if (int.TryParse(item, out temp) == false)
                    {
                        ModelState.AddModelError("Products", "Количество должно быть целым числом");
                        break;
                    }
                }            
            if (ModelState.IsValid)
            {
                order.Products = new List<ProductInOrder>();
                for (int i = 0; i < amount.Length; i++)
                {
                    ProductInOrder product = new ProductInOrder();
                    product.Color = MatColor[i];
                    product.Name = ProductName[i];
                    long temp;
                    if (long.TryParse(ProductId[i], out temp) == true)
                    {
                        product.ProductId = temp;
                    }
                    else
                    {
                        product.ProductId = -1;
                    }
                    product.Quantity = Convert.ToInt32(amount[i]);
                    product.Size = Size[i];
                    order.Products.Add(product);
                      
                }


                order.ChangeDate = DateTime.Now;
                order.CreateDate = DateTime.Now;
                order.Counterparty_Id = order.Counterparty_Id;
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var currentUser = await UserManager.FindByNameAsync(User.Identity.Name);
                order.Manager_Id = currentUser.Id;
                order.StatusOrder_Id = 1;
                db.Orders.Add(order);
                db.SaveChanges();
                return order.Counterparty_Id.ToString();
            }

            ViewBag.Counterparty_Id = new SelectList(db.Counterparties, "Id", "Name", order.Counterparty_Id);
            ViewBag.StatusOrder_Id = new SelectList(db.StatusOrders, "Id", "Name", order.StatusOrder_Id);
            var Counterparty = db.Counterparties.Find(order.Counterparty_Id);
            ViewBag.CounterpartyId = Counterparty.Id;
            ViewBag.Counterparty = Counterparty;
            ViewBag.FirstContact = Counterparty.Contacts.FirstOrDefault();          
            return Counterparty.Id.ToString();
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
            var Counterparty = order.Counterparty;
            ViewBag.CounterpartyId = Counterparty.Id;
            ViewBag.Counterparty = Counterparty;
            ViewBag.FirstContact = Counterparty.Contacts.FirstOrDefault();
            ViewBag.Counterparty_Id = new SelectList(db.Counterparties, "Id", "Name", order.Counterparty_Id);
            ViewBag.StatusOrder_Id = new SelectList(db.StatusOrders, "Id", "Name", order.StatusOrder_Id);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,InvoiceUrl,CreateDate,ChangeDate,ReadyDate,Counterparty_Id,StatusOrder_Id,Comments, amount, MatColor, Size, ProductId, ProductName")] Order order,
            string[] amount,
            string[] MatColor,
            string[] Size,
            string[] ProductId,
            string[] ProductName)
        {
                              
            if(amount.Length==0)
            {
                ModelState.AddModelError("Products", "В заявке должен быть хотя бы один товар");
            }
            else            
                foreach (var item in amount)
                {
                    int temp;
                    if (int.TryParse(item, out temp) == false)
                    {
                        ModelState.AddModelError("Products", "Количество должно быть целым числом");
                        break;
                    }
                }            
            if (ModelState.IsValid)
            {
                order.Products = new List<ProductInOrder>();

                Order orderFromDb = db.Orders.Find(order.Id);
                orderFromDb.Products.Clear();
                
                for (int i = 0; i < amount.Length; i++)
                {
                    ProductInOrder product = new ProductInOrder();
                    
                    product.Color = MatColor[i];
                    product.Name = ProductName[i];
                    long temp;
                    if (long.TryParse(ProductId[i], out temp) == true)
                    {
                        product.ProductId = temp;
                    }
                    else
                    {
                        product.ProductId = -1;
                    }
                    product.Quantity = Convert.ToInt32(amount[i]);
                    product.Size = Size[i];
                    orderFromDb.Products.Add(product);
                }


                orderFromDb.ChangeDate = DateTime.Now;
                orderFromDb.Comments = order.Comments;
                orderFromDb.CreateDate = order.CreateDate;
                orderFromDb.StatusOrder_Id = 2;
                orderFromDb.ReadyDate = order.ReadyDate;
                db.SaveChanges();
                
                return RedirectToAction("Index");
            }

            ViewBag.Counterparty_Id = new SelectList(db.Counterparties, "Id", "Name", order.Counterparty_Id);
            ViewBag.StatusOrder_Id = new SelectList(db.StatusOrders, "Id", "Name", order.StatusOrder_Id);
            var Counterparty = db.Counterparties.Find(order.Counterparty_Id);
            ViewBag.CounterpartyId = Counterparty.Id;
            ViewBag.Counterparty = Counterparty;
            ViewBag.FirstContact = Counterparty.Contacts.FirstOrDefault();
            order.Products = new List<ProductInOrder>();
            for (int i = 0; i < amount.Length; i++)
            {
                ProductInOrder product = new ProductInOrder();

                product.Color = MatColor[i];
                product.Name = ProductName[i];
                long temp;
                if (long.TryParse(ProductId[i], out temp) == true)
                {
                    product.ProductId = temp;
                }
                else
                {
                    product.ProductId = -1;
                }
                product.Quantity = Convert.ToInt32(amount[i]);
                product.Size = Size[i];
                order.Products.Add(product);
            }


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
