using Cqrs.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateCurrentAccount
{
    public class CreateCurrentAccountCommandHandler : IRequestHandler<CreateCurrentAccountCommand, CreateCurrentAccountResponse>
    {
        public Task<CreateCurrentAccountResponse> Handle(CreateCurrentAccountCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
