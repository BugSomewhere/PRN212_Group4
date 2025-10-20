using PRN212_Group4.BLL;
using PRN212_Group4.DAL.Entities;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PRN212_Group4
{
    public partial class PaymentWindow : Window
    {
        private readonly List<Product> _products;
        private ProductService service = new();
        private UserService userService = new();

        public PaymentWindow(List<Product> products)
        {
            InitializeComponent();
            _products = products;
            LoadPaymentInfo();
        }

        private void LoadPaymentInfo()
        {
            decimal totalPrice = _products.Sum(p => p.Price ?? 0);
            string productList = string.Join(", ", _products.Select(p => p.Title));
            txtProductTitle.Text = $"Sản phẩm: {productList}";
            txtPrice.Text = $"Tổng tiền: {totalPrice:N0} VND";
            string qrContent = $"Thanh toán {_products.Count} sản phẩm\n" +
                               $"Tổng tiền: {totalPrice:N0} VND\n" +
                               $"Chi tiết: {productList}";

            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrBitmap = qrCode.GetGraphic(20);

            imgQr.Source = BitmapToImageSource(qrBitmap);
        }

        private static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        private void ConfirmPayment_Click(object sender, RoutedEventArgs e)
        {
            var currentUserId = ((App)Application.Current).CurrentUserId ?? 0;
            if (currentUserId == 0)
            {
                MessageBox.Show("Vui lòng đăng nhập để thanh toán!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var orderService = new OrderService();
            decimal total = _products.Sum(p => p.Price ?? 0);

            try
            {
                foreach (var product in _products)
                {
                    orderService.CreateOrder(currentUserId, product.Id, product.Price ?? 0);
                    service.UpdateProductPaidStatus(product.Id);
                }

                // Update status cho các order mới tạo (giả sử lấy last orders tương ứng số product)
                var recentOrders = orderService.GetAllOrders().OrderByDescending(o => o.Id).Take(_products.Count).ToList();
                foreach (var order in recentOrders)
                {
                    orderService.UpdateOrderStatus(order.Id, "Sucess");
                }

                MessageBox.Show($"Thanh toán thành công {_products.Count} sản phẩm, tổng cộng {total:N0} VND! Credit đã được cập nhật.");
                this.DialogResult = true; // Để CartWindow clear cart nếu gọi từ đó
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi thanh toán: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            HomePageWindow h = new HomePageWindow();
            h.Show();
            Close();
        }
    }
}


