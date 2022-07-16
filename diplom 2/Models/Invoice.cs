using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom_2.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public DateTime Date { get; set; }
        public string FileName { get; set; }
        public virtual Order Order { get; set; }        
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; }
        public virtual ICollection<InvoicePayment> InvoicePayments { get; set; }
        public decimal GetSum()
        {
            decimal sum = 0m;
            foreach (var i in InvoiceItems)
            {
                sum += i.Price * i.Amount;
            }
            return sum;
        }
    }


    public class InvoicePayment
    {
        public int Id { get; set; }
        public decimal Payed { get; set; }
        public DateTime Date { get; set; }
        public virtual Invoice Invoice { get; set; }
    }


    public class InvoiceItem
    {
        public int Id { get; set; }
        public string Articul { get; set; }
        public string Product { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}
