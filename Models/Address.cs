using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserInfromationAPI.Models
{
    public class Address
    {
        [Required]
        public string HouseNumber { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public int Pincode { get; set; }

    }
}
