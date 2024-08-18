using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RecurrentTransactionMicroservice.Domain.Models;

namespace RecurrentTransactionMicroservice.Core.Services
{
    public class RecurringTransactionService : BackgroundService
    {
        private readonly RabbitMqProducerService _rabbitMqProducerService;
        private readonly IServiceProvider _serviceProvider;

        public RecurringTransactionService(IServiceProvider serviceProvider)
        {
            _rabbitMqProducerService = new RabbitMqProducerService();
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<RecurrentTransactionsDbContext>();
                    var recurrentTransactions = await GetRecurrentTransactionsFromDatabase(context);

                    foreach (var transaction in recurrentTransactions)
                    {
                        if (transaction.IsTransactionDue())
                        {
                            ExecuteTransaction(transaction);
                            transaction.LastTransactionDate = DateTime.UtcNow;
                            UpdateRecurrentTransactionInDatabase(context, transaction);
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(3600), stoppingToken); // Check every 1 hour
            }
        }

        private async Task<List<RecurrentTransaction>> GetRecurrentTransactionsFromDatabase(RecurrentTransactionsDbContext context)
        {
            return await context.RecurrentTransactions.ToListAsync();
        }

        private void ExecuteTransaction(RecurrentTransaction transaction)
        {
            _rabbitMqProducerService.SendMessage("transactionQueue", transaction);
        }

        private void UpdateRecurrentTransactionInDatabase(RecurrentTransactionsDbContext context, RecurrentTransaction transaction)
        {
            transaction.LastTransactionDate = DateTime.UtcNow;
            context.RecurrentTransactions.Update(transaction);
            context.SaveChanges();
        }
    }
}
