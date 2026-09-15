using RPMExamPodgot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using RPMExamPodgot.Models;
using System.Data.Entity;

namespace RPMExamPodgot.Data
{
    public class AppDbContext : DbContext
    {
        // Имя строки подключения из App.config
        public AppDbContext() : base("name=DefaultConnection")
        {
            // Отключаем ленивую загрузку и автопроверку модели при старте
            // (для ускорения работы и во избежание лишних проверок)
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Явно указываем имена таблиц и первичные ключи
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(u => u.Id);

            modelBuilder.Entity<Product>()
                .ToTable("Products")
                .HasKey(p => p.Id);

            base.OnModelCreating(modelBuilder);
        }

        public static void InitializeDatabase()
        {
            // Инициализатор: создаёт БД при первом запуске, ничего не удаляет,
            // если структура изменилась — пользователь сам удалит app.db
            Database.SetInitializer(new CreateDatabaseIfNotExists<AppDbContext>());

            using (var db = new AppDbContext())
            {
                // Создаём БД (если её нет)
                db.Database.CreateIfNotExists();

                // Наполняем начальными данными, если таблицы пусты
                if (!db.Users.Any())
                {
                    db.Users.Add(new User { Login = "admin", Password = "admin", Role = UserRole.Admin });
                    db.Users.Add(new User { Login = "client", Password = "client", Role = UserRole.Client });
                    db.SaveChanges();
                }

                if (!db.Products.Any())
                {
                    db.Products.Add(new Product { Name = "Ноутбук", Price = 75000, Quantity = 5 });
                    db.Products.Add(new Product { Name = "Мышь", Price = 1500, Quantity = 30 });
                    db.Products.Add(new Product { Name = "Клавиатура", Price = 3200, Quantity = 15 });
                    db.SaveChanges();
                }
            }
        }
    }
}
