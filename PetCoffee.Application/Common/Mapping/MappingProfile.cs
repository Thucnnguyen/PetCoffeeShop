
using AutoMapper;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CustomerRegisterCommand,Account>().ReverseMap();
    }
}
