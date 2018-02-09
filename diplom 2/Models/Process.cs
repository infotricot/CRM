using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace diplom_2.Models
{
    public class ProcessType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Process> Proceses { get; set; }
    }

    public class Process
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; } //Дата, создания задачи
        public DateTime ExecuteDate { get; set; } //Дата, когда действие совршено
        public DateTime PlaningDate { get; set; } //Дата, на когда запланировано выполнение задачи

        public string Description  { get; set; }

        public virtual ProcessType ProcessType { get; set; }

        public virtual ApplicationUser Manager { get; set; }
        public virtual Counterparty Counterparty { get; set; }
    }
}