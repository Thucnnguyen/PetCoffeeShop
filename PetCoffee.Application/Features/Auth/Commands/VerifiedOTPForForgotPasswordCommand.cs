
using MediatR;

namespace PetCoffee.Application.Features.Auth.Commands;

public class VerifiedOTPForForgotPasswordCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string OTP { get; set; }
}
