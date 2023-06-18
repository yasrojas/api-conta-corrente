using Application.Commands.CreateCurrentAccount;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CurrentAccountController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CurrentAccountController(IMediator mediator) 
        { 
            _mediator = mediator;
        }

        //Criar uma conta corrente
        [HttpPost]
        public async Task<IActionResult> CreateCurrentAccount([FromBody] CreateCurrentAccountCommand command)
        {
           CreateCurrentAccountResponse response = await _mediator.Send(command);
           return response is null ? BadRequest() : Created(nameof(CreateCurrentAccount), command);
        } 

        //Realizar um depósito
        [HttpPost("deposit")]
        public IActionResult Deposit()
        {
            throw new NotImplementedException();
        }

        //Realizar um saque
        [HttpPost("withdrawal")]
        public IActionResult Withdrawal()
        {
            throw new NotImplementedException();
        }

        //Realizar uma transferência entre contas
        [HttpPost("transfer")]
        public IActionResult AccountTransfer()
        {
            throw new NotImplementedException();
        }

        //Consultar o saldo da conta
        [HttpGet("availableBalance")]
        public IActionResult GetAvailableBalance()
        {
            throw new NotImplementedException();
        }

        //Consultar o extrato da conta por período
        [HttpGet("extract")]
        public IActionResult GetAccountExtractByPeriod()
        {
            throw new NotImplementedException();
        }

        //Consultar o extrato da conta por tipo de operação (crédito ou débito)
        [HttpGet("extractByType")]
        public IActionResult GetAccountExtractByType()
        {
            throw new NotImplementedException();
        }
    }
}
