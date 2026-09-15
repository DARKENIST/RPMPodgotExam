using RPMExamPodgot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace RPMExamPodgot
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly User _user;
        private readonly DispatcherTimer _timer;

        public MainWindow(User user)
        {
            InitializeComponent();
            _user = user;

            string roleName = user.Role == UserRole.Admin ? "Администратор" : "Клиент";
            GreetingText.Text = $"Добро пожаловать, {roleName} ({user.Login})!";

            // Кнопка управления — только для админа
            AdminButton.Visibility = user.Role == UserRole.Admin
                ? Visibility.Visible
                : Visibility.Collapsed;

            // Таймер для обновления времени
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (sender, e) => UpdateTime();
            _timer.Start();
            UpdateTime();
        }

        private void UpdateTime()
        {
            TimeText.Text = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
        }

        private void OpenProducts_Click(object sender, RoutedEventArgs e)
        {
            // Клиент смотрит товары в режиме чтения
            new ProductsWindow(canEdit: _user.Role == UserRole.Admin).Show();
        }

        private void OpenAdminProducts_Click(object sender, RoutedEventArgs e)
        {
            new ProductsWindow(canEdit: true).Show();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
            new LoginWindow().Show();
            Close();
        }
    }
}
