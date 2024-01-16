

using MediatR;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Service;

namespace PetCoffee.Application.Features.Auth.Handlers;

internal class FileHandler : IRequestHandler<FileCommand, string>
{
	private readonly IAzureService _service;
	public FileHandler(IAzureService service)
	{
		_service = service;
	}
	public Task<string> Handle(FileCommand request, CancellationToken cancellationToken)
	{
		return _service.CreateBlob(request.File.FileName, request.File);
	}
}


