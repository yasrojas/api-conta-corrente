using Application.Queries.AvailableBalance;
using Domain;
using Domain.Abstractions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Extract
{
    public class GetExtractByFiltersCommandHandler : IRequestHandler<GetExtractByFiltersQuery, Response<IEnumerable<Movements>>>
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;

        public GetExtractByFiltersCommandHandler(ICurrentAccountRepository currentAccountRepository)
            => _currentAccountRepository = currentAccountRepository;

        public async Task<Response<IEnumerable<Movements>>> Handle(GetExtractByFiltersQuery request, CancellationToken cancellationToken)
        {
            var movements = _currentAccountRepository.GetMovementsByFilter(request);

            return movements.Any() ? Response<IEnumerable<Movements>>.Success(movements) : Response<IEnumerable<Movements>>.Fail(new Error("404", "Movements were not found for this account")) ;
        }
    }
}
