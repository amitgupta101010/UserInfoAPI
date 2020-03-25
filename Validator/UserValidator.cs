using FluentValidation;
using UserInfromationAPI.Models;

namespace UserInfromationAPI.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Address).SetValidator(new AddressValidator());
            RuleFor(user => user.FirstName)
                .NotEmpty();
            RuleFor(user => user.LastName)
                .NotEmpty();
            RuleFor(user => user.Email)
                .NotEmpty();
            RuleFor(user => user.Department)
                .NotEmpty();
            RuleFor(user => user.Age)
                .NotEmpty();
            RuleFor(user => user.Role)
                .NotEmpty();
            RuleFor(user => user.PhoneNumber)
                .NotEmpty()
                .Length(10);
        }
    }
}
