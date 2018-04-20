using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace diplom_2.Models
{
    // Чтобы добавить данные профиля для пользователя, можно добавить дополнительные свойства в класс ApplicationUser. Дополнительные сведения см. по адресу: http://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }

        public bool Deleted { get; set; }

        public virtual ICollection<Counterparty> Counterparties { get; set; }
        public virtual ICollection<Process> Proceses { get; set; }
        public virtual ApplicationUser ParentManager { get; set; }
        public virtual ICollection<ApplicationUser> SubManagers { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }

        public static List<ApplicationUser> GetUsersInRole(string roleName)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var roleManager =
             new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindByName(roleName).Users.First();
            var usersInRole = db.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(role.RoleId)).ToList();
            return usersInRole;
        }

        public static List<ApplicationUser> GetSubManagers(string Name)
        {
            
            ApplicationDbContext db = new ApplicationDbContext();
            var CurrentManager = db.Users.Where(a => a.UserName == Name).FirstOrDefault();
            //Если пользователь, для которого нужно получить список подчинённых - администратор
            if (CurrentManager.Roles.FirstOrDefault().RoleId == "3")
            {
                //получаем список всех менеджеров (удалённых и руководящих)
                var Managers = db.Users.Where(a => a.Roles.FirstOrDefault().RoleId == "1" || a.Roles.FirstOrDefault().RoleId == "5").ToList();
                return Managers;
            }
            else
            //Если пользователь, для которого нужно получить список подчинённых - директор
            if (CurrentManager.Roles.FirstOrDefault().RoleId == "0")
            {
                //получаем список всех сотрудников, кроме текущего пользователя (директора)
                var Employers = db.Users.Where(a => a.Name != CurrentManager.Name).ToList();
                return Employers;
            }
            //иначе, если пользователь - руководящий менеджер
            else
            {
                //получаем список его подчинённых менеджеров
                var SubManagers = CurrentManager.SubManagers.ToList();
                return SubManagers;
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Counterparty> Counterparties { get; set; }
        public virtual DbSet<StatusOrder> StatusOrders { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductInOrder> ProductInOrders { get; set; }
        public virtual DbSet<ProcessType> ProcessTypes { get; set; }
        public virtual DbSet<Process> Proceses { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }
        public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }


        //public System.Data.Entity.DbSet<diplom_2.Models.ApplicationUser> ApplicationUsers { get; set; }
    }
}