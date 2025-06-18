using FluentValidation;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db.Validators;

public class ReservationValidator : AbstractValidator<Reservation>
{
    public ReservationValidator()
    {
        RuleFor(x => x.ReservationDate)
            .NotNull()
            .WithMessage("Reservation Date is required.");
        RuleFor(x => x.PartySize)
            .GreaterThan(0)
            .WithMessage("Party Size must greater than zero.");
        RuleFor(x => x.Customer)
            .NotNull()
            .WithMessage("Reservation perform by specific user");
        RuleFor(x => x.Restaurant)
            .NotNull()
            .WithMessage("Reservation perform inside specific restaurant");
        RuleFor(x => x.Tables)
            .NotNull()
            .Must(x => x.Count >= 1)
            .WithMessage("Reservation must have one or more tables.");
        RuleFor(x => x.Orders)
            .NotNull()
            .Must(x => x.Count >= 1)
            .WithMessage("Reservation must have one or more orders.");
    }
}