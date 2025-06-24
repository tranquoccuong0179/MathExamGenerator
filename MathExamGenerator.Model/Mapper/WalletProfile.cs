using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Payload.Response.Transaction;
using MathExamGenerator.Model.Payload.Response.Wallet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Model.Mapper
{
    public class WalletProfile : Profile
    {

        public WalletProfile() {

            //respone
            CreateMap<Wallet, WalletResponse>();
            CreateMap<Transaction, TransactionRespone>();
        }
    }
}
