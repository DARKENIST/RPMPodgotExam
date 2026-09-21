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
        private readonly Users _user;
        private readonly DispatcherTimer _timer;

        public MainWindow(Users user)
        {
            InitializeComponent();
            _user = user;

            // Кнопка управления — только для админа
            AdminButton.Visibility = user.Role == 1
                ? Visibility.Visible
                : Visibility.Collapsed;

            // Таймер на 1 секунду — обновляет строку с приветствием и временем
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) => UpdateHeader();
            _timer.Start();

            // Показать сразу, не ждать первую секунду
            UpdateHeader();
        }

        /// <summary>
        /// Возвращает приветствие по часу суток.
        /// 06:00–12:00 — Доброе утро
        /// 12:00–17:00 — Добрый день
        /// 17:00–23:00 — Добрый вечер
        /// 23:00–06:00 — Доброй ночи
        /// </summary>
        private static string GetGreetingByHour(int hour)
        {
            if (hour >= 6 && hour < 12) return "Доброе утро";
            if (hour >= 12 && hour < 17) return "Добрый день";
            if (hour >= 17 && hour < 23) return "Добрый вечер";
            return "Доброй ночи";
        }

        /// <summary>
        /// Обновляет приветствие и строку времени.
        /// Пример: "Добрый день, Иванов!       15.03.2025 14:07:42"
        /// </summary>
        private void UpdateHeader()
        {
            var now = DateTime.Now;
            string greeting = GetGreetingByHour(now.Hour);

            // Приветствие с обращением по фамилии
            GreetingText.Text = $"{greeting}, {_user.LastName}!";
        }

        private void OpenProducts_Click(object sender, RoutedEventArgs e)
        {
            new ProductsWindow(canEdit: _user.Role == 1).Show();
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

        protected override void OnClosed(EventArgs e)
        {
            _timer?.Stop();
            base.OnClosed(e);
        }
    }
}