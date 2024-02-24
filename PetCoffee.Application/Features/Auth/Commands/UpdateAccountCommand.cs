using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Auth.Models;

namespace PetCoffee.Application.Features.Auth.Commands;

public class UpdateAccountCommand : IRequest<AccountResponse>
{
	public string? FullName { get; set; }
	public string? PhoneNumber { get; set; }
	public IFormFile? AvatarFile { get; set; }
	public IFormFile? BackgroundFile { get; set; }
	public string? Description { get; set; }
	public string? Address { get; set; }
}
