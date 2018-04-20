using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace diplom_2.Models
{
    public class ProcessType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Process> Proceses { get; set; }
    }

    public class Process : ProcessOrderBase
    {
 
        [Display(Name = "Когда задача была выполнена? (оставить пустым, если выполнение планируется в будущем)")]                
        public DateTime? ExecuteDate { get; set; } //Дата, когда действие было совршено
        [Display(Name = "Планируемая дата выполнения задачи")]
        [Required]
        public DateTime PlaningDate { get; set; } //Дата, на когда запланировано выполнение задачи
        [Display(Name = "Комментарий")]
        [AllowHtml]
        public string Description { get; set; }
        [AllowHtml]
        public string ExecuteDescription { get; set; }
      
        public virtual ProcessType ProcessType { get; set; }

        public virtual ApplicationUser CreatedBy { get; set; }
    
    }
}