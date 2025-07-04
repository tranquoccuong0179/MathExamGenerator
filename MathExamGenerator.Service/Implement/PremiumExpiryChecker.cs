using MathExamGenerator.Model.Entity;
using MathExamGenerator.Model.Utils;
using MathExamGenerator.Repository.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MathExamGenerator.Service.Implement
{
    public class PremiumExpiryChecker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PremiumExpiryChecker> _logger;

        public PremiumExpiryChecker(IServiceScopeFactory scopeFactory, ILogger<PremiumExpiryChecker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    try
                    {
                        var unitOfWork = scope.ServiceProvider
                            .GetRequiredService<IUnitOfWork<MathExamGeneratorContext>>();

                        var accounts = await unitOfWork.GetRepository<Account>().GetListAsync(
                            predicate: a => a.IsPremium == true && a.PremiumExpireAt < TimeUtil.GetCurrentSEATime());

                        foreach (var acc in accounts)
                            acc.IsPremium = false;

                        if (accounts.Any())
                        {
                            unitOfWork.GetRepository<Account>().UpdateRange(accounts);
                            await unitOfWork.CommitAsync();
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error while checking premium expiry");
                    }
                }

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken); 
            }
        }
    }
}
