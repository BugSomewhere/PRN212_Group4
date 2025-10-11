using PRN212_Group4.BLL;
using PRN212_Group4.DAL.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PRN212_Group4
{
    public partial class CartWindow : Window
    {
        private readonly CartService _cart;

        public CartWindow(CartService cart)
        {
            InitializeComponent();
            _cart = cart;
            LoadCart();
        }

        private void LoadCart()
        {
            cartGrid.ItemsSource = _cart.GetCartItems();
            txtTotal.Text = $"{_cart.GetTotalPrice():N0} VND";
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            var products = _cart.GetCartItems().Select(i => i.Product).ToList();

            if (products.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!");
                return;
            }

            PaymentWindow paymentWindow = new PaymentWindow(products);
            paymentWindow.ShowDialog();

            _cart.ClearCart();
            LoadCart();
        }
        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is CartItem item)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc muốn xóa '{item.Product.Title}' khỏi giỏ hàng?",
                    "Xác nhận",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _cart.RemoveFromCart(item.Product.Id);
                    LoadCart();
                }
            }
        }
    }
}
