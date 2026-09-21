using System.Linq;
using System.Windows;
using System.Windows.Controls;
using RPMExamPodgot.Models;  

namespace RPMExamPodgot
{
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

            Users user;
            using (var db = new RPMExamDBEntities())
            {
                user = db.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            }

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
                return;
            }

            new MainWindow(user).Show();
            Close();
        }

        private void Guest_Click(object sender, RoutedEventArgs e)
        {
            new ProductsWindow(canEdit: false).Show();
            Close();
        }
    }
}