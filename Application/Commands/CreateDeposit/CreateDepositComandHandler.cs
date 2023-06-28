using Application.Commands.CreateCurrentAccount;
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
    public class CreateDepositComandHandler : IRequestHandler<CreateDepositCommand, Response<string>>
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;
        public CreateDepositComandHandler(ICurrentAccountRepository currentAccountRepository)
            => _currentAccountRepository = currentAccountRepository;

        public async Task<Response<string>> Handle(CreateDepositCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return Response<string>.Fail(new Error("400", "Amount shouldn't be zero or less"));

            var account = await _currentAccountRepository.GetAccount(request.Account_Id, cancellationToken);
            if (account is null)
                return Response<string>.Fail(new Error("404", "Account was not found"));

            account = new Account(account.Id, account.Name, account.Balance + request.Amount, account.Limit);

            var movement = Movements.CreateDebit(request.Account_Id, request.Amount);

            await _currentAccountRepository.UpdateAccountBalanceAndLimit(account, cancellationToken);
            await _currentAccountRepository.AddMovement(movement, cancellationToken);

            return Response<string>.Success(movement.Id);
        }
    }
}
