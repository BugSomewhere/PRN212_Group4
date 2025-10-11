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
    /// Interaction logic for TransactionDetailWindow.xaml
    /// </summary>
    public partial class TransactionDetailWindow : Window
    {
        private int currentUserId;
        private int currentproductId;
        private readonly BLL.UserOrders userOrders = new BLL.UserOrders();
        private readonly BLL.ProductService productService = new BLL.ProductService();
        private readonly BLL.TransactionDetailsServices transactionDetailsServices = new BLL.TransactionDetailsServices();

        public TransactionDetailWindow()
        {
            InitializeComponent();
        }

        public TransactionDetailWindow(int userid)
        {
            InitializeComponent();
            currentUserId = userid;
            Loaded += TransactionDetailWindow_Loaded;
        }

        private void TransactionDetailWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            
            var transactionDetailsList = transactionDetailsServices.GetAllTransactionDetails();
            var orders = userOrders.GetAllOrders();
            var products = productService.GetAllProducts();
            if(currentUserId == 1) {
                var adminTransactionView = from transaction in transactionDetailsList
                                           join order in orders on transaction.OrderId equals order.Id
                                           join product in products on order.ProductId equals product.Id
                                           select new
                                           {
                                               Transactionid = transaction.Id,
                                               Orderid = order.Id,
                                               ProductID = product.Id,
                                               ProductTitle = product.Title,
                                               ProductBrand = product.Brand,
                                               ProductModel = product.Model,
                                               Type = transaction.Type,
                                               Amount = transaction.Amount,
                                               OrderStatus = order.Status,
                                               OrderPrice = order.Price,
                                               BuyerId = order.BuyerId
                                           };

                var adminResultList = adminTransactionView.ToList();

                if (adminResultList.Count == 0)
                {
                    MessageBox.Show("No transaction details found in the system.",
                                  "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                dgUserOrders.ItemsSource = adminResultList;
            }
            else
            {
                
                var userTransactionView = from transaction in transactionDetailsList
                                          join order in orders on transaction.OrderId equals order.Id
                                          join product in products on order.ProductId equals product.Id
                                          where order.BuyerId == currentUserId
                                          select new
                                          {
                                              Transactionid = transaction.Id,
                                              Orderid = order.Id,
                                              ProductID = product.Id,
                                              ProductTitle = product.Title,
                                              ProductBrand = product.Brand,
                                              ProductModel = product.Model,
                                              Type = transaction.Type,
                                              Amount = transaction.Amount,
                                              OrderStatus = order.Status,
                                              OrderPrice = order.Price,
                                              BuyerId = order.BuyerId
                                          };

                var userResultList = userTransactionView.ToList();

                if (userResultList.Count == 0)
                {
                    MessageBox.Show("No transaction details found for your account.",
                                  "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                dgUserOrders.ItemsSource = userResultList;
            }

        }
            
          

        private void dgUserOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = txtSearch.Text.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                MessageBox.Show("Please enter an Order ID to search", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (int.TryParse(searchText, out int orderId))
            {
                var transactionDetails = transactionDetailsServices.GetTransactionDetailsByOrderId(orderId);

                if (transactionDetails.Count == 0)
                {
                    MessageBox.Show($"No transaction details found for Order ID {orderId}",
                                  "No Results", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var orders = userOrders.GetAllOrders();
                var products = productService.GetAllProducts();

                
                if (currentUserId == 1)
                {
                   
                    var adminSearchResults = from transaction in transactionDetails
                                             join order in orders on transaction.OrderId equals order.Id
                                             join product in products on order.ProductId equals product.Id
                                             select new
                                             {
                                                 Transactionid = transaction.Id,
                                                 Orderid = order.Id,
                                                 ProductID = product.Id,
                                                 ProductTitle = product.Title,
                                                 ProductBrand = product.Brand,
                                                 ProductModel = product.Model,
                                                 Type = transaction.Type,
                                                 Amount = transaction.Amount,
                                                 OrderStatus = order.Status,
                                                 OrderPrice = order.Price,
                                                 BuyerId = order.BuyerId
                                             };

                    var adminResultList = adminSearchResults.ToList();

                    if (adminResultList.Count == 0)
                    {
                        MessageBox.Show("No transactions found for this order ID.",
                                      "No Results", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    dgUserOrders.ItemsSource = adminResultList;
                }
                else
                {
                    
                    var userSearchResults = from transaction in transactionDetails
                                            join order in orders on transaction.OrderId equals order.Id
                                            join product in products on order.ProductId equals product.Id
                                            where order.BuyerId == currentUserId
                                            select new
                                            {
                                                Transactionid = transaction.Id,
                                                Orderid = order.Id,
                                                ProductID = product.Id,
                                                ProductTitle = product.Title,
                                                ProductBrand = product.Brand,
                                                ProductModel = product.Model,
                                                Type = transaction.Type,
                                                Amount = transaction.Amount,
                                                OrderStatus = order.Status,
                                                OrderPrice = order.Price,
                                                BuyerId = order.BuyerId
                                            };

                    var userResultList = userSearchResults.ToList();

                    if (userResultList.Count == 0)
                    {
                        MessageBox.Show("This order does not belong to your account or has no transactions.",
                                      "No Results", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }

                    dgUserOrders.ItemsSource = userResultList;
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid Order ID (numeric value)",
                              "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
