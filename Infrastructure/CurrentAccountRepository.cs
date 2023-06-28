using Domain;
using Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Infrastructure
{
    public class CurrentAccountRepository : ICurrentAccountRepository
    {
        private readonly ICurrentAccountDbContext _dbContext;
        private readonly ILogger<CurrentAccountRepository> _logger;

        public CurrentAccountRepository(CurrentAccountDbContext dbContext, ILogger<CurrentAccountRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        //Account
        public async Task AddAccount(Account account, CancellationToken cancellationToken)
        {
            try
            {
                //var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
                var entity = _dbContext.Account.Add(account);
                var count = _dbContext.SaveChanges();
                //await transaction.CommitAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Ocorreu um erro ao inserir uma conta no banco de dados");
            }
        }

        public async Task<Account?> GetAccount(string id, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Account.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);
            return account;
        }

        public async Task<int> UpdateAccountBalanceAndLimit(Account account, CancellationToken cancellationToken)
        {
            var updatedRows = await _dbContext.Account.Where(x => x.Id == account.Id).ExecuteUpdateAsync(x => x.SetProperty(property => property.Balance, account.Balance).SetProperty(property => property.Limit, account.Limit), cancellationToken);
            return updatedRows;
        }

        public async Task AddMovement(Movements movements, CancellationToken cancellationToken)
        {
            var entity = await _dbContext.Movements.AddAsync(movements, cancellationToken);
            var count = await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public IEnumerable<Movements> GetMovementsByFilter(MovementsFilter filter)
        {
            Expression<Func<Movements, bool>> movementsFilter = m =>
            (!string.IsNullOrEmpty(filter.Id) ? m.Id.Equals(filter.Id) : true) &&
            (filter.InitialDate.HasValue ? m.Date >= filter.InitialDate.Value : true) &&
            (filter.EndDate.HasValue ? m.Date >= filter.EndDate.Value : true) &&
            (filter.Type.HasValue ? m.Type.Equals(filter.Type.Value) : true);

            return _dbContext.Movements
                .Where(movementsFilter);
        }
    }
}
