using Application.Commands.CreateCurrentAccount;
using Application.Commands.CreateDeposit;
using Application.Queries.AvailableBalance;
using Application.Queries.Extract;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Net;
using System.Numerics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace API.Controllers
{
    [ApiController]
    [Route("v1/current-account")]
    public class CurrentAccountController : BaseController
    {
        private readonly IMediator _mediator;

        public CurrentAccountController(IMediator mediator) 
        { 
            _mediator = mediator;
        }

        //Criar uma conta corrente
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCurrentAccount([FromBody] CreateCurrentAccountCommand command)
        {
            var response = await _mediator.Send(command);
            return response.HasError ? HandleError(response.Error) : CreatedAtAction(nameof(GetAvailableBalance), new { id = response.Data }, response.Data);
        } 

        //Realizar um depósito
        [HttpPost("{id}/deposit")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Deposit([FromRoute] string id, [FromBody] decimal amount)
        {
            var response = await _mediator.Send(new CreateDepositCommand(id, amount));
            return response.HasError ? HandleError(response.Error) : CreatedAtAction(nameof(GetAccountExtract), new GetExtractByFiltersQuery { Id = response.Data, Account_Id = id, Type = null, InitialDate = null, EndDate = null }, response.Data);
        }

        //Realizar um saque
        [HttpPost("{id}/withdrawal")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Withdrawal([FromRoute] string id, [FromBody] decimal amount)
        {
            var response = await _mediator.Send(new CreateWithdrawCommand(id, amount));
            return response.HasError ? HandleError(response.Error) : CreatedAtAction(nameof(GetAccountExtract), new GetExtractByFiltersQuery{ Id = response.Data, Account_Id = id, Type = null, InitialDate = null, EndDate = null }, response.Data);
        }

        //Realizar uma transferência entre contas
        [HttpPost("{sourceId}/transfer/{destinationId}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AccountTransfer([FromRoute] string sourceId, [FromRoute] string destinationId, [FromBody] decimal amount)
        {
            var response = await _mediator.Send(new CreateTransferCommand(sourceId, amount, destinationId));
            return response.HasError ? HandleError(response.Error) : CreatedAtAction(nameof(GetAccountExtract), new GetExtractByFiltersQuery { Id = response.Data, Account_Id = sourceId, Type = null, InitialDate = null, EndDate = null }, response.Data);
        }

        //Consultar o saldo da conta
        [HttpGet("{id}/available-balance")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAvailableBalance([FromRoute] string id)
        {
            var response = await _mediator.Send(new GetAvailableBalanceQuery(id));
            return response.HasError ? HandleError(response.Error) : Ok(response.Data);
        }

        //Consultar o extrato da conta por período e tipo de operação (crédito ou débito)
        [HttpGet("extract")]
        [ProducesResponseType(typeof(IEnumerable<Movements>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAccountExtract([FromQuery] GetExtractByFiltersQuery filter)
        {
            var response = await _mediator.Send(filter);
            return response.HasError ? HandleError(response.Error) : Ok(response.Data);
        }
    }
}
