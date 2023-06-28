using Domain;
using Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.AvailableBalance
{
    public class GetAvailableBalanceCommandHandler : IRequestHandler<GetAvailableBalanceQuery, Response<decimal>>
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;

        public GetAvailableBalanceCommandHandler(ICurrentAccountRepository currentAccountRepository)
            => _currentAccountRepository = currentAccountRepository;

        public async Task<Response<decimal>> Handle(GetAvailableBalanceQuery request, CancellationToken cancellationToken)
        {
            var account = await _currentAccountRepository.GetAccount(request.Id, cancellationToken);

            return account is null ? Response<decimal>.Fail(new Error("404", "Account was not found")) : Response<decimal>.Success(account.Balance);
        }
    }
}

