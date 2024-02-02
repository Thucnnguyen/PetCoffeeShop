using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public ShopStatus Status { get; set; } = ShopStatus.Active;
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}
