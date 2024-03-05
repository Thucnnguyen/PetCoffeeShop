using AutoMapper;
using PetCoffee.Application.Features.Areas.Commands;
using PetCoffee.Application.Features.Areas.Models;
using PetCoffee.Application.Features.Auth.Commands;
using PetCoffee.Application.Features.Auth.Models;
using PetCoffee.Application.Features.Comment.Commands;
using PetCoffee.Application.Features.Comment.Models;
using PetCoffee.Application.Features.Events.Commands;
using PetCoffee.Application.Features.Events.Models;
using PetCoffee.Application.Features.Items.Commands;
using PetCoffee.Application.Features.Items.Models;
using PetCoffee.Application.Features.Memory.Commands;
using PetCoffee.Application.Features.Memory.Models;
using PetCoffee.Application.Features.Notifications.Models;
using PetCoffee.Application.Features.Pet.Commands;
using PetCoffee.Application.Features.Pet.Models;
using PetCoffee.Application.Features.PetCfShop.Commands;
using PetCoffee.Application.Features.PetCfShop.Models;
using PetCoffee.Application.Features.Post.Command;
using PetCoffee.Application.Features.Post.Commands;
using PetCoffee.Application.Features.Post.Model;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Application.Features.PostCategory.Commands;
using PetCoffee.Application.Features.PostCategory.Models;
using PetCoffee.Application.Features.SubmitttingEvents.Commands;
using PetCoffee.Application.Features.SubmitttingEvents.Models;
using PetCoffee.Application.Features.Vaccination.Commands;
using PetCoffee.Application.Features.Vaccination.Models;
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

        //Area
        CreateMap<AreaResponse, Area>().ReverseMap();
        CreateMap<UpdateAreaCommand, Area>().ReverseMap();
        CreateMap<CreateAreaCommand, Area>().ReverseMap();



        //Item
        CreateMap<ItemResponse, Item>().ReverseMap();
        CreateMap<CreateItemCommand, Item>().ReverseMap();

        // pet cafe shop
        CreateMap<PetCoffeeShop, PetCoffeeShopResponse>().ReverseMap();
        CreateMap<PetCoffeeShop, PetCoffeeShopForCardResponse>().ReverseMap();
        CreateMap<CreatePetCfShopCommand, PetCoffeeShop>().ReverseMap();
        CreateMap<ShopResponseForAccount, PetCoffeeShop>().ReverseMap();


		//category
		CreateMap<Category,PostCategoryResponse>().ReverseMap();
		CreateMap<Category, CreatePostCategoryCommand>().ReverseMap();

        //post
        CreateMap<CreatePostCommand, Post>().ReverseMap();
        CreateMap<PetCoffeeShop, CoffeeshopForPostModel>().ReverseMap();
        CreateMap<Category, CategoryForPostModel>().ReverseMap();
		CreateMap<Post, PostResponse>()
			   .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.PostCategories.Select(pc => pc.Category)))
			   .ForMember(dest => dest.PetCoffeeShops, opt => opt.MapFrom(src => src.PostPetCoffeeShops.Select(ppc => ppc.Shop)))
			   .ForMember(dest => dest.NamePoster, opt => opt.MapFrom(src => src.ShopId != null ? src.PetCoffeeShop.Name : src.CreatedBy.FullName))
			   .ForMember(dest => dest.PosterAvatar, opt => opt.MapFrom(src => src.ShopId != null ? src.PetCoffeeShop.AvatarUrl : src.CreatedBy.Avatar))
			   .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.CreatedBy.Id));
		//pets
		CreateMap<Pet, PetResponse>()
				.ForMember(dest => dest.Backgrounds, opt => opt.MapFrom(src => src.Backgound))
				.ForMember(dest => dest.Area, opt => opt.MapFrom(src => src.Area));
		CreateMap<Area, AreaResponseForPet>();

		CreateMap<CreatePetCommand, Pet>().ReverseMap();

        //Vaccination
        CreateMap<VaccinationResponse, Vaccination>().ReverseMap();
        CreateMap<AddVaccinationCommand, Vaccination>().ReverseMap();

        //moment
        CreateMap<MomentResponse, Moment>().ReverseMap();
        CreateMap<CreateMomentCommand, Moment>().ReverseMap();
		//comment 
		CreateMap<Comment, CommentForPost>()
			.ForMember(dest => dest.CommentorName, opt => opt.MapFrom(src => src.CreatedBy.FullName));
        CreateMap<Comment, CreateCommentCommand>().ReverseMap();
		CreateMap<Comment, CommentResponse>()
			.ForMember(dest => dest.CommentorName, opt => opt.MapFrom(src => src.ShopId != null ? src.PetCoffeeShop.Name : src.CreatedBy.FullName))
			.ForMember(dest => dest.CommentorImage, opt => opt.MapFrom(src => src.ShopId != null ? src.PetCoffeeShop.AvatarUrl : src.CreatedBy.Avatar));
			
		//like
		CreateMap<CreateLikePostCommand, Like>().ReverseMap();
		//Event
		CreateMap<CreateEventCommand, Event>().ReverseMap();
		CreateMap<EventForCardResponse, Event>().ReverseMap();
		CreateMap<EventResponse, Event>();
		CreateMap<Event, EventResponse>()
				.ForMember(dest => dest.TotalJoinEvent, opt => opt.MapFrom(src => src.SubmittingEvents.Count()))
				.ForMember(dest => dest.IsCanceled, opt => opt.MapFrom(src => src.Deleted));
				

		//eventfield
		CreateMap<CreateFieldEvent, EventField>().ReverseMap();
		CreateMap<FieldEventResponseForEventResponse, EventField>().ReverseMap();

		//SubmittingEvent
		CreateMap<CreateSubmittingEventCommand, SubmittingEvent>().ReverseMap();
		CreateMap<Event,SubmittingEventResponse>()
				.ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.Id));

		//SubmittingEventField
		CreateMap<CreateSubmittingEventField,SubmittingEventField>().ReverseMap();
		CreateMap<EventField, SubmittingEventField>()
			 .ForMember(dest => dest.Id, opt => opt.Ignore());



		CreateMap<SubmittingEventField, EventFieldResponse>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
			.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
			.ForMember(dest => dest.IsOptional, opt => opt.MapFrom(src => src.IsOptional))
			.ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Submitcontent))
			.ForMember(dest => dest.SubmittinhEventId, opt => opt.MapFrom(src => src.SubmittingEventId))
			.ForMember(dest => dest.SubmmitContent, opt => opt.MapFrom(src => src.Submitcontent));

		CreateMap<SubmittingEventField, FieldEventResponseForEventResponse>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Question, opt => opt.MapFrom(src => src.Question))
			.ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
			.ForMember(dest => dest.IsOptional, opt => opt.MapFrom(src => src.IsOptional))
			.ForMember(dest => dest.Answer, opt => opt.MapFrom(src => src.Submitcontent))
			.ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.SubmittingEventId))
			.ForMember(dest => dest.SubmmitContent, opt => opt.MapFrom(src => src.Submitcontent));

		CreateMap<SubmittingEvent, SubmittingEventResponse>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.EventId, opt => opt.MapFrom(src => src.EventId))
			.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Event.Title))
			.ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Event.Image))
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Event.Description))
			.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Event.StartTime))
			.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Event.EndTime))
			.ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Event.Location))
			.ForMember(dest => dest.IsCanceled, opt => opt.MapFrom(src => src.Event.Deleted))
			.ForMember(dest => dest.PetCoffeeShopId, opt => opt.MapFrom(src => src.Event.PetCoffeeShopId));

		CreateMap<SubmittingEvent, EventForCardResponse>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Event.Title))
			.ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Event.Image))
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Event.Description))
			.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Event.StartTime))
			.ForMember(dest => dest.TotalJoinEvent, opt => opt.MapFrom(src => src.Event.FollowEvents.Count))
			.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Event.EndTime))
			.ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Event.Location));

		CreateMap<SubmittingEvent, EventResponse>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Event.Title))
			.ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Event.Image))
			.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Event.Description))
			.ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Event.StartTime))
			.ForMember(dest => dest.TotalJoinEvent, opt => opt.MapFrom(src => src.Event.FollowEvents.Count))
			.ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.Event.EndTime))
			.ForMember(dest => dest.IsCanceled, opt => opt.MapFrom(src => src.Event.Deleted))
			.ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Event.Location));

		// notification
		CreateMap<Notification, NotificationResponse>().ReverseMap();


	}
}
