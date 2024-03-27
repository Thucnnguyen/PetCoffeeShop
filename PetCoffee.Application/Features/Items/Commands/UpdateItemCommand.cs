
using MediatR;
using Microsoft.AspNetCore.Http;

namespace PetCoffee.Application.Features.Items.Commands;

public class UpdateItemCommand : IRequest<bool>
{
	public long Id { get; set; }
	public string Name { get; set; }
	public double Price { get; set; }
	public string Description { get; set; }
	public IFormFile? newIconImg { get; set; }
}
