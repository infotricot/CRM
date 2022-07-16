using diplom_2.Models;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diplom_2.Controllers
{
    class InvoiceInfoModel
    {
       
    }

    public class MyReportModel
    {
        public ApplicationUser Manager { get; set; }
        public string MonthString { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TaskComplete { get; set; }
        public int TaskCanceled { get; set; }
        public int TaskInProgress { get; set; }
        public int ClientInWork { get; set; }
        public int AmountMeetings { get; set; }
        public int AmountCreatedOrders { get; set; }
        public int NewClients { get; set; }
        public int NewInvoices { get; set; }
        public decimal InvoicesPayment { get; set; }
        public int AmountInvoicesPayment { get; set; }

        public List<Invoice> Invoices { get; set; }

    }



    public class ReportsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reports
        [Authorize]
        public ActionResult MyReport(int month, int year)
        {
            //Создаём объект модели данных для оформления отчёта
            MyReportModel report = new MyReportModel();

            switch (month)
            {
                case 1: report.MonthString = "январь"; break;
                case 2: report.MonthString = "февраль"; break;
                case 3: report.MonthString = "март"; break;
                case 4: report.MonthString = "апрель"; break;
                case 5: report.MonthString = "май"; break;
                case 6: report.MonthString = "июнь"; break;
                case 7: report.MonthString = "июль"; break;
                case 8: report.MonthString = "август"; break;
                case 9: report.MonthString = "сентябрь"; break;
                case 10: report.MonthString = "октябрь"; break;
                case 11: report.MonthString = "ноябрь"; break;
                case 12: report.MonthString = "декабрь"; break;              
                default:
                    new Exception("Не определён месяц, указанный в отчёте");
                    break;
            }

            //Есть текущий пользователь является удалённым менеджером
            if (User.IsInRole("remove manager"))
                //Тогда отчёт строится только для него
                report.Manager = db.Users.Where(a => a.UserName == User.Identity.Name).First();

            //Устанавливаем месяц и год отчёта
            report.Month = month;
            report.Year = year;

            //Получаем информацию о задачах 
            report.TaskComplete = db.Proceses.Count(a =>
                //получаем задачи только того менеджера для которого строится отчёт
                a.Manager.Id == report.Manager.Id &&
                //получаем только выполненные задачи
                a.IsExecuted == true &&
                //получаем только те задачи, которые были выполнены в указанном году и месяце
                a.ExecuteDate.Value.Year == year && a.ExecuteDate.Value.Month == month 
                );

            //Получаем информацию об отменённых задачах
            report.TaskCanceled = db.Proceses.Count(a =>
                //получаем задачи только того менеджера для которого строится отчёт
                a.Manager.Id == report.Manager.Id &&
                //получаем только не выполненные задачи
                a.IsExecuted == false &&
                //получаем только те задачи, которые были выполнены в указанном году и месяце
                a.ExecuteDate.Value.Year == year && a.ExecuteDate.Value.Month == month
                );

            //Получаем информацию о не выполненных задачах
            report.TaskInProgress = db.Proceses.Count(a =>
                //получаем задачи только того менеджера для которого строится отчёт
                a.Manager.Id == report.Manager.Id &&
                //получаем только не выполненные задачи
                a.IsExecuted == null
                );

            //Получаем список уникальных идентификаторов клиентов связанных с задачами
            var ProcessClientInWork = db.Proceses.Where(a =>
                //получаем задачи только того менеджера для которого строится отчёт
                a.Manager.Id == report.Manager.Id &&
               //получаем только выполненные задачи
               a.IsExecuted == true &&
               //получаем только те задачи, которые были выполнены в указанном году и месяце
               a.ExecuteDate.Value.Year == year && a.ExecuteDate.Value.Month == month &&
               //получаем только те задачи, которые связаны с клиентами из базы
               a.Counterparty != null
               //Получаем только те задачи, которые относятся к уникальным клиентам и получаем их количество
           ).GroupBy(a=>a.Counterparty.Id).Select(a=>a.Key).ToList();

            //Получаем список уникальных идентификаторов клиентов связанных с заявками
            var OrderClientInWork = db.Orders.Where(a =>
              //получаем заявки только того менеджера для которого строится отчёт
              a.Manager.Id == report.Manager.Id &&
             
             //получаем только те задачи, которые были изменены в указанном году и месяце
             a.ChangeDate.Year == year && a.ChangeDate.Month == month
         //Получаем только те задачи, которые относятся к уникальным клиентам и получаем их количество
         ).GroupBy(a => a.Counterparty.Id).Select(a=>a.Key).ToList();


            //Создаём и добавляем идентификаторы клиентов из заявок и задач в общий список
            List<int> SumCounterparty = new List<int>();
            SumCounterparty.AddRange(ProcessClientInWork);
            SumCounterparty.AddRange(OrderClientInWork);

            //Исключить дубликаты клиентов, с которыми могло быть взаимодействие в заявках и задачах
            //и получить кол-во уникальных клиентов
            report.ClientInWork = SumCounterparty.GroupBy(a => a).Count();

            report.AmountMeetings = db.Proceses.Count(a =>
                //получаем задачи только того менеджера для которого строится отчёт
                a.Manager.Id == report.Manager.Id &&
                //получаем только выполненные задачи
                a.IsExecuted == true &&
                //получаем только те задачи, которые были выполнены в указанном году и месяце
                a.ExecuteDate.Value.Year == year && a.ExecuteDate.Value.Month == month &&
                //получаем только задачи-встречи
                a.ProcessType.Id == 4
                );

            report.AmountCreatedOrders = db.Orders.Count(a=>
                 a.Manager.Id == report.Manager.Id &&
                //получаем только те задачи, которые были изменены в указанном году и месяце
                 a.CreateDate.Year == year && a.CreateDate.Month == month
                );

            report.NewClients = db.Counterparties.ToList().Count(a =>
                a.Managers.Contains(report.Manager) &&
                //получаем только те задачи, которые были изменены в указанном году и месяце
                a.Created.Year == year && a.Created.Month == month
               );

            var TempInvoices = db.Invoices.ToList();

            report.Invoices = TempInvoices.Where(a =>
                a.Order.Manager_Id == report.Manager.Id &&
                //получаем только те задачи, которые были изменены в указанном году и месяце
                a.Date.Year == year && a.Date.Month == month 
             ).ToList();

            //foreach (var i in report.Invoices)
            //{
            //    if (i.Payed != 0)
            //    {
            //        report.InvoicesPayment += i.Payed;
            //        report.AmountInvoicesPayment++;
            //    }
            //}
     


            return View(report);
        }
    }
}