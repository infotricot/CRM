﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace diplom_2.Models
{
    public class StatusOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }

    public class Order
    {
        public int Id { get; set; }
        [Display(Name = "Накладная")]
        public string InvoiceUrl { get; set; }
        [Display(Name = "Дата создания")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Дата последнего действия")]
        public DateTime ChangeDate { get; set; }
        [Display(Name = "Срок поставки")]
        public DateTime ReadyDate { get; set; }

        [ForeignKey("Counterparty")]
        public int Counterparty_Id { get; set; }       
        [ForeignKey("StatusOrder")]
        public int StatusOrder_Id { get; set; }

        
        public virtual Counterparty Counterparty { get;set;}
        public virtual ApplicationUser Manager { get; set; }
        public virtual StatusOrder StatusOrder { get; set; }
        public virtual ICollection<ProductInOrder> Products { get; set; }
    }

    public class ProductInOrder
    {
        public int Id { get; set; }
        public long ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Price { get; set; }
        public string Comments { get; set; }
        public virtual Order Order { get; set; }
    }
}