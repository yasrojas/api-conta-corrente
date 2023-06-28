using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.Extract
{
    public class GetExtractByFiltersQuery : MovementsFilter, IRequest<Response<IEnumerable<Movements>>>
    {
    }
}
