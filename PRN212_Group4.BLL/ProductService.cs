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
        public void AddProduct(DAL.Entities.Product product)
        {
            repo.Products.Add(product);
            repo.SaveChanges();
        }
    }

}
