using PRN212_Group4.BLL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Windows;

namespace PRN212_Group4
{
    public partial class UpdateUserWindow : Window
    {
        private UserService service = new();
        private User userToUpdate;

        public UpdateUserWindow(User user)
        {
            InitializeComponent();
            userToUpdate = user;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (userToUpdate != null)
            {
                txtFullName.Text = userToUpdate.FullName;
                txtEmail.Text = userToUpdate.Email;
                txtRoleId.Text = userToUpdate.RoleId.ToString();
                txtPassword.Password = userToUpdate.Password; // Lưu ý: PasswordBox không bind trực tiếp, chỉ hiển thị placeholder
                txtTotalCredit.Text = userToUpdate.TotalCredit.ToString();
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (userToUpdate != null)
            {
                userToUpdate.FullName = txtFullName.Text;
                userToUpdate.Email = txtEmail.Text;
                userToUpdate.RoleId = int.Parse(txtRoleId.Text);
                userToUpdate.Password = txtPassword.Password; // Lưu ý: Hash nếu cần
                userToUpdate.TotalCredit = decimal.Parse(txtTotalCredit.Text);

                service.UpdateUser(userToUpdate);
                MessageBox.Show("User updated successfully!");
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}