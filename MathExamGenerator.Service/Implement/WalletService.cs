using AutoMapper;
using MathExamGenerator.Model.Entity;
using MathExamGenerator.Repository.Interface;
using MathExamGenerator.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class WalletService : BaseService<WalletService>, IWalletService
    {
        public WalletService(IUnitOfWork<MathExamGeneratorContext> unitOfWork, ILogger<WalletService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(unitOfWork, logger, mapper, httpContextAccessor)
        {
        }
    }
}
