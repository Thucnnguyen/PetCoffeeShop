
using AutoMapper;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Comment.Commands;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Memory.Commands;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Post.Command;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Domain.Entities;

namespace PetCoffee.Application.Common.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        //Account
        CreateMap<CustomerRegisterCommand,Account>().ReverseMap();
        CreateMap<AccountResponse,Account>().ReverseMap();
        CreateMap<AccountForPostModel,Account>().ReverseMap();
            

        // pet cafe shop
        CreateMap<PetCoffeeShop, PetCoffeeShopResponse>().ReverseMap();
        CreateMap<PetCoffeeShop, PetCoffeeShopForCardResponse>().ReverseMap();
        CreateMap<CreatePetCfShopCommand, PetCoffeeShop>().ReverseMap();
            
            
		//category
		CreateMap<Category,PostCategoryResponse>().ReverseMap();
		CreateMap<Category, CreatePostCategoryCommand>().ReverseMap();

        //post
        CreateMap<CreatePostCommand, Post>().ReverseMap();
        CreateMap<PetCoffeeShop, CoffeeshopForPostModel>().ReverseMap();
        CreateMap<Category, CategoryForPostModel>().ReverseMap();
		CreateMap<Post, PostResponse>()
			   .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments))
			   .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.PostCategories))
			   .ForMember(dest => dest.PetCoffeeShops, opt => opt.MapFrom(src => src.PostPetCoffeeShops))
			   .ForMember(dest => dest.Account, opt => opt.MapFrom(src => src.CreatedBy));
		//pets
		CreateMap<PetResponse, Pet>().ReverseMap();
		CreateMap<CreatePetCommand, Pet>().ReverseMap();

        //moment
        CreateMap<MomentResponse, Moment>().ReverseMap();
        CreateMap<CreateMomentCommand, Moment>().ReverseMap();
		//comment 
		CreateMap<Comment, CommentForPost>()
			.ForMember(dest => dest.CommentorName, opt => opt.MapFrom(src => src.CreatedBy.FullName));
        CreateMap<Comment, CreateCommentCommand>().ReverseMap();
        CreateMap<Comment, CommentResponse>().ReverseMap();
	}
}
