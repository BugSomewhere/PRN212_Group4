using PRN212_Group4.DAL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Group4.BLL
{
    public class FeedbackServices
    {
        private PrnGroupProjectContext repo = new();

        public List<Feedback> GetAllFeedbacks()
        {
            return repo.Feedbacks.ToList();
        }

        public Feedback GetFeedbackById(int id)
        {
            return repo.Feedbacks.Find(id);
        }

        public List<Feedback> GetFeedbacksByUserId(int userId)
        {
            return repo.Feedbacks.Where(f => f.UserId == userId).ToList();
        }

        public void AddFeedback(Feedback feedback)
        {
            repo.Feedbacks.Add(feedback);
            repo.SaveChanges();
        }

        public Boolean AddFeedbackboo(Feedback feedback)
        {

            try
            {
                repo.Feedbacks.Add(feedback);
                repo.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Feedback> GetFeedbacksByProductId(int productId)
        {
            return repo.Feedbacks.Where(f => f.ProductId == productId).ToList();
        }

        public Boolean UpdateFeedback(int feedbackid, string newcmt)
        {
            try
            {
                var existingFeedback = repo.Feedbacks.Find(feedbackid);
                if (existingFeedback != null)
                {
                    existingFeedback.Comment = newcmt;
                    existingFeedback.UserId = existingFeedback.UserId;
                    existingFeedback.ProductId = existingFeedback.ProductId;
                    repo.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;

            }
        }



    }
}
