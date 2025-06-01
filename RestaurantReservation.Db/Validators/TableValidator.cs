using FluentValidation;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db.Validators;

public class TableValidator : AbstractValidator<Table>
{
    public TableValidator()
    {
        RuleFor(x => x.Capacity)
            .GreaterThan(0)
            .LessThan(10)
            .WithMessage("Capacity must be between 0 and 10");
        RuleFor(x => x.Reservation)
            .NotNull()
            .WithMessage("Table must included to a specific reservation");
        RuleFor(x => x.Restaurant)
            .NotNull()
            .WithMessage("Table must included to a specific restaurant");
        
    }
}