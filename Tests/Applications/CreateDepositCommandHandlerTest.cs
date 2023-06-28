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
    public class CreateDepositCommandHandlerTest
    {
        private readonly Mock<ICurrentAccountRepository> _repository;
        private readonly CreateDepositComandHandler _handler;

        public CreateDepositCommandHandlerTest()
        {
            _repository = new Mock<ICurrentAccountRepository>();
            _handler = new CreateDepositComandHandler(_repository.Object);
            _repository.Setup(x => x.UpdateAccountBalanceAndLimit(It.IsAny<Account>(), It.IsAny<CancellationToken>())).ReturnsAsync(1);
            _repository.Setup(x => x.AddMovement(It.IsAny<Movements>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        }

        [Fact(DisplayName = "Create deposit should not return error when amount is more than zero and account exists")]
        public async void CreateDepositCommandHandler_ShouldReturnSucces()
        {
            _repository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Domain.Account("1", "Name", 1000, 1000));
            var result = await _handler.Handle(new CreateDepositCommand("1", 1000), new CancellationToken());

            Assert.False(result.HasError);
        }

        [Fact(DisplayName = "Creae deposit should fail when amount is zero or less")]
        public async void CreateDepositCommandHandler_ShouldReturnFail_AmountEqualsZero()
        {
            var result = await _handler.Handle(new CreateDepositCommand("1", 0), new CancellationToken());

            Assert.True(result.HasError);
        }

        [Fact(DisplayName = "Create deposit should return error when account is not found")]
        public async void CreateDepositCommandHandler_ShouldReturnFail_SourceAccountNotFound()
        {
            _repository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Domain.Account));
            var result = await _handler.Handle(new CreateDepositCommand("1", 1000), new CancellationToken());

            Assert.True(result.HasError);
        }
    }
}
