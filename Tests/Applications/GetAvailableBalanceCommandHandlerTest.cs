using Application.Queries.Extract;
using Domain.Abstractions;
using Domain;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Queries.AvailableBalance;

namespace Tests.Applications
{
    public class GetAvailableBalanceCommandHandlerTest
    {
        private readonly Mock<ICurrentAccountRepository> _repository;
        private readonly GetAvailableBalanceCommandHandler _handler;

        public GetAvailableBalanceCommandHandlerTest()
        {
            _repository = new Mock<ICurrentAccountRepository>();
            _handler = new GetAvailableBalanceCommandHandler(_repository.Object);
        }

        [Fact(DisplayName = "Obtain available balance should not have error when account exists")]
        public async void GetAvailableBalanceCommandHandler_ShouldReturnSucces()
        {
            _repository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(new Domain.Account("1", "Name", 1000, 1000));
            
            var result = await _handler.Handle(new GetAvailableBalanceQuery("1"), new CancellationToken());

            Assert.False(result.HasError);
        }

        [Fact(DisplayName = "Obtain available balance should return error when account is not found")]
        public async void GetAvailableBalanceCommandHandler_ShouldReturnFail()
        {
            _repository.Setup(x => x.GetAccount(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(default(Domain.Account));

            var result = await _handler.Handle(new GetAvailableBalanceQuery("1"), new CancellationToken());

            Assert.True(result.HasError);
        }
    }
}
