using PRN212_Group4.DAL.Entities;
using PRN212_Group4.BLL;
using System;
using System.Windows;

namespace PRN212_Group4
{
    public partial class ProductDetailPopup : Window
    {
        private readonly Product _product;
        private readonly CartService _cart;

        public ProductDetailPopup(Product product, CartService cart)
        {
            InitializeComponent();
            _product = product;
            _cart = cart;
            LoadProduct();
        }

        private void LoadProduct()
        {
            txtTitle.Text = _product.Title;
            txtPrice.Text = $"Giá: {_product.Price?.ToString("N0")} VND";
            txtDescription.Text = _product.Description ?? "(Không có mô tả)";
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtQuantity.Text, out int quantity) || quantity <= 0)
            {
                MessageBox.Show("Số lượng không hợp lệ!");
                return;
            }

            _cart.AddToCart(_product, quantity);
            MessageBox.Show($"✅ Đã thêm {_product.Title} x{quantity} vào giỏ hàng!");
            this.DialogResult = true;
            this.Close();
        }
    }
}
