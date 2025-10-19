using Login_Register;
using PRN212_Group4.BLL;
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
    /// Interaction logic for DashboardWindow.xaml
    /// </summary>
    public partial class DashboardWindow : Window
    {
        private UserService service = new();
        public DashboardWindow()
        {
            InitializeComponent();
            //// Kiểm tra quyền admin
            //var currentUser = ((App)Application.Current).CurrentUser;
            //if (currentUser == null || currentUser.RoleId != 1)
            //{
            //    MessageBox.Show("Chỉ admin mới truy cập được Dashboard!", "Lỗi Quyền", MessageBoxButton.OK, MessageBoxImage.Error);
            //    LoginWindow login = new LoginWindow();
            //    login.Show();
            //    this.Close();
            //}
        }

        public void refreshData()
        {
            listUser.ItemsSource = service.GetAllUsers();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           listUser.ItemsSource = service.GetAllUsers();
            refreshData();
            //LoadStats();
            //RefreshUserList();
        }

        //private void LoadStats()
        //{
        //    var stats = service.GetDashboardStats();

        //    // Hiển thị User theo Role
        //    var usersByRole = (Dictionary<int, int>)stats["UsersByRole"];
        //    txtUsersStats.Text = $"Số User theo Role: Admin: {usersByRole.GetValueOrDefault(1, 0)}, User: {usersByRole.GetValueOrDefault(2, 0)}";

        //    // Product theo Status
        //    var productsByStatus = (Dictionary<string, int>)stats["ProductsByStatus"];
        //    txtProductsStats.Text = $"Số Product theo Status: Pending: {productsByStatus.GetValueOrDefault("Pending", 0)}, Approved: {productsByStatus.GetValueOrDefault("Approved", 0)}, Rejected: {productsByStatus.GetValueOrDefault("Rejected", 0)}";

        //    // Tổng Orders và Revenue
        //    txtOrdersTotal.Text = $"Tổng Orders: {stats["TotalOrders"]}";
        //    txtRevenueTotal.Text = $"Tổng Revenue: {((decimal)stats["TotalRevenue"]):N0} VND";
        //}

        //private void RefreshUserList()
        //{
        //    listUser.ItemsSource = service.GetAllUsers();
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductListWindow w = new();
            w.Show();
            Close();
        }

        private void UserManagement_Click(object sender, RoutedEventArgs e)
        {
            UserManagementWindow w = new();
            w.ShowDialog();
            listUser.ItemsSource = service.GetAllUsers();
            Close();
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            ((App)Application.Current).CurrentUserId = null;

            LoginWindow l = new();
            l.Show();
            this.Close();
        }

        private void PostApproval_Click(object sender, RoutedEventArgs e)
        {
            PostApprovalWindow w = new PostApprovalWindow();
            w.ShowDialog();
        }
    }
}
