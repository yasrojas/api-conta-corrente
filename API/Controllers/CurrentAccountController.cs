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

        /// <summary>
        /// Create an account
        /// </summary>
        /// <param name="command">Object with name of desired account</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateCurrentAccount([FromBody] CreateCurrentAccountCommand command)
        {
            var response = await _mediator.Send(command);
            return response.HasError ? HandleError(response.Error) : CreatedAtAction(nameof(GetAvailableBalance), new { id = response.Data }, response.Data);
        } 

        /// <summary>
        /// Deposit to an account
        /// </summary>
        /// <param name="id">Account identifier</param>
        /// <param name="amount">Value to deposit</param>
        /// <returns></returns>
        [HttpPost("{id}/deposit")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Deposit([FromRoute] string id, [FromBody] decimal amount)
        {
            var response = await _mediator.Send(new CreateDepositCommand(id, amount));
            return response.HasError ? HandleError(response.Error) : CreatedAtAction(nameof(GetAccountExtract), new GetExtractByFiltersQuery { Id = response.Data, Account_Id = id, Type = null, InitialDate = null, EndDate = null }, response.Data);
        }

        /// <summary>
        /// Withdraw of an account
        /// </summary>
        /// <param name="id">Account identifier</param>
        /// <param name="amount">Value to withdraw</param>
        /// <returns></returns>
        [HttpPost("{id}/withdraw")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Withdraw([FromRoute] string id, [FromBody] decimal amount)
        {
            var response = await _mediator.Send(new CreateWithdrawCommand(id, amount));
            return response.HasError ? HandleError(response.Error) : CreatedAtAction(nameof(GetAccountExtract), new GetExtractByFiltersQuery{ Id = response.Data, Account_Id = id, Type = null, InitialDate = null, EndDate = null }, response.Data);
        }

        /// <summary>
        /// Transfer value between accounts
        /// </summary>
        /// <param name="sourceId">Source account identifier</param>
        /// <param name="destinationId">Destination account identifier</param>
        /// <param name="amount">Value to transfer</param>
        /// <returns></returns>
        [HttpPost("{sourceId}/transfer/{destinationId}")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> AccountTransfer([FromRoute] string sourceId, [FromRoute] string destinationId, [FromBody] decimal amount)
        {
            var response = await _mediator.Send(new CreateTransferCommand(sourceId, amount, destinationId));
            return response.HasError ? HandleError(response.Error) : CreatedAtAction(nameof(GetAccountExtract), new GetExtractByFiltersQuery { Id = response.Data, Account_Id = sourceId, Type = null, InitialDate = null, EndDate = null }, response.Data);
        }

        /// <summary>
        /// Obtain available balance of account
        /// </summary>
        /// <param name="id">Account identifier</param>
        /// <returns></returns>
        [HttpGet("{id}/available-balance")]
        [ProducesResponseType(typeof(decimal), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAvailableBalance([FromRoute] string id)
        {
            var response = await _mediator.Send(new GetAvailableBalanceQuery(id));
            return response.HasError ? HandleError(response.Error) : Ok(response.Data);
        }

        /// <summary>
        /// Obtain extract with movements per account with optional filter, such as initial date, movement identifier, end date and type(credit or debit)
        /// </summary>
        /// <param name="filter">Movements filter</param>
        /// <returns></returns>
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
