using Domain;
using Domain.Abstractions;
using MediatR;

namespace Application.Commands.CreateCurrentAccount
{
    public class CreateCurrentAccountCommandHandler : IRequestHandler<CreateCurrentAccountCommand, Response<string>>
    {
        private readonly ICurrentAccountRepository _currentAccountRepository;
        public CreateCurrentAccountCommandHandler(ICurrentAccountRepository currentAccountRepository) 
            => _currentAccountRepository = currentAccountRepository;

        public async Task<Response<string>> Handle(CreateCurrentAccountCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Name)) 
                return Response<string>.Fail(new Error("400", "Name shouldn't be null or empty"));

            var account = Account.CreateBasicAccount(request.Name);
            await _currentAccountRepository.AddAccount(account, cancellationToken);

            return Response<string>.Success(account.Id);
        }
    }
}
