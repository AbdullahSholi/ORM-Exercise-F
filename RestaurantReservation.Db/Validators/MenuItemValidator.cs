using FluentValidation;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db.Validators;

public class MenuItemValidator : AbstractValidator<MenuItem>
{
    public MenuItemValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Menu name is required.");
        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Menu description is required.");
        RuleFor(x => x.Price)
            .NotEmpty()
            .Must(x => x > 0)
            .WithMessage("Menu price is required.");
        RuleFor(x => x.Restaurant)
            .NotNull()
            .WithMessage("Menu must related at least to one restaurant.");
        RuleFor(x => x.OrderItems)
            .NotNull()
            .WithMessage("Menu must have at least one order item.");
    }
}