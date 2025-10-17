using PRN212_Group4.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Group4.BLL
{
    public class ProductService
    {
        private PrnGroupProjectContext repo = new();

        public List<DAL.Entities.Product> GetAllProducts()
        {
            return repo.Products.ToList();
        }

        public List<DAL.Entities.Product> SearchProducts(string keyword)
        {
            // Tìm kiếm sản phẩm theo title
            return repo.Products
                       .Where(p => p.Title.Contains(keyword))
                       .ToList();
        }
        public DAL.Entities.Product? GetProductById(int id)
        {
            return repo.Products.Find(id);
        }

        public void UpdateProduct(DAL.Entities.Product updatedProduct)
        {
            var product = repo.Products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (product != null)
            {
                product.Type = updatedProduct.Type;
                product.Status = updatedProduct.Status;
                product.Brand = updatedProduct.Brand;
                product.Model = updatedProduct.Model;
                product.Title = updatedProduct.Title;
                product.Description = updatedProduct.Description;
                product.Price = updatedProduct.Price;
                product.Color = updatedProduct.Color;
                product.Dimension = updatedProduct.Dimension;
                repo.SaveChanges();
            }
        }

        public void DeleteProduct(int id)
        {
            var product = repo.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                repo.Products.Remove(product);
                repo.SaveChanges();
            }
        }
    }
}
