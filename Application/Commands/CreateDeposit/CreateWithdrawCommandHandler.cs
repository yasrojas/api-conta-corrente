using Domain;
using Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateDeposit
{
    public class CreateWithdrawCommandHandler : IRequestHandler<CreateWithdrawCommand, Response<string>>
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;
        public CreateWithdrawCommandHandler(ICurrentAccountRepository currentAccountRepository)
            => _currentAccountRepository = currentAccountRepository;

        public async Task<Response<string>> Handle(CreateWithdrawCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return Response<string>.Fail(new Error("400", "Amount shouldn't be zero or less"));

            var account = await _currentAccountRepository.GetAccount(request.Account_Id, cancellationToken);

            if (account is null)
                return Response<string>.Fail(new Error("404", "Account was not found"));
            if (request.Amount > (account.Balance + account.Limit))
                return Response<string>.Fail(new Error("400", "Account doesn't have enough to complete transfer"));

            account = Account.UpdateBalanceWithTransaction(account, request.Amount);

            var accountMovement = Movements.CreateCredit(request.Account_Id, request.Amount);

            await _currentAccountRepository.UpdateAccountBalanceAndLimit(account, cancellationToken);

            await _currentAccountRepository.AddMovement(accountMovement, cancellationToken);

            return Response<string>.Success(accountMovement.Id);
        }
    }
}
