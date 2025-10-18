using Login_Register;
using Microsoft.IdentityModel.Tokens;
using PRN212_Group4.BLL;
using PRN212_Group4.DAL;
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
    /// Interaction logic for HomePageWindow.xaml
    /// </summary>
    public partial class HomePageWindow : Window
    {
        private ProductService productService;
        private FeedbackServices feedbackService;
        private User currentUser;
        private List<Product> allProducts;
        private List<Product> filteredProducts;

        public HomePageWindow()
        {
            InitializeComponent();
            InitializeServices();
            LoadProducts();
            SetupFilters();
        }

        public HomePageWindow(User user) : this()
        {
            currentUser = user;
        }

        private void InitializeServices()
        {
            productService = new ProductService();
            feedbackService = new FeedbackServices();
        }

        private void LoadProducts()
        {
            try
            {
                // Load only approved products for regular users with User information
                using (var context = new PrnGroupProjectContext())
                {
                    allProducts = context.Products
                        .Where(p => p.Status == "approved")
                        .ToList();

                    filteredProducts = new List<Product>(allProducts);
                    lvProducts.ItemsSource = filteredProducts;

                    // Update the display to show creator information
                    lvProducts.ItemContainerGenerator.StatusChanged += (s, e) =>
                    {
                        if (lvProducts.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                        {
                            UpdateCreatorInfo();
                        }
                    };
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateCreatorInfo()
        {
            try
            {
                using (var context = new PrnGroupProjectContext())
                {
                    foreach (var item in lvProducts.Items)
                    {
                        var product = item as Product;
                        if (product != null && product.CreatedBy.HasValue)
                        {
                            var creator = context.Users.Find(product.CreatedBy.Value);
                            if (creator != null)
                            {
                                // Find the corresponding ListViewItem and update the creator text
                                var container = lvProducts.ItemContainerGenerator.ContainerFromItem(item) as ListViewItem;
                                if (container != null)
                                {
                                    var txtCreatedByUser = FindVisualChild<TextBlock>(container, "txtCreatedByUser");
                                    if (txtCreatedByUser != null)
                                    {
                                        txtCreatedByUser.Text = $"Posted by: {creator.FullName}";
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Silently handle error to avoid disrupting user experience
                System.Diagnostics.Debug.WriteLine($"Error updating creator info: {ex.Message}");
            }
        }

        private T FindVisualChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T && (child as FrameworkElement)?.Name == childName)
                {
                    return child as T;
                }
                var childOfChild = FindVisualChild<T>(child, childName);
                if (childOfChild != null)
                {
                    return childOfChild;
                }
            }
            return null;
        }

        private void SetupFilters()
        {
            try
            {
                // Setup Type filter
                var types = allProducts.Select(p => p.Type).Distinct().Where(t => !string.IsNullOrEmpty(t)).ToList();
                cmbTypeFilter.Items.Clear();
                cmbTypeFilter.Items.Add(new ComboBoxItem { Content = "All Types", Tag = "" });
                foreach (var type in types)
                {
                    cmbTypeFilter.Items.Add(new ComboBoxItem { Content = type, Tag = type });
                }
                cmbTypeFilter.SelectedIndex = 0;

                // Setup Brand filter
                var brands = allProducts.Select(p => p.Brand).Distinct().Where(b => !string.IsNullOrEmpty(b)).ToList();
                cmbBrandFilter.Items.Clear();
                cmbBrandFilter.Items.Add(new ComboBoxItem { Content = "All Brands", Tag = "" });
                foreach (var brand in brands)
                {
                    cmbBrandFilter.Items.Add(new ComboBoxItem { Content = brand, Tag = brand });
                }
                cmbBrandFilter.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error setting up filters: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterProducts()
        {
            try
            {
                var selectedType = (cmbTypeFilter.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "";
                var selectedBrand = (cmbBrandFilter.SelectedItem as ComboBoxItem)?.Tag?.ToString() ?? "";
                var searchText = txtSearch.Text.ToLower();

                filteredProducts = allProducts.Where(p =>
                    (string.IsNullOrEmpty(selectedType) || p.Type == selectedType) &&
                    (string.IsNullOrEmpty(selectedBrand) || p.Brand == selectedBrand) &&
                    (string.IsNullOrEmpty(searchText) ||
                     p.Title?.ToLower().Contains(searchText) == true ||
                     p.Description?.ToLower().Contains(searchText) == true ||
                     p.Brand?.ToLower().Contains(searchText) == true)
                ).ToList();

                lvProducts.ItemsSource = filteredProducts;

                // Update creator info after filtering
                Dispatcher.BeginInvoke(new Action(() => UpdateCreatorInfo()), System.Windows.Threading.DispatcherPriority.Loaded);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error filtering products: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        #region Event Handlers

        private void FilterChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterProducts();
        }

        private void txtSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtSearch.Text == "Search products...")
            {
                txtSearch.Text = "";
                txtSearch.Foreground = Brushes.Black;
            }
        }

        private void txtSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSearch.Text))
            {
                txtSearch.Text = "Search products...";
                txtSearch.Foreground = Brushes.Gray;
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtSearch.Text != "Search products...")
            {
                FilterProducts();
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadProducts();
            SetupFilters();
            // Creator info will be updated automatically in LoadProducts
        }

        private void btnLike_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var productId = Convert.ToInt32(button.Tag);

                // Create a simple like feedback
                var feedback = new Feedback
                {
                    UserId = currentUser?.Id,
                    ProductId = productId,
                    Comment = "👍 Liked this product!"
                };

                if (feedbackService.AddFeedbackboo(feedback))
                {
                    MessageBox.Show("Product liked successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to like product. You may have already liked it.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error liking product: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnComment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var productId = Convert.ToInt32(button.Tag);

                // Create a simple input dialog
                var inputDialog = new Window()
                {
                    Title = "Add Comment",
                    Width = 400,
                    Height = 200,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner,
                    Owner = this
                };

                var stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(20);

                var label = new TextBlock() { Text = "Enter your comment:", Margin = new Thickness(0, 0, 0, 10) };
                var textBox = new TextBox() { Height = 80, TextWrapping = TextWrapping.Wrap, AcceptsReturn = true };
                var buttonPanel = new StackPanel() { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right, Margin = new Thickness(0, 10, 0, 0) };

                var okButton = new Button() { Content = "OK", Width = 80, Margin = new Thickness(0, 0, 10, 0) };
                var cancelButton = new Button() { Content = "Cancel", Width = 80 };

                string result = "";
                okButton.Click += (s, args) => { result = textBox.Text; inputDialog.Close(); };
                cancelButton.Click += (s, args) => { inputDialog.Close(); };

                buttonPanel.Children.Add(okButton);
                buttonPanel.Children.Add(cancelButton);
                stackPanel.Children.Add(label);
                stackPanel.Children.Add(textBox);
                stackPanel.Children.Add(buttonPanel);
                inputDialog.Content = stackPanel;

                inputDialog.ShowDialog();

                if (!string.IsNullOrWhiteSpace(result))
                {
                    var feedback = new Feedback
                    {
                        UserId = currentUser?.Id,
                        ProductId = productId,
                        Comment = result
                    };

                    if (feedbackService.AddFeedbackboo(feedback))
                    {
                        MessageBox.Show("Comment added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add comment.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding comment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to logout?", "Logout",
                MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Close();
            }
        }

        #endregion
    }
}
