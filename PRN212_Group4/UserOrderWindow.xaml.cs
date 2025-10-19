using PRN212_Group4.DAL.Entities;
using System;
using System.Collections;
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
using static System.Net.Mime.MediaTypeNames;

namespace PRN212_Group4
{
    /// <summary>
    /// Interaction logic for UserOrderWindow.xaml
    /// </summary>
    public partial class UserOrderWindow : Window
    {
        private int currentUserId;
        private readonly BLL.UserOrders userOrders = new BLL.UserOrders();
        private readonly BLL.ProductService productService = new BLL.ProductService();
        public UserOrderWindow()
        {
            InitializeComponent();
        }

        public UserOrderWindow(int userId)
        {
            InitializeComponent();
            currentUserId = userId;
            Loaded += UserOrderWindow_Loaded;
        }
        private void UserOrderWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUserOrders(currentUserId);
        }

        private void LoadUserOrders(int user_id)
        {
            var products = productService.GetAllProducts();

            if (user_id == 2)
            {
                //Admin view 
                var orders = userOrders.GetAllOrders();
                var adminOrderDetails = from order in orders
                                        join product in products on order.ProductId equals product.Id
                                        select new
                                        {
                                            OrderID = order.Id,
                                            ProductId = product.Id,
                                            ProductTitle = product.Title,
                                            ProductBrand = product.Brand,
                                            ProductModel = product.Model,
                                            ProductColor = product.Color,
                                            OrderPrice = order.Price,
                                            OrderStatus = order.Status,
                                            ProductDemesion = product.Dimension,
                                            OrderBuyerId = order.BuyerId
                                        };

                var adminResultList = adminOrderDetails.ToList();

                if (adminResultList.Count == 0)
                {
                    MessageBox.Show("There are no orders in the system.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                dgUserOrders.ItemsSource = adminResultList;
            }
            else
            {
                //user view 
                var userOrders = this.userOrders.GetOrdersByUserId(user_id);
                var userOrderDetails = from order in userOrders
                                       join product in products on order.ProductId equals product.Id
                                       select new
                                       {
                                           OrderID = order.Id,
                                           ProductId = product.Id,
                                           ProductTitle = product.Title,
                                           ProductBrand = product.Brand,
                                           ProductModel = product.Model,
                                           ProductColor = product.Color,
                                           OrderPrice = order.Price,
                                           OrderStatus = order.Status,
                                           ProductDemesion = product.Dimension,
                                           OrderBuyerId = order.BuyerId
                                       };

                var userResultList = userOrderDetails.ToList();

                if (userResultList.Count == 0)
                {
                    MessageBox.Show("You have no orders.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                dgUserOrders.ItemsSource = userResultList;
            }
        }





        private void FeedBackButton_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var orderDetail = button.DataContext;
            
            try
            {
                dynamic item = orderDetail;
                int productId = item.ProductId; 
                
                if (productId != 0)
                {
                    FeedbackManagment w = new FeedbackManagment(currentUserId, productId);
                    w.ShowDialog();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Could not find product information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btntransactiondetail_Click(object sender, RoutedEventArgs e)
        {
            TransactionDetailWindow w = new TransactionDetailWindow(currentUserId);
            w.ShowDialog();
            this.Close();
        }
    }
    
}
