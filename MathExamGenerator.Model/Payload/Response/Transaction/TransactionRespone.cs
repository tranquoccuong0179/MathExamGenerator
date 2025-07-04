﻿using System;
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
        public string Description { get; set; }
        public DateTime CreateAt { get; set; }    
        public string DepositDescription { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}
