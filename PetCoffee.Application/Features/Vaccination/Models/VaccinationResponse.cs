using PetCoffee.Application.Common.Models.Response;
using PetCoffee.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Application.Features.Vaccination.Models
{
    public class VaccinationResponse 
    {
        [Key]
        public long Id { get; set; }
        public DateTime VacciniationDate { get; set; }
        public DateTime ExpireTime { get; set; }
        public VaccinationType VaccinationType { get; set; }
        public string? PhotoEvidence { get; set; }

        public long PetId { get; set; }

        public bool IsVerified { get; set; }    
    }
}
