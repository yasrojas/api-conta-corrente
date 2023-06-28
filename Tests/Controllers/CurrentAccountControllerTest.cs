using API.Controllers;
using Application.Commands.CreateCurrentAccount;
using Application.Commands.CreateDeposit;
using Application.Queries.AvailableBalance;
using Application.Queries.Extract;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Controllers
{
    public class CurrentAccountControllerTest
    {
        private readonly Mock<IMediator> _mediator;
        private readonly CurrentAccountController _controller;
        public CurrentAccountControllerTest()
        {
            _mediator = new Mock<IMediator>();
            _controller = new CurrentAccountController(_mediator.Object);
        }

        [Fact(DisplayName = "POST v1/current-account should return http status code 201 created")]
        public async void CreateCurrentAccount_ShouldReturnCreated()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateCurrentAccountCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Success(""));
            var response = await _controller.CreateCurrentAccount(new Application.Commands.CreateCurrentAccount.CreateCurrentAccountCommand());

            Assert.IsType<CreatedAtActionResult>(response);
        }

        [Fact(DisplayName = "POST v1/current-account should return http status code 400 bad request")]
        public async void CreateCurrentAccount_ShouldReturnBadRequest()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateCurrentAccountCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Fail(new Error("400", "")));
            var response = await _controller.CreateCurrentAccount(new Application.Commands.CreateCurrentAccount.CreateCurrentAccountCommand());

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact(DisplayName = "POST v1/{id}/deposit should return http status code 201 created")]
        public async void Deposit_ShouldReturnCreated()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateDepositCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Success(""));
            var response = await _controller.Deposit("1", 1000);

            Assert.IsType<CreatedAtActionResult>(response);
        }

        [Fact(DisplayName = "POST v1/{id}/deposit should return http status code 404 not found")]
        public async void Deposit_ShouldReturnNotFound()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateDepositCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Fail(new Error("404", "")));
            var response = await _controller.Deposit("1890", 1000);

            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact(DisplayName = "POST v1/{id}/deposit should return http status code 400 bad request")]
        public async void Deposit_ShouldReturnBadRequest()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateDepositCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Fail(new Error("400", "")));
            var response = await _controller.Deposit("1", 1000);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact(DisplayName = "POST v1/{id}/withdrawal should return http status code 201 created")]
        public async void Withdrawal_ShouldReturnCreated()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateWithdrawCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Success(""));
            var response = await _controller.Withdrawal("1", 1000);

            Assert.IsType<CreatedAtActionResult>(response);
        }

        [Fact(DisplayName = "POST v1/{id}/withdrawal should return http status code 404 not found")]
        public async void Withdrawal_ShouldReturnNotFound()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateWithdrawCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Fail(new Error("404", "")));
            var response = await _controller.Withdrawal("1", 1000);

            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact(DisplayName = "POST v1/{id}/withdrawal should return http status code 400 bad request")]
        public async void Withdrawal_ShouldReturnBadRequest()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateWithdrawCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Fail(new Error("400", "")));
            var response = await _controller.Withdrawal("1", 1000);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact(DisplayName = "POST v1/{sourceId}/transfer/{destinationId} should return http status code 201 created")]
        public async void Transfer_ShouldReturnCreated()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateTransferCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Success(""));
            var response = await _controller.AccountTransfer("1", "2", 1000);

            Assert.IsType<CreatedAtActionResult>(response);
        }

        [Fact(DisplayName = "POST v1/{sourceId}/transfer/{destinationId} should return http status code 404 not found")]
        public async void Transfer_ShouldReturnNotFound()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateTransferCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Fail(new Error("404", "")));
            var response = await _controller.AccountTransfer("1", "2", 1000);

            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact(DisplayName = "POST v1/{sourceId}/transfer/{destinationId} should return http status code 400 bad request")]
        public async void Transfer_ShouldReturnBadRequest()
        {
            _mediator.Setup(x => x.Send(It.IsAny<CreateTransferCommand>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<string>.Fail(new Error("400", "")));
            var response = await _controller.AccountTransfer("1", "2", 1000);

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact(DisplayName = "GET v1/{id}/available-balance should return http status code 200 ok")]
        public async void GetAvailableBalance_ShouldReturnOk()
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetAvailableBalanceQuery>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<decimal>.Success(new decimal(1000)));
            var response = await _controller.GetAvailableBalance("1");

            Assert.IsType<OkObjectResult>(response);
        }

        [Fact(DisplayName = "GET v1/{id}/available-balance  should return http status code 404 not found")]
        public async void GetAvailableBalance_ShouldReturnNotFound()
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetAvailableBalanceQuery>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<decimal>.Fail(new Error("404", "")));
            var response = await _controller.GetAvailableBalance("1");

            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact(DisplayName = "GET v1/{id}/available-balance should return http status code 400 bad request")]
        public async void GetAvailableBalance_ShouldReturnBadRequest()
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetAvailableBalanceQuery>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<decimal>.Fail(new Error("400", "")));
            var response = await _controller.GetAvailableBalance("1");

            Assert.IsType<BadRequestObjectResult>(response);
        }

        [Fact(DisplayName = "GET v1/{id}/extract should return http status code 200 ok")]
        public async void GetAccountExtract_ShouldReturnOk()
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetExtractByFiltersQuery>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<IEnumerable<Movements>>.Success(new List<Movements>()));
            var response = await _controller.GetAccountExtract(new GetExtractByFiltersQuery());

            Assert.IsType<OkObjectResult>(response);
        }

        [Fact(DisplayName = "GET v1/{id}/extract should return http status code 404 not found")]
        public async void GetAccountExtract_ShouldReturnNotFound()
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetExtractByFiltersQuery>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<IEnumerable<Movements>>.Fail(new Error("404", "")));
            var response = await _controller.GetAccountExtract(new GetExtractByFiltersQuery());

            Assert.IsType<NotFoundObjectResult>(response);
        }

        [Fact(DisplayName = "GET v1/{id}/extract should return http status code 400 bad request")]
        public async void GetAccountExtract_ShouldReturnBadRequest()
        {
            _mediator.Setup(x => x.Send(It.IsAny<GetExtractByFiltersQuery>(), It.IsAny<System.Threading.CancellationToken>())).ReturnsAsync(Domain.Response<IEnumerable<Movements>>.Fail(new Error("400", "")));
            var response = await _controller.GetAccountExtract(new GetExtractByFiltersQuery());

            Assert.IsType<BadRequestObjectResult>(response);
        }
    }
}
