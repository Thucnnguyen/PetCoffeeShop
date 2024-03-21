
using MediatR;
using Microsoft.AspNetCore.Http;
using PetCoffee.Application.Features.Comment.Models;

namespace PetCoffee.Application.Features.Comment.Commands;

public class UpdateCommentCommand : IRequest<CommentResponse>
{
    public long Id { get; set; }
    public string? Content { get; set; }
    public IFormFile? NewImage { get; set; }
}
