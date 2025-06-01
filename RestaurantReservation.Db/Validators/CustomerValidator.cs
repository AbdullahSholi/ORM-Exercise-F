using FluentValidation;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db.Validators;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Invalid first name");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Invalid last name.");
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email format.");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+?[0-9]{10,15}$")
            .WithMessage("Phone number must be between 10 and 15 digits and can start with +");
        RuleFor(x => x.Reservations)
            .NotNull()
            .Must(r => r != null && r.Count > 0)
            .WithMessage("Customer must have at least one reservation.");
    }
}