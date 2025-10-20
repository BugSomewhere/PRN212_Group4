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

        public void FillComboBox()
        {
            RoleService roleService = new RoleService();
            List<Role> roles = roleService.GetAll();

            combo.ItemsSource = roles.ToList();
            combo.DisplayMemberPath = "Name";
            combo.SelectedValuePath = "Id";


            combo.SelectedValue = userToUpdate.RoleId;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (userToUpdate != null)
            {
                txtFullName.Text = userToUpdate.FullName;
                txtEmail.Text = userToUpdate.Email;
                txtRoleId.Text = userToUpdate.RoleId.ToString();
                txtPassword.Password = userToUpdate.Password;
                txtTotalCredit.Text = userToUpdate.TotalCredit.ToString();
            }
            FillComboBox();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var id= combo.SelectedItem as Role;

            if (userToUpdate != null)
            {
                userToUpdate.FullName = txtFullName.Text;
                userToUpdate.Email = txtEmail.Text;


                userToUpdate.RoleId = id.Id;
                userToUpdate.Password = txtPassword.Password;

                service.UpdateUser(userToUpdate);
                MessageBox.Show("User updated successfully!");
                this.Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void combo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}