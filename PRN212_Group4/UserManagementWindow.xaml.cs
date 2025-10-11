using PRN212_Group4.BLL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PRN212_Group4
{
    public partial class UserManagementWindow : Window
    {
        private UserService service = new();
        private User selectedUser = null;

        public UserManagementWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dgUsers.ItemsSource = service.GetAllUsers();
        }

        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedUser = dgUsers.SelectedItem as User;
            btnDelete.IsEnabled = selectedUser != null;
            btnUpdate.IsEnabled = selectedUser != null;
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null && MessageBox.Show("Bạn có chắc muốn xóa user này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                service.DeleteUser(selectedUser.Id);
                dgUsers.ItemsSource = service.GetAllUsers(); // Refresh list
                selectedUser = null;
                btnDelete.IsEnabled = false;
                btnUpdate.IsEnabled = false;
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUser != null)
            {
                UpdateUserWindow updateWindow = new(selectedUser);
                updateWindow.ShowDialog();
                dgUsers.ItemsSource = service.GetAllUsers(); // Refresh sau khi update
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}