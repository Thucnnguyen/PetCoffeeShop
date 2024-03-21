using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Events.Models;

namespace PetCoffee.Application.Features.Events.Commands;

public class CreateEventValidation : AbstractValidator<CreateEventCommand>
{
    public CreateEventValidation()
    {
        RuleFor(model => model.StartDate)
            .Must((command, StartDate) => StartDate >= DateTime.UtcNow)
            .WithMessage("Thời gian bắt đầu không được ở trong quá khứ");

        RuleFor(model => model.StartDate)
            .Must((command, StartDate) => StartDate < command.EndDate)
            .WithMessage("Thời gian kết thúc phải sau thời gian bắt đầu");
        RuleFor(model => model.StartTime)
            .Must((command, StartTime) => (TimeSpan?)TimeSpan.Parse(StartTime) < (TimeSpan?)TimeSpan.Parse(command.EndTime))
            .WithMessage("Giờ kết thúc phải sau giờ bắt đầu");
    }
}
public class CreateEventCommand : IRequest<EventResponse>
{
    public long PetCoffeeShopId { get; set; }
    public string Title { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string StartTime { get; set; }
    public string EndTime { get; set; }
    public string? Location { get; set; }


}


