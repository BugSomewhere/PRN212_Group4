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
    /// Interaction logic for CreateProductWindow.xaml
    /// </summary>
    public partial class CreateProductWindow : Window
    {
        private readonly ProductService _service = new();
        private int created_by;
        public CreateProductWindow(User user)
        {
            InitializeComponent();
            created_by = user.Id;
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
                    Status = "Pending",
                    CreatedBy = created_by
                };

                _service.AddProduct(product);

                MessageBox.Show("Thêm sản phẩm thành công!");

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu sản phẩm: {ex.Message}");
            }
        }
    }
}
