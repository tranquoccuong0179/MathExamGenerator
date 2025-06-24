using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Transaction
{
    public class TransactionRespone
    {
        public Guid Id { get; set; }             
        public decimal Amount { get; set; }      
        public DateTime CreateAt { get; set; }    
        public Guid DepositId { get; set; }     
        public string DepositDescription { get; set; }
    }
}
