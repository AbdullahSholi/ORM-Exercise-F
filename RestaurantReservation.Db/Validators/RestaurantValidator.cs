using Bogus;
using FluentValidation;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db.Validators;

public class RestaurantValidator : AbstractValidator<Restaurant>
{
    public RestaurantValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Name is required");
        RuleFor(x => x.Address)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Address is required");
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Phone number is required");
        RuleFor(x => x.OpeningHours)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Opening hours is required");
        RuleFor(x => x.Tables)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage("Restaurant must have at least one table");
        RuleFor(x => x.Reservations)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage("Reservations must have at least one reservation");
        RuleFor(x => x.Employees)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage("Employees must have at least one employee");
        RuleFor(x => x.MenuItems)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage("Menu items must have at least one menu item");
    }
}