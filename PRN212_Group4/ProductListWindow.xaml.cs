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
    /// Interaction logic for ProductListWindow.xaml
    /// </summary>
    public partial class ProductListWindow : Window
    {
        private ProductService service = new();
        private Product selectedProduct = null;

        public ProductListWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshProductList();
        }

        public void RefreshProductList()
        {
            listProduct.ItemsSource = service.GetAllProducts();
        }

        private void Button_Click1(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text))
                listProduct.ItemsSource = service.SearchProducts(txtSearch.Text);
            else
                RefreshProductList();
        }

        private void dgProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProduct = listProduct.SelectedItem as Product;
            btnDelete.IsEnabled = selectedProduct != null;
            btnUpdate.IsEnabled = selectedProduct != null;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct != null && MessageBox.Show("Bạn có chắc muốn xóa sản phẩm này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                service.DeleteProduct(selectedProduct.Id);
                RefreshProductList();
                selectedProduct = null;
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct != null)
            {
                UpdateProductWindow updateWindow = new(selectedProduct);
                updateWindow.ShowDialog();
                RefreshProductList(); // Refresh sau khi update
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow d = new();
            d.Show();
            this.Close();
        }
    }
}
