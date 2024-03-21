using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Items.Models;


namespace PetCoffee.Application.Features.Items.Commands
{
	public class CreateItemValidation : AbstractValidator<CreateItemCommand>
	{
		public CreateItemValidation()
		{
			RuleFor(model => model.Name).NotEmpty();
			RuleFor(model => model.Price).NotEmpty().GreaterThan(0);
		}
	}
	public class CreateItemCommand : IRequest<ItemResponse>
	{
		public string Name { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		public IFormFile IconImg { get; set; }
	}
}
