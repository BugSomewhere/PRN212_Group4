using PRN212_Group4.BLL;
using PRN212_Group4.DAL.Entities;
using System.Windows;

namespace PRN212_Group4
{
    public partial class AddProductWindow : Window
    {
        private readonly ProductService _service = new();

        public AddProductWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên sản phẩm!");
                    return;
                }

                decimal? price = null;
                if (!string.IsNullOrWhiteSpace(txtPrice.Text))
                {
                    if (!decimal.TryParse(txtPrice.Text, out decimal parsedPrice))
                    {
                        MessageBox.Show("Giá không hợp lệ!");
                        return;
                    }
                    price = parsedPrice;
                }

                var product = new Product
                {
                    Title = txtTitle.Text.Trim(),
                    Type = txtType.Text.Trim(),
                    Brand = txtBrand.Text.Trim(),
                    Model = txtModel.Text.Trim(),
                    Price = price,
                    Color = txtColor.Text.Trim(),
                    Dimension = txtDimension.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    Status = "Available",
                    CreatedBy = 1
                };

                _service.AddProduct(product);

                MessageBox.Show("✅ Thêm sản phẩm thành công!");

                this.DialogResult = true; 
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Lỗi khi lưu sản phẩm: {ex.Message}");
            }
        }
    }
}
