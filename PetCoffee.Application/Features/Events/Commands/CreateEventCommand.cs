﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Commands;

public class CreateEventValidation : AbstractValidator<CreateEventCommand>
{
    public CreateEventValidation()
    {
        RuleFor(model => model.StartTime)
			.Must((command, StartTime) => StartTime <= DateTime.UtcNow)
			.WithMessage("Thời gian bắt đầu không được ở trong quá khứ");

		RuleFor(model => model.StartTime)
			.Must((command, StartTime) => StartTime > command.EndTime)
			.WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu");
	}
}
public class CreateEventCommand : IRequest<EventResponse>
{
	public string Title { get; set; }
	public IFormFile? ImageFile { get; set; }
	public string? Description { get; set; }
	public DateTime StartTime { get; set; }
	public DateTime EndTime { get; set; }
	public string? Location { get; set; }
	
	
}


