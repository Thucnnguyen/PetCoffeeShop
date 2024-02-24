using MediatR;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Post.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Post.Queries
{
    public class GetPostByIdQuery : IRequest<PostResponse>
    {
        public long Id { get; init; }
    }
}
