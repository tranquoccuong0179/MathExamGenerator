using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Payload.Response.Wallet
{
    public class WalletResponse
    {
        public Guid Id { get; set; }              
        //public Guid AccountId { get; set; }      
        public int Point { get; set; }            
        public bool IsActive { get; set; }        
        public DateTime CreateAt { get; set; }    
    }
}
