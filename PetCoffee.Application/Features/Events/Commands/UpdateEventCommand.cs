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
			.Must((command, StartDate) => StartDate >= DateTime.UtcNow)
			.WithMessage("Thời gian bắt đầu không được ở trong quá khứ");

		RuleFor(model => model.StartDate)
			.Must((command, StartDate) => StartDate < command.EndDate)
			.WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu");

		RuleFor(model => model.StartTime)
			.Must((command, StartTime) => StartTime < command.EndTime)
			.WithMessage("Giờ kết thúc phải sau giờ bắt đầu");
	}
}
public class UpdateEventCommand : IRequest<EventResponse>
{
	public long Id { get; set; }
	public string? Title { get; set; }
	public IFormFile? NewImageFile { get; set; }
	public string? Description { get; set; }
	public DateTime? StartDate { get; set; }
	public DateTime? EndDate { get; set; }
	public TimeSpan? StartTime { get; set; }
	public TimeSpan? EndTime { get; set; }
	public string? Location { get; set; }
}
