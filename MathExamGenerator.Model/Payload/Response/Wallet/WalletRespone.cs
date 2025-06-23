using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Wallet
{
    public class WalletRespone
    {
        public Guid Id { get; set; }              // Mã ví
        public Guid AccountId { get; set; }       // Mã tài khoản
        public int Point { get; set; }            // Số điểm (tiền) hiện có
        public bool IsActive { get; set; }        // Cờ hoạt động
        public DateTime CreateAt { get; set; }    // Thời điểm tạo
    }
}
