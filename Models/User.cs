using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserInfromationAPI.Models
{
    
    public class User
    {
        [Required]
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Department { get; set; }
        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [Required]
        public Address Address { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public int Age { get; set; }

    }
}
