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
    /// Interaction logic for UserPostWindow.xaml
    /// </summary>
    public partial class UserPostWindow : Window
    {
        private int userID;
        private ProductService service = new();
        public UserPostWindow(User user)
        {
            InitializeComponent();
            userID = user.Id;
        }

        public void RefreshPostList()
        {
            UserPostList.ItemsSource = service.GetAllProductsByUserId(userID);
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            HomePageWindow h = new HomePageWindow();
            h.Show();
            this.Close();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            UpdateProductWindow u = new UpdateProductWindow((Product)UserPostList.SelectedItem);
            u.ShowDialog();
            RefreshPostList();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            UserPostList.ItemsSource = service.GetAllProductsByUserId(userID);
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Title.Text))
                UserPostList.ItemsSource = service.SearchUserProducts(Title.Text, userID);
            else
                RefreshPostList();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            service.DeleteProduct(((Product)UserPostList.SelectedItem).Id);
            RefreshPostList();
        }
    }
}
