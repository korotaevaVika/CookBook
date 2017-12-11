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
using MahApps.Metro.Controls;
namespace CookBook_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            ProductsMU.Visibility = Visibility.Collapsed;
            RecipesMU.Visibility = Visibility.Collapsed;
            PlansMU.Visibility = Visibility.Collapsed;
            BasketsMU.Visibility = Visibility.Collapsed;
        }

        private void Admin(object sender, RoutedEventArgs e)
        {
            txtRole.Text = "Администратор";
            ProductsMU.Visibility = Visibility.Visible;
            RecipesMU.Visibility = Visibility.Visible;
            PlansMU.Visibility = Visibility.Visible;
            BasketsMU.Visibility = Visibility.Visible;
        }
        private void Plan(object sender, RoutedEventArgs e)
        {
            txtRole.Text = "Планировщик";
            ProductsMU.Visibility = Visibility.Collapsed;
            RecipesMU.Visibility = Visibility.Collapsed;
            PlansMU.Visibility = Visibility.Visible;
            BasketsMU.Visibility = Visibility.Visible;
        }
        private void Recipe(object sender, RoutedEventArgs e)
        {
            txtRole.Text = "Хранитель рецептов";
            ProductsMU.Visibility = Visibility.Visible;
            RecipesMU.Visibility = Visibility.Visible;
            PlansMU.Visibility = Visibility.Collapsed;
            BasketsMU.Visibility = Visibility.Collapsed;
        }
    }
}
