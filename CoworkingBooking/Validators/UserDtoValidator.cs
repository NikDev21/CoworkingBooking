using CoworkingBooking.Models.DTO;
using FluentValidation;

namespace CoworkingBooking.Validators
{
    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .MinimumLength(3)
                .WithMessage("Name must be at least 3 characters long")
                .MaximumLength(50)
                .WithMessage("Name must not exceed 50 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Invalid email format");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required")
                .MinimumLength(6)
                .WithMessage("Password must be at least 6 characters long")
                .MaximumLength(100)
                .WithMessage("Password must not exceed 100 characters");
        }   
    }
}
