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
        private List<Products> _items;

        public ProductsWindow(bool canEdit)
        {
            InitializeComponent();
            _canEdit = canEdit;

            using (var db = new RPMExamDBEntities())
            {
                _items = db.Products.ToList();
            }
            Grid.ItemsSource = _items;

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
            Grid.CommitEdit(DataGridEditingUnit.Row, true);

            using (var db = new RPMExamDBEntities())
            {
                foreach (var p in _items)
                {
                    if (p.Id == 0)
                    {
                        db.Products.Add(p);
                    }
                    else
                    {
                        var existing = db.Products.Find(p.Id);
                        if (existing != null)
                        {
                            existing.Name = p.Name;
                            existing.Price = p.Price;
                            existing.Quantity = p.Quantity;
                        }
                    }
                }
                db.SaveChanges();
            }

            using (var db = new RPMExamDBEntities())
            {
                _items = db.Products.ToList();
            }
            Grid.ItemsSource = _items;

            MessageBox.Show("Изменения сохранены.");
        }

        private void Close_Click(object sender, RoutedEventArgs e) => Close();
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();

            Close();
        }
    }
}