using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace diplom_2.Models
{
    public enum CounterpartyType { Поставщик, Клиент }

    public class Counterparty
    {
        public int Id { get; set; }
        [Display(Name="Название")]
        public string Name { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Город")]
        public string City { get; set; }
        [Display(Name = "Область")]
        public string Region { get; set; }
        [Display(Name = "Адрес")]
        public string Address { get; set; }
        [Display(Name = "Реквизиты")]
        public string Requisites { get; set; }
        [Display(Name = "Сайт")]
        public string Web { get; set; }
        [Display(Name = "Статус")]
        public string Status { get; set; }

        [Display(Name = "Тип")]
        public CounterpartyType CounterpartyType { get; set; }
        [Display(Name = "Дата добавления")]
        public DateTime Created { get; set; }
        [Display(Name = "Последнее действие")]
        public DateTime LastModify { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Contact> Contacts { get; set; }
        public virtual ICollection<ApplicationUser> Managers { get; set; }
        public virtual ICollection<Process> Proceses { get; set; }

        public static List<Counterparty> GetAllCounterparties()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Counterparties.ToList();
        }
    }
}