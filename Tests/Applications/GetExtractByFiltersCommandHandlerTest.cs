using Application.Commands.CreateCurrentAccount;
using Application.Queries.Extract;
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
    public class GetExtractByFiltersCommandHandlerTest
    {
        private readonly Mock<ICurrentAccountRepository> _repository;
        private readonly GetExtractByFiltersCommandHandler _handler;

        public GetExtractByFiltersCommandHandlerTest()
        {
            _repository = new Mock<ICurrentAccountRepository>();
            _handler = new GetExtractByFiltersCommandHandler(_repository.Object);
        }

        [Fact(DisplayName = "Obtain extract by filters should not have error when account identifier is specified and movements for this account are found")]
        public async void GetExtractByFiltersCommandHandler_ShouldReturnSucces()
        {
            _repository.Setup(x => x.GetMovementsByFilter(It.IsAny<MovementsFilter>())).Returns(new List<Movements>() { new Movements("1", "1", "", 1000, Domain.Type.DEBIT) });

            var result = await _handler.Handle(new GetExtractByFiltersQuery() { Account_Id = "1" }, new CancellationToken());

            Assert.False(result.HasError);
        }

        [Fact(DisplayName = "Obtain extract by filter should return error when movements for this account are not found")]
        public async void GetExtractByFiltersCommandHandler_ShouldReturnFail_MovementsNotFound()
        {
            _repository.Setup(x => x.GetMovementsByFilter(It.IsAny<MovementsFilter>())).Returns(new List<Movements>());

            var result = await _handler.Handle(new GetExtractByFiltersQuery() { Account_Id = "1"}, new CancellationToken());

            Assert.True(result.HasError);
        }
    }
}
