using PRN212_Group4.DAL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Group4.BLL
{
    public class UserOrders
    {
        private PrnGroupProjectContext repo = new();
        public Order? SearchOrders(int user_id, int product_id)
        {
            List<Order> allorders = this.GetAllOrders();
            for (int i = 0; i < allorders.Count; i++)
            {
                if (allorders[i].BuyerId == user_id && allorders[i].ProductId == product_id)
                {
                    return allorders[i];
                }
            }
            return null;
        }

        public List<Order> GetOrdersByUserId(int user_id)
        {
            List<Order> allorders = this.GetAllOrders();
            List<Order> user_orders = new List<Order>();
            for (int i = 0; i < allorders.Count; i++)
            {
                if (allorders[i].BuyerId == user_id)
                {
                    user_orders.Add(allorders[i]);
                }
            }
            return user_orders;
        }

        public List<Order> GetAllOrders()
        {
            return repo.Orders.ToList();
        }
    }
}
