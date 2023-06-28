using Application.Commands.CreateDeposit;
using Domain;
using Domain.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Applications
{
    public class CreateTransferCommandHandlerTest
    {
        private readonly Mock<ICurrentAccountRepository> _repository;
        private readonly CreateTransferCommandHandler _handler;

        public CreateTransferCommandHandlerTest()
        {
            _repository = new Mock<ICurrentAccountRepository>();
            _handler = new CreateTransferCommandHandler(_repository.Object);
            _repository.Setup(x => x.UpdateAccountBalanceAndLimit(It.IsAny<Account>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _repository.Setup(x => x.AddMovement(It.IsAny<Movements>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        }

        [Fact(DisplayName = "Create transfer between accounts should not have error when all params are specified, accounts exists and source account has enought balance/limit to transfer")]
        public async void CreateTransferCommandHandler_ShouldReturnSucces()
        {
            _repository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Domain.Account("1", "Name", 1000, 1000));
            var result = await _handler.Handle(new CreateTransferCommand("1", 1000, "2"), new CancellationToken());

            Assert.False(result.HasError);
        }

        [Fact(DisplayName = "Create transfer between accounts should return error when amount to tranfer is zero or less")]
        public async void CreateTransferCommandHandler_ShouldReturnFail_AmountEqualsZero()
        {
            var result = await _handler.Handle(new CreateTransferCommand("1", 0, "2"), new CancellationToken());

            Assert.True(result.HasError);
        }

        [Fact(DisplayName = "Create transfer between accounts should return error when source account is not found")]
        public async void CreateTransferCommandHandler_ShouldReturnFail_SourceAccountNotFound()
        {
            _repository.Setup(x => x.GetAccount(It.Is<string>(y => y == "1"), It.IsAny<CancellationToken>())).ReturnsAsync(default(Domain.Account));
            _repository.Setup(x => x.GetAccount(It.Is<string>(y => y == "2"), It.IsAny<CancellationToken>())).ReturnsAsync(new Domain.Account("1", "Name", 1000, 1000));
            var result = await _handler.Handle(new CreateTransferCommand("1", 1000, "2"), new CancellationToken());

            Assert.True(result.HasError);
        }

        [Fact(DisplayName = "Create transfer between accounts should return error when destination account is not found")]
        public async void CreateTransferCommandHandler_ShouldReturnFail_DestinationAccountNotFound()
        {
            _repository.Setup(x => x.GetAccount(It.Is<string>(y => y == "1"), It.IsAny<CancellationToken>())).ReturnsAsync(new Domain.Account("1", "Name", 1000, 1000));
            _repository.Setup(x => x.GetAccount(It.Is<string>(y => y == "2"), It.IsAny<CancellationToken>())).ReturnsAsync(default(Domain.Account));
            var result = await _handler.Handle(new CreateTransferCommand("1", 1000, "2"), new CancellationToken());

            Assert.True(result.HasError);
        }

        [Fact(DisplayName = "Create transfer between accounts should return error when source account doesn't have enough balance and limit to support transfer")]
        public async void CreateTransferCommandHandler_ShouldReturnFail_SourceAccountWithoutEnoughBalance()
        {
            _repository.Setup(x => x.GetAccount(It.Is<string>(y => y == "1"), It.IsAny<CancellationToken>())).ReturnsAsync(new Domain.Account("1", "Name", 1000, 1000));
            _repository.Setup(x => x.GetAccount(It.Is<string>(y => y == "2"), It.IsAny<CancellationToken>())).ReturnsAsync(new Domain.Account("2", "Name", 1000, 1000));
            var result = await _handler.Handle(new CreateTransferCommand("1", 100000, "2"), new CancellationToken());

            Assert.True(result.HasError);
        }
    }
}
