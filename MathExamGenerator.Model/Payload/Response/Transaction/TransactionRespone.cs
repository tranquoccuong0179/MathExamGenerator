using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Transaction
{
    public class TransactionRespone
    {
        public Guid Id { get; set; }              // Mã giao dịch
        public decimal Amount { get; set; }       // Số tiền giao dịch
        public DateTime CreateAt { get; set; }    // Ngày giao dịch
        public Guid DepositId { get; set; }       // Mã deposit liên quan (nạp tiền)
    }
}
