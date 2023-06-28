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
    public class CreateDepositCommand : IRequest<Response<string>>
    {
        public string Account_Id { get; set; }
        public decimal Amount { get; set; }

        public CreateDepositCommand(string account_id, decimal amount)
        {
            Account_Id = account_id;
            Amount = amount;
        }
    }
}
