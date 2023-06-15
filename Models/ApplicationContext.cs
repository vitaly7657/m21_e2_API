using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace m21_e2_API.Models
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        //создание таблицы абонентов в базе данных
        public DbSet<Subscriber> Subscribers { get; set; } 
        //контекст подключения к БД
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated(); //создание базы данных если при запуске её нет
        }
    }
}
