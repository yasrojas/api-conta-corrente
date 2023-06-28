using Application.Commands.CreateCurrentAccount;
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
    public class CreateCurrentAccountCommandHandlerTest
    {
        private readonly Mock<ICurrentAccountRepository> _repository;
        private readonly CreateCurrentAccountCommandHandler _handler;

        public CreateCurrentAccountCommandHandlerTest() 
        { 
            _repository = new Mock<ICurrentAccountRepository>();
            _repository.Setup(x => x.AddAccount(It.IsAny<Account>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _handler = new CreateCurrentAccountCommandHandler(_repository.Object);
        }

        [Fact(DisplayName = "Create current account request with name should not have error")]
        public async void CreateCurrentAccountCommandHandler_ShouldReturnSucces()
        {
            var result = await _handler.Handle(new CreateCurrentAccountCommand() { Name = "name test" }, new CancellationToken());

            Assert.False(result.HasError);
        }

        [Fact(DisplayName = "Create current account request should have error when name is not specified")]
        public async void CreateCurrentAccountCommandHandler_ShouldReturnFail()
        {
            var result = await _handler.Handle(new CreateCurrentAccountCommand(), new CancellationToken());

            Assert.True(result.HasError);
        }
    }
}
