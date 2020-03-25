using FluentValidation;
using UserInfromationAPI.Models;

namespace UserInfromationAPI.Validator
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public AddressValidator()
        {
            RuleFor(address => address.HouseNumber)
               .NotEmpty();
            RuleFor(address => address.City)
                .NotEmpty();
            RuleFor(address => address.State)
                .NotEmpty();
            RuleFor(address => address.Country)
                .NotEmpty();
            RuleFor(address => address.Pincode)
                .NotEmpty();
        }
    }
}
