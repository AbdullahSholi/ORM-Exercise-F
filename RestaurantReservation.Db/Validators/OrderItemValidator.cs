using FluentValidation;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db.Validators;

public class OrderItemValidator : AbstractValidator<OrderItem>
{
    public OrderItemValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than 0");
        RuleFor(x => x.Order)
            .NotNull()
            .WithMessage("Order item must included to specific order");
        RuleFor(x => x.MenuItem)
            .NotNull()
            .WithMessage("Order item must be one of menu items");
    }
}