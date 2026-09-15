using RPMExamPodgot.Data;
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
using System.Windows.Shapes;

namespace RPMExamPodgot
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow() => InitializeComponent();

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            var login = LoginBox.Text.Trim();
            var password = PasswordBox.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите логин и пароль.");
                return;
            }

            User user;
            using (var db = new AppDbContext())
            {
                user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            }

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }

            var main = new MainWindow(user);
            main.Show();
            Close();
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            // Гость — открываем просмотр товаров (только чтение)
            var products = new ProductsWindow(canEdit: false);
            products.Show();
            Close();
        }
    }
}
