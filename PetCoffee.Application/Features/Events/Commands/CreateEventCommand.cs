using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Commands;

public class CreateEventCommand : IRequest<EventResponse>
{
	public string Title { get; set; }
	public IFormFile? ImageFile { get; set; }
	public string? Description { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public string? Location { get; set; }
	
	
}


