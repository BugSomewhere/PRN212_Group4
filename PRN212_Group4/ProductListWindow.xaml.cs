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
    public partial class ProductListWindow : Window
    {
        public ProductListWindow()
        {
            InitializeComponent();
        }

        private BLL.ProductService service = new();

        private void Create_btn_Click(object sender, RoutedEventArgs e)
        {
            AddProductWindow addProductWindow = new AddProductWindow();

            bool? result = addProductWindow.ShowDialog();
            if (result == true)
            {
                listProduct.ItemsSource = null;
                listProduct.ItemsSource = service.GetAllProducts();
            }
        }

        private readonly CartService _cart = new();

        private void listProduct_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selected = listProduct.SelectedItem as PRN212_Group4.DAL.Entities.Product;
            if (selected == null) return;

            ProductDetailPopup popup = new ProductDetailPopup(selected, _cart);
            popup.Owner = this;
            popup.ShowDialog();
        }

        private void Cart_btn_Click(object sender, RoutedEventArgs e)
        {
            CartWindow cartWindow = new CartWindow(_cart);
            cartWindow.ShowDialog();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            listProduct.ItemsSource = service.GetAllProducts();
        }
    }
}
