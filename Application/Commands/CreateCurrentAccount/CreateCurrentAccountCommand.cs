using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateCurrentAccount
{
    public class CreateCurrentAccountCommand : IRequest<Response<string>>
    {
        public string Name { get; set; }
    }
}
