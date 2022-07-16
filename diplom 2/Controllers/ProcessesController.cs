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

        public ActionResult Complete(int id, bool Complete)
        {
            var process = db.Proceses.Find(id);
            process.ExecuteDate = DateTime.Now;
            process.IsExecuted = Complete;


            return View(db.Proceses.Find(id));
        }


        public PartialViewResult ShowProcesses(int? id, bool? showTaskToday, bool? showTaskReady, bool? showTaskDiscard, bool? showTaskInWork, int? typeSort, string SelectManagerProcess)
        {
            var currentManager = db.Users.Find(User.Identity.GetUserId());
            ViewBag.currentManager = currentManager;
            ViewBag.CounterpartyId = id;
            List<ProcessOrderBase> ProcessesList = new List<ProcessOrderBase>();
            
            List<ProcessOrderBase> ResultProcessesList = new List<ProcessOrderBase>();
            if (id == null)
            {
                //Если нужно показать задачи подчинённых
                if (SelectManagerProcess == "MyManagerProcesses")
                {
                    //Если текущий пользователь вышестоящий менеджер показываем задачи его подчинённых
                    if (User.IsInRole("manager"))
                        ProcessesList = db.Proceses.Where(a => a.Manager.ParentManager.Id == currentManager.Id).ToList<ProcessOrderBase>();
                    //Если текущий пользователь администратор, то показываем задачи всех менеджеров
                    if (User.IsInRole("admin"))
                        ProcessesList = db.Proceses.Where(a => a.Manager.Roles.FirstOrDefault().RoleId == "1" || a.Manager.Roles.FirstOrDefault().RoleId == "5").ToList<ProcessOrderBase>();
                    //Если текущий пользователь директор, то показываем задачи всех сотрудников (кроме своих собственных)
                    if (User.IsInRole("director"))
                        ProcessesList = db.Proceses.Where(a => a.Manager.Id != currentManager.Id).ToList<ProcessOrderBase>();
                }
                else
                    //Если нужно показать задачи выбранного менеджера
                    if (SelectManagerProcess != "undefined" && SelectManagerProcess != "MyProcesses")
                    {
                        ProcessesList = db.Proceses.Where(a => a.Manager.Id == SelectManagerProcess).ToList<ProcessOrderBase>();
                    }
                    else
                    {
                        //Если нужно показать задачи текущего пользователя
                        ProcessesList = db.Proceses.Where(a => a.Manager.Id == currentManager.Id).ToList<ProcessOrderBase>();
                    }
            }
            else
            {
                ProcessesList = db.Counterparties.Find(id).Proceses.Where(a => a.Manager.Id == currentManager.Id).ToList<ProcessOrderBase>();
                var OrderList = db.Counterparties.Find(id).Orders;
                if (OrderList.Count()!=0)
                    ProcessesList.AddRange(OrderList.Where(a => a.Manager!=null && a.Manager.Id == currentManager.Id).ToList<ProcessOrderBase>());
            }

            if(showTaskToday == null)
            {
                return PartialView(ProcessesList);
            }

            if (showTaskToday.Value)
                ResultProcessesList.AddRange(ProcessesList.Where(a => (a as Process).PlaningDate <= DateTime.Now && a.IsExecuted == null).ToList());            
            if (showTaskReady.Value)
                ResultProcessesList.AddRange(ProcessesList.Where(a => a.IsExecuted != null && a.IsExecuted.Value==true).ToList());
            if (showTaskDiscard.Value)
                ResultProcessesList.AddRange(ProcessesList.Where(a => a.IsExecuted != null && a.IsExecuted.Value == false).ToList());
            if (showTaskInWork.Value)
                ResultProcessesList.AddRange(ProcessesList.Where(a => (a as Process).PlaningDate > DateTime.Now && a.IsExecuted == null).ToList());

        
          

            if (typeSort == 0)
            {
                ResultProcessesList = ResultProcessesList.OrderByDescending(a => a.CreateDate).ToList();
            } else
                //if (typeSort == 1)
            {
                ResultProcessesList = ResultProcessesList.OrderByDescending(a => (a as Process).PlaningDate).ToList();
            }


            return PartialView(ResultProcessesList);
        }


        [HttpPost]
        public string Complete([Bind(Include = "Id,CreateDate,ExecuteDate,PlaningDate,Description,ExecuteDescription, IsExecuted")] Process process)
        {
            Process editProcess = db.Proceses.Find(process.Id);
            editProcess.ExecuteDescription = process.ExecuteDescription;
            editProcess.ExecuteDate = process.ExecuteDate;
            editProcess.IsExecuted = process.IsExecuted;
            db.Entry(editProcess).State = EntityState.Modified;
            db.SaveChanges();
            if (editProcess.Counterparty== null)
                return "";
            else
                return editProcess.Counterparty.Id.ToString();
           
        }
   

        // GET: Processes/Create
        public ActionResult Create(int ProcessTypeId, int? CounterpatyId)
        {
            ViewBag.ProcessType = db.ProcessTypes.Find(ProcessTypeId);
            ViewBag.CounterpatyId = CounterpatyId;
        

            return View();
        }

        // POST: Processes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
    
        public string Create([Bind(Include = "Id,CreateDate,ExecuteDate,PlaningDate,Description,Manager_Id")] Process process, int? ProcessTypeId, int? CounterpatyId)
        {
            process.Counterparty = db.Counterparties.Find(CounterpatyId);
            process.ProcessType = db.ProcessTypes.Find(ProcessTypeId);
            process.CreateDate = DateTime.Now;
            
            process.CreatedBy = db.Users.Find(User.Identity.GetUserId());
            //Если не указано какому менеджеру поставлена задача, значит её ставил менджер сам себе
            if (string.IsNullOrEmpty(process.Manager_Id) || process.Manager_Id == "MyProcesses")
                process.Manager = db.Users.Find(User.Identity.GetUserId());
            //иначе, если есть id-менеджера, значит задачу поставил вышестоящий менеджер подченённому менеджеру с указанным Id
            else
                process.Manager = db.Users.Find(process.Manager_Id);

            if (ModelState.IsValid)
            {
                if (process.ExecuteDate != null)
                    process.IsExecuted = true;

                db.Proceses.Add(process);
                db.SaveChanges();
                if (CounterpatyId == null)
                    return "";
                else
                    return process.Counterparty.Id.ToString();
            }
            ViewBag.ProcessType = db.ProcessTypes.Find(ProcessTypeId);
            ViewBag.CounterpatyId = CounterpatyId;
            if (CounterpatyId == null)
                return "";
            else
                return process.Counterparty.Id.ToString();
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
