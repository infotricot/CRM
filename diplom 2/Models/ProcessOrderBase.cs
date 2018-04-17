using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diplom_2.Models
{
    public class ProcessOrderBase
    {
        public int Id { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreateDate { get; set; }
        //null - задача в процессе выполнение, true - задача выполнена, false - задача не выполнена по каким-то причинам (менеджер отказался её выполнять)
        public bool? IsExecuted { get; set; } 
        public virtual ApplicationUser Manager { get; set; }
        public virtual Counterparty Counterparty { get; set; }
    }
}