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
    public class CreateWithdrawCommandHandlerTest
    {
        private readonly Mock<ICurrentAccountRepository> _repository;
        private readonly CreateWithdrawCommandHandler _handler;

        public CreateWithdrawCommandHandlerTest()
        {
            _repository = new Mock<ICurrentAccountRepository>();
            _handler = new CreateWithdrawCommandHandler(_repository.Object);
            _repository.Setup(x => x.UpdateAccountBalanceAndLimit(It.IsAny<Account>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _repository.Setup(x => x.AddMovement(It.IsAny<Movements>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        }

        [Fact(DisplayName = "Create withdraw should not return error when amount is more than zero, account exists and has enough balance")]
        public async void CreateWithdrawCommandHandler_ShouldReturnSucces()
        {
            _repository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Domain.Account("1", "Name", 1000, 1000));
            var result = await _handler.Handle(new CreateWithdrawCommand("1", 1000), new CancellationToken());

            Assert.False(result.HasError);
        }

        [Fact(DisplayName = "Create withdraw should fail when amount is zero or less")]
        public async void CreateDepositCommandHandler_ShouldReturnFail_AmountEqualsZero()
        {
            var result = await _handler.Handle(new CreateWithdrawCommand("1", 0), new CancellationToken());

            Assert.True(result.HasError);
        }

        [Fact(DisplayName = "Create withdraw should return error when account is not found")]
        public async void CreateDepositCommandHandler_ShouldReturnFail_SourceAccountNotFound()
        {
            _repository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Domain.Account));
            var result = await _handler.Handle(new CreateWithdrawCommand("1", 1000), new CancellationToken());

            Assert.True(result.HasError);
        }

        [Fact(DisplayName = "Create withdraw should return error when account doesn't have enough balance")]
        public async void CreateDepositCommandHandler_ShouldReturnFail_SourceAccountWithoutEnoughBalance()
        {
            _repository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Domain.Account("1", "Name", 1000, 1000));
            var result = await _handler.Handle(new CreateWithdrawCommand("1", 100000), new CancellationToken());

            Assert.True(result.HasError);
        }
    }
}
