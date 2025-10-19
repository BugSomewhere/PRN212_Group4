using Login_Register;
using PRN212_Group4.BLL;
using PRN212_Group4.DAL.Entities;
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
    /// Interaction logic for HomePageWindow.xaml
    /// </summary>
    public partial class HomePageWindow : Window
    {
        private ProductService service = new();


        public HomePageWindow()
        {
            InitializeComponent();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).CurrentUserId = null;

            LoginWindow l = new();
            l.Show();
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshProductList();
        }

        public void RefreshProductList()
        {
            ProductList.ItemsSource = service.GetAllApproveProducts();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Title.Text))
                ProductList.ItemsSource = service.SearchApproveProducts(Title.Text);
            else
                RefreshProductList();
        }

        private void Payment_Click(object sender, RoutedEventArgs e)
        {
            List<Product> selectProducts = ProductList.SelectedItems.Cast<Product>().ToList();
            RefreshProductList();
            PaymentWindow p = new(selectProducts);
            p.ShowDialog();
            Close();
        }

        private void Order_Click(object sender, RoutedEventArgs e)
        {
            RefreshProductList();
            UserOrderWindow u = new UserOrderWindow();
            u.Show();
            Close();
        }

        private void Update_Account_Click(object sender, RoutedEventArgs e)
        {
            if (((App)Application.Current).CurrentUser == null)
            {
                MessageBox.Show("Invalid User");
                return;
            }
            User user = ((App)Application.Current).CurrentUser;
            UpdateUserWindow updateUserWindow = new UpdateUserWindow(user);
            updateUserWindow.ShowDialog();
            RefreshProductList();
        }

        private void Create_Order_Click(object sender, RoutedEventArgs e)
        {
            if (((App)Application.Current).CurrentUser == null)
            {
                MessageBox.Show("Invalid User");
                return;
            }
            User user = ((App)Application.Current).CurrentUser;
            CreateProductWindow createProductWindow = new CreateProductWindow(user);
            createProductWindow.ShowDialog();
            RefreshProductList();
        }

        private void Post_Click(object sender, RoutedEventArgs e)
        {
            if (((App)Application.Current).CurrentUser == null)
            {
                MessageBox.Show("Invalid User");
                return;
            }
            User user = ((App)Application.Current).CurrentUser;
            UserPostWindow u = new UserPostWindow(user);
            u.Show();
            this.Close();
        }
    }
}
