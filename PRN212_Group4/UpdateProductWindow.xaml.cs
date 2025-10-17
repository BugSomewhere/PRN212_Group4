using PRN212_Group4.BLL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Windows;

namespace PRN212_Group4
{
    public partial class UpdateProductWindow : Window
    {
        private ProductService service = new();
        private Product productToUpdate;

        public UpdateProductWindow(Product product)
        {
            InitializeComponent();
            productToUpdate = product;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (productToUpdate != null)
            {
                txtType.Text = productToUpdate.Type ?? "";
                txtStatus.Text = productToUpdate.Status ?? "";
                txtBrand.Text = productToUpdate.Brand ?? "";
                txtModel.Text = productToUpdate.Model ?? "";
                txtTitle.Text = productToUpdate.Title ?? "";
                txtDescription.Text = productToUpdate.Description ?? "";
                txtPrice.Text = productToUpdate.Price?.ToString() ?? "";
                txtColor.Text = productToUpdate.Color ?? "";
                txtDimension.Text = productToUpdate.Dimension ?? "";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (productToUpdate != null)
                {
                    // Validate required fields
                    if (string.IsNullOrWhiteSpace(txtTitle.Text))
                    {
                        MessageBox.Show("Title is required!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Update product properties
                    productToUpdate.Type = txtType.Text.Trim();
                    productToUpdate.Status = txtStatus.Text.Trim();
                    productToUpdate.Brand = txtBrand.Text.Trim();
                    productToUpdate.Model = txtModel.Text.Trim();
                    productToUpdate.Title = txtTitle.Text.Trim();
                    productToUpdate.Description = txtDescription.Text.Trim();
                    
                    // Parse price with validation
                    if (decimal.TryParse(txtPrice.Text, out decimal price))
                    {
                        productToUpdate.Price = price;
                    }
                    else if (!string.IsNullOrWhiteSpace(txtPrice.Text))
                    {
                        MessageBox.Show("Please enter a valid price!", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    else
                    {
                        productToUpdate.Price = null;
                    }
                    
                    productToUpdate.Color = txtColor.Text.Trim();
                    productToUpdate.Dimension = txtDimension.Text.Trim();

                    // Save changes
                    service.UpdateProduct(productToUpdate);
                    MessageBox.Show("Product updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
