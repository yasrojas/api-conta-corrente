using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.AvailableBalance
{
    public class GetAvailableBalanceQuery : IRequest<Response<decimal>>
    {
        public string Id { get; set; }
        public GetAvailableBalanceQuery(string id) => Id = id;
    }
}
