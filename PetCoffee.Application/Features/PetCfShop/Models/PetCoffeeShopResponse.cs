﻿using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Application.Features.Post.Models;
using PetCoffee.Domain.Enums;
using System.Text.Json.Serialization;

namespace PetCoffee.Application.Features.PetCfShop.Models
{
    public class PetCoffeeShopResponse : BaseAuditableEntityResponse
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? FbUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? AvatarUrl { get; set; }
        public string? BackgroundUrl { get; set; }

        public string Phone { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

		[JsonConverter(typeof(JsonStringEnumConverter))]
		public ShopStatus Status { get; set; } = ShopStatus.Active;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ShopType Type { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public double? Distance { get; set; } = 0;
        public double? TotalFollow { get; set; } = 0;
        public bool IsFollow {  get; set; }
        public AccountForPostModel? CreatedBy { get; set; }

    }
}
