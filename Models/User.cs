namespace UserInfromationAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public string PhoneNumber { get; set; }
        public Address Address { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
    }
}
