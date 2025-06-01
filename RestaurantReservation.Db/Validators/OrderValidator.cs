using FluentValidation;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db.Validators;

public class OrderValidator : AbstractValidator<Order>
{
    public OrderValidator()
    {
        RuleFor(x => x.OrderDate)
            .NotNull()
            .Must(x => x >= DateTime.Today)
            .WithMessage("Order date is required.");
        RuleFor(x => x.TotalAmount)
            .NotEmpty()
            .Must(x => x >= decimal.Zero)
            .WithMessage("Total amount is required.");
        RuleFor(x => x.Reservation)
            .NotNull()
            .WithMessage("Order must be related to at least one reservation.");
        RuleFor(x => x.Employee)
            .NotNull()
            .WithMessage("Order must reserve by specific employee.");
        RuleFor(x => x.OrderItems)
            .NotNull()
            .Must(x => x.Count >= 1)
            .WithMessage("Order must have at least one item.");
    }
}