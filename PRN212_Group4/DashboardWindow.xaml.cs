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
        private UserService service = new();
        public DashboardWindow()
        {
            InitializeComponent();
        }

        public void refreshData()
        {
            listUser.ItemsSource = service.GetAllUsers();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           listUser.ItemsSource = service.GetAllUsers();
            refreshData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow w = new();
            w.ShowDialog();
            Close();
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            UserManagementWindow w = new();
            w.ShowDialog();
            listUser.ItemsSource = service.GetAllUsers();
            Close();
        }
    }
}
