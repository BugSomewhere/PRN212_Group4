using PRN212_Group4.DAL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Group4.BLL
{
    public class OrderService
    {
        private PrnGroupProjectContext repo = new();

        public void CreateOrder(int buyerId, int productId, decimal price)
        {
            var order = new Order { Status = "Pending", Price = price, ProductId = productId, BuyerId = buyerId };
            repo.Orders.Add(order);
            repo.SaveChanges();

            // Tạo TransactionDetails
            var tdReduce = new TransactionDetail { OrderId = order.Id, Type = "Giảm", Amount = -price };
            var tdIncrease = new TransactionDetail { OrderId = order.Id, Type = "Tăng", Amount = price };

            repo.TransactionDetails.AddRange(tdReduce, tdIncrease);
            repo.SaveChanges();

            // Update TotalCredit (giả sử trừ credit)
            var user = repo.Users.Find(buyerId);
            if (user != null) user.TotalCredit -= price;
            repo.SaveChanges();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = repo.Orders.Find(orderId);
            if (order != null) order.Status = status;
            repo.SaveChanges();
        }

        public List<Order> GetAllOrders()
        {
            return repo.Orders.ToList();
        }
    }
}
