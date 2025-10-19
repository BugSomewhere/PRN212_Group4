using PRN212_Group4.BLL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PRN212_Group4
{
    public partial class PostApprovalWindow : Window
    {
        private ProductService productService = new ProductService();
        private FeedbackServices feedbackService = new FeedbackServices();
        private Product selectedProduct = null;

        public PostApprovalWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshPendingProducts();
        }

        private void RefreshPendingProducts()
        {
            // Lấy list Product với Status = "Pending"
            var pendingProducts = productService.GetAllProducts().Where(p => p.Status.ToLower() == "pending").ToList();
            dgPendingProducts.ItemsSource = pendingProducts;

            if (pendingProducts.Count == 0)
            {
                MessageBox.Show("Không có sản phẩm nào đang chờ duyệt.", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void dgPendingProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedProduct = dgPendingProducts.SelectedItem as Product;
            if (selectedProduct != null)
            {
                // Hiển thị chi tiết
                txtDescription.Text = $"Description: {selectedProduct.Description ?? "N/A"}";
                txtColor.Text = $"Color: {selectedProduct.Color ?? "N/A"}";
                txtDimension.Text = $"Dimension: {selectedProduct.Dimension ?? "N/A"}";

                // Hiển thị Feedbacks cho sản phẩm
                var feedbacks = feedbackService.GetFeedbacksByProductId(selectedProduct.Id);
                dgFeedbacks.ItemsSource = feedbacks;
            }
            else
            {
                ClearDetails();
            }
        }

        private void ClearDetails()
        {
            txtDescription.Text = "Description: ";
            txtColor.Text = "Color: ";
            txtDimension.Text = "Dimension: ";
            dgFeedbacks.ItemsSource = null;
            txtRejectReason.Text = string.Empty;
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để duyệt.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn approve sản phẩm này?", "Xác Nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedProduct.Status = "approved";
                productService.UpdateProduct(selectedProduct);
                MessageBox.Show("Sản phẩm đã được approve thành công!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshPendingProducts();
                ClearDetails();
            }
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            if (selectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để reject.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string reason = txtRejectReason.Text.Trim();
            if (string.IsNullOrEmpty(reason))
            {
                MessageBox.Show("Vui lòng nhập lý do reject.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn reject sản phẩm này với lý do: '{reason}'?", "Xác Nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedProduct.Status = "Rejected";
                // maybe reject vào một field mới trong Product :"D
                // selectedProduct.RejectReason = reason; // Nếu thêm field
                productService.UpdateProduct(selectedProduct);
                MessageBox.Show("Sản phẩm đã được reject thành công!", "Thành Công", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshPendingProducts();
                ClearDetails();
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DashboardWindow dashboard = new DashboardWindow();
            dashboard.Show();
            this.Close();
        }
    }
}