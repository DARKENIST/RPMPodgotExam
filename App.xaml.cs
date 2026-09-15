using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RPMExamPodgot.Data;

namespace RPMExamPodgot
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Создаём БД и наполняем начальными данными
            AppDbContext.InitializeDatabase();

            new LoginWindow().Show();
        }
    }
}
