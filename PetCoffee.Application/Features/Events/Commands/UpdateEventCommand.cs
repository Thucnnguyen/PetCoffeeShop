﻿using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Commands;

public class UpdateEventValidation : AbstractValidator<UpdateEventCommand>
{
	public UpdateEventValidation()
	{

		RuleFor(model => model.StartDate)
			.Must((command, StartDate) => StartDate >= DateTimeOffset.UtcNow)
			.WithMessage("Thời gian bắt đầu không được ở trong quá khứ");

		RuleFor(model => model.StartDate)
			.Must((command, StartDate) => StartDate < command.EndDate)
			.WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu");

		RuleFor(model => model.StartTime)
			.Must((command, StartTime) => (TimeSpan?)TimeSpan.Parse(StartTime) < (TimeSpan?)TimeSpan.Parse(command.EndTime))
			.WithMessage("Giờ kết thúc phải sau giờ bắt đầu");
	}
}
public class UpdateEventCommand : IRequest<EventResponse>
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public IFormFile? NewImageFile { get; set; }
	public string? Description { get; set; }
	public DateTimeOffset? StartDate { get; set; }
	public DateTimeOffset? EndDate { get; set; }
	public string? StartTime { get; set; }
	public string? EndTime { get; set; }
	public string? Location { get; set; }
	public int? MinParticipants { get; set; }
	public int? MaxParticipants { get; set; }
}
