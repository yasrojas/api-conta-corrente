using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstractions
{
    public interface ICurrentAccountRepository
    {
        Task AddAccount(Account account, CancellationToken cancellationToken);
        Task<Account?> GetAccount(string id, CancellationToken cancellationToken);
        Task<int> UpdateAccountBalanceAndLimit(Account account, CancellationToken cancellationToken);
        Task AddMovement(Movements movements, CancellationToken cancellationToken);
        IEnumerable<Movements> GetMovementsByFilter(MovementsFilter filter);
    }
}
