using PRN212_Group4.DAL;
using PRN212_Group4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRN212_Group4.BLL
{
    public class TransactionDetailsServices
    {
        private PrnGroupProjectContext repo = new();

        public List<TransactionDetail> GetAllTransactionDetails()
        {
            return repo.TransactionDetails.ToList();
        }

        public TransactionDetail? GetTransactionDetailById(int id)
        {
            return repo.TransactionDetails.FirstOrDefault(td => td.Id == id);
        }

        public List<TransactionDetail> GetTransactionDetailsByOrderId(int orderId)
        {
            return repo.TransactionDetails.Where(td => td.OrderId == orderId).ToList();
        }
    }
}
