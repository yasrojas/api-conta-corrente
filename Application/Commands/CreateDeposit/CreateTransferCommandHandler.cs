using Domain;
using Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateDeposit
{
    public class CreateTransferCommandHandler : IRequestHandler<CreateTransferCommand, Response<string>>
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;
        public CreateTransferCommandHandler(ICurrentAccountRepository currentAccountRepository)
            => _currentAccountRepository = currentAccountRepository;

        public async Task<Response<string>> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
        {
            if (request.Amount <= 0)
                return Response<string>.Fail(new Error("400", "Amount shouldn't be zero or less"));
            
            var sourceAccount = await _currentAccountRepository.GetAccount(request.SourceAccount, cancellationToken);
            var destinationAccount = await _currentAccountRepository.GetAccount(request.DestinationAccount, cancellationToken);
            
            if (sourceAccount is null)
                return Response<string>.Fail(new Error("404", "Source account was not found"));
            if (destinationAccount is null)
                return Response<string>.Fail(new Error("404", "Destination account was not found"));
            if (request.Amount > (sourceAccount.Balance + sourceAccount.Limit))
                return Response<string>.Fail(new Error("400", "Source account doesn't have enough to complete transfer"));

            sourceAccount = Account.UpdateBalanceWithTransaction(sourceAccount, request.Amount);
     
            destinationAccount = new Account(destinationAccount.Id, destinationAccount.Name, destinationAccount.Balance + request.Amount, destinationAccount.Limit);
            
            var sourceAccountMovement = Movements.CreateCredit(request.SourceAccount, request.Amount);
            var destinationAccountMovement = Movements.CreateDebit(request.DestinationAccount, request.Amount);

            await _currentAccountRepository.UpdateAccountBalanceAndLimit(sourceAccount, cancellationToken);
            await _currentAccountRepository.UpdateAccountBalanceAndLimit(destinationAccount, cancellationToken);

            await _currentAccountRepository.AddMovement(sourceAccountMovement, cancellationToken);
            await _currentAccountRepository.AddMovement(destinationAccountMovement, cancellationToken);

            return Response<string>.Success(sourceAccountMovement.Id);
        }


    }
}
