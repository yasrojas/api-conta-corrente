using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateCurrentAccount
{
    public class CreateCurrentAccountCommand : IRequest<CreateCurrentAccountResponse>
    {
        public string Name { get; set; }
    }
}
