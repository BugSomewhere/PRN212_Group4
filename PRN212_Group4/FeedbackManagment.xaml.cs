using Microsoft.EntityFrameworkCore;
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
    /// Interaction logic for FeedbackManagment.xaml
    /// </summary>
    public partial class FeedbackManagment : Window
    {


        private int currentUserId;
        private int currentproductId;
        private dynamic currentSelectedItem;
        private readonly BLL.UserService userservice = new BLL.UserService();
        private readonly BLL.UserOrders userOrders = new BLL.UserOrders();
        private readonly BLL.ProductService productService = new BLL.ProductService();
        private readonly BLL.TransactionDetailsServices transactionDetailsServices = new BLL.TransactionDetailsServices();
        private readonly BLL.FeedbackServices feedbackService = new BLL.FeedbackServices();
        private PrnGroupProjectContext _context = new PrnGroupProjectContext();

        public FeedbackManagment()
        {
            InitializeComponent();
        }

        public FeedbackManagment(int userid, int productid)
        {
            InitializeComponent();
            currentUserId = userid;
            currentproductId = productid;
            Loaded += FeedbaclWindow_Loaded;
        }

        private void FeedbaclWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            var feedbacklist = feedbackService.GetAllFeedbacks();
            var products = productService.GetAllProducts();
            var users = userservice.GetAllUsers();

            var feedbackview = from feedback in feedbacklist
                               join product in products on feedback.ProductId equals product.Id
                               join user in users on feedback.UserId equals user.Id
                               where feedback.ProductId == currentproductId
                               select new
                               {
                                   Comment = feedback.Comment,
                                   ProductName = product.Title,
                                   UserName = user.FullName,
                                   FeedbackId = feedback.Id
                               };

            var resultList = feedbackview.ToList();

            if (resultList.Count == 0)
            {
                MessageBox.Show("There are no comments for this product.",
                              "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            dgUserOrders.ItemsSource = resultList;
            lbProductTitle.Content = productService.GetProductById(currentproductId).Title;
            lbproductid.Content = currentproductId.ToString();
        }

        private void btnfeedback_Click(object sender, RoutedEventArgs e)
        {

            string feedbackText = txtfeedback.Text.Trim();

            if (string.IsNullOrEmpty(feedbackText))
            {
                MessageBox.Show("Please enter a comment.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (currentSelectedItem != null)
                {

                    int feedbackId = currentSelectedItem.FeedbackId;

                    bool updated = feedbackService.UpdateFeedback(feedbackId, feedbackText);
                    Feedback existingfeedback= _context.Feedbacks.Find(feedbackId);
                    if (existingfeedback != null && existingfeedback.UserId != currentUserId && currentUserId != 1)
                    {
                        MessageBox.Show("You can only update your own comments.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (updated)
                    {
                        MessageBox.Show("Feedback updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to update feedback.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    Feedback feedbacknew = new Feedback
                    {
                        UserId = currentUserId,
                        ProductId = currentproductId,
                        Comment = feedbackText
                    };
                    bool added = feedbackService.AddFeedbackboo(feedbacknew);

                    if (added)
                    {
                        MessageBox.Show("Feedback added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Failed to add feedback.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }


                LoadData();


                txtfeedback.Text = string.Empty;
                currentSelectedItem = null;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        public bool UpdateFeedback(int feedbackId, string newComment)
        {

            try
            {

                var feedback = _context.Feedbacks.Find(feedbackId);
               
                if (feedback != null)
                {
                    feedback.Comment = newComment;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool AddFeedback(int userId, int productId, string comment)
        {
            try
            {

                var feedback = new Feedback
                {
                    UserId = userId,
                    ProductId = productId,
                    Comment = comment,

                };

                _context.Feedbacks.Add(feedback);
                _context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }


        private void dgUserOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (dgUserOrders.SelectedItem != null)
            {

                currentSelectedItem = dgUserOrders.SelectedItem;

                try
                {

                    string comment = currentSelectedItem.Comment;
                    txtfeedback.Text = comment;
                }
                catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException)
                {

                    txtfeedback.Text = string.Empty;
                }
            }
        }
    }
}
