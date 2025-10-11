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
    }
}
