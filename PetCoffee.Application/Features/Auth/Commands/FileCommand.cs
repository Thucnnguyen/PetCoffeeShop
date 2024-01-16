

using MediatR;
using Microsoft.AspNetCore.Http;

namespace PetCoffee.Application.Features.Auth.Commands;

public class FileCommand : IRequest<string>
{
	public IFormFile File { get; set; } = default!;
}
