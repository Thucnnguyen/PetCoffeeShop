using PetCoffee.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace PetCoffee.Application.Features.Vaccination.Models
{
    public class VaccinationResponse
    {
        [Key]
        public long Id { get; set; }
        public DateTime VaccinationDate { get; set; }
        public DateTime ExpireTime { get; set; }
        public VaccinationType VaccinationType { get; set; }
        public string? PhotoEvidence { get; set; }

        public long PetId { get; set; }

        public bool IsVerified { get; set; }
    }
}
