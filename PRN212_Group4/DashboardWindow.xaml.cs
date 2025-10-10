using PRN212_Group4.BLL;
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

namespace PRN212_Group4
{
    /// <summary>
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
        }

        private UserService service = new();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           listUser.ItemsSource = service.GetAllUsers();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow w = new();
            w.ShowDialog();
            this.Close();
        }
    }
}
