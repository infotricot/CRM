using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace diplom_2.Models
{
    public class Contact
    {
        public int Id { get; set; }
        [Display(Name = "Имя, фамилия")]
        public string FIO { get; set; }
        [Display(Name = "Должность")]
        public string Position { get; set; }
        [Display(Name = "Номера телефонов")]
        public string Phones { get; set; }
        [Display(Name = "E-mail адреса")]
        public string Email { get; set; }
        [Display(Name = "Номер skype")]
        public string Skype { get; set; }
        [Display(Name = "Номер viber")]
        public string Viber { get; set; }
        [Display(Name = "Страница Facebook")]
        public string Facebook { get; set; }
        [Display(Name = "Заметки")]
        public string Notes { get; set; }
        [Display(Name = "Статус")]
        public bool Deleted { get; set; }

        public virtual Counterparty Counterparty { get; set; }
    }
}