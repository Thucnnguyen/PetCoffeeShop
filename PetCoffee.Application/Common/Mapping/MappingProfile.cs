
using AutoMapper;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Domain.Entities;
using static System.Formats.Asn1.AsnWriter;

namespace PetCoffee.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CustomerRegisterCommand,Account>().ReverseMap();

        // pet cafe shop
        CreateMap<PetCoffeeShop, PetCoffeeShopResponse>();
    }
}
