using Application.DTOs.Users;
using FluentValidation;

namespace Application.Validations
{
    public class UpdatePasswordDtoValidation : AbstractValidator<UpdatePasswordDto>
    {
        public UpdatePasswordDtoValidation()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required.");
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage("New password is required.")
                .MinimumLength(6)
                .WithMessage("New password must be at least 6 characters long.")
                .Matches("[A-Z]")
                .WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]")
                .WithMessage("Password must contain at least one number");
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirm password is required.")
                .Equal(x => x.NewPassword)
                .WithMessage("Confirm password must match the new password.");
        }
    }
}
