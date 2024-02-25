using MediatR;
using PetCoffee.Application.Features.Auth.Queries;
using PetCoffee.Application.Persistence.Repository;

namespace PetCoffee.Application.Features.Auth.Handlers;

public class CheckEmailExistHandler : IRequestHandler<CheckEmailExistQuery, bool>
{
	private readonly IUnitOfWork _unitOfWork;

	public CheckEmailExistHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork;
	}

	public async Task<bool> Handle(CheckEmailExistQuery request, CancellationToken cancellationToken)
	{
		var account = await _unitOfWork.AccountRepository.GetAsync(a => a.Email == request.Email);
		
		return account.Any();
	}
}
