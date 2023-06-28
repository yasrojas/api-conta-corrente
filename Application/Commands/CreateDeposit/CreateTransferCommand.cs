using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.CreateDeposit
{
    public class CreateTransferCommand : IRequest<Response<string>>
    {
        public string SourceAccount { get; set; }
        public decimal Amount { get; set; }
        public string DestinationAccount { get; set; }

        public CreateTransferCommand(string sourceAccount, decimal amount, string destinationAccount)
        {
            SourceAccount = sourceAccount;
            Amount = amount;
            DestinationAccount = destinationAccount;
        }
    }
}
