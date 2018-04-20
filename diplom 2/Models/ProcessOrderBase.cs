using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom_2.Models
{
    public class ProcessOrderBase
    {
        public int Id { get; set; }

        [Display(Name = "Дата создания")]
        public DateTime CreateDate { get; set; }
        public bool? IsExecuted { get; set; } //null - задача в процессе выполнение, true - задача выполнена, false - задача не выполнена по каким-то причинам
        public virtual Counterparty Counterparty { get; set; }
        [ForeignKey("Manager")]
        public string Manager_Id { get; set; }
        public virtual ApplicationUser Manager { get; set; }
    }
}
