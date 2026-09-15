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
using System.Data.Entity;

namespace RPMExamPodgot
{
    /// <summary>
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        private readonly bool _canEdit;

        public ProductsWindow(bool canEdit)
        {
            InitializeComponent();
            _canEdit = canEdit;

            List<Product> products;
            using (var db = new AppDbContext())
            {
                products = db.Products.ToList();
            }
            Grid.ItemsSource = products;

            if (!_canEdit)
            {
                Grid.IsReadOnly = true;
                Grid.CanUserAddRows = false;
                Grid.CanUserDeleteRows = false;
                SaveBtn.Visibility = Visibility.Collapsed;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Grid.CommitEdit(System.Windows.Controls.DataGridEditingUnit.Row, true);

            var items = ((IEnumerable<Product>)Grid.ItemsSource).ToList();

            using (var db = new AppDbContext())
            {
                foreach (var p in items)
                {
                    if (p.Id == 0)
                        db.Products.Add(p);
                    else
                        db.Products.Attach(p);                      
                        db.Entry(p).State = EntityState.Modified;   
                }
                db.SaveChanges();
            }

            using (var db = new AppDbContext())
            {
                Grid.ItemsSource = db.Products.ToList();
            }

            MessageBox.Show("Изменения сохранены.");
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
    }
}
