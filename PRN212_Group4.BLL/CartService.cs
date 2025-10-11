using PRN212_Group4.DAL.Entities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PRN212_Group4.BLL
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => (Product.Price ?? 0) * Quantity;
    }

    public class CartService
    {
        private ObservableCollection<CartItem> _items = new();
        public ObservableCollection<CartItem> GetCartItems() => _items;

        public void AddToCart(Product product, int quantity)
        {
            var existing = _items.FirstOrDefault(i => i.Product.Id == product.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                _items.Add(new CartItem { Product = product, Quantity = quantity });
            }
        }


        public void ClearCart() => _items.Clear();

        public decimal GetTotalPrice() => _items.Sum(i => i.TotalPrice);
        public void RemoveFromCart(int productId)
        {
            var item = _items.FirstOrDefault(i => i.Product.Id == productId);
            if (item != null)
            {
                _items.Remove(item);
            }
        }
    }
}
