using FluentValidation;
using RestaurantReservation.Db.models;

namespace RestaurantReservation.Db.Validators;

public class EmployeeValidator : AbstractValidator<Employee>
{
    public EmployeeValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("First name is required.");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Last name is required.");
        RuleFor(x => x.Position)
            .NotEmpty()
            .Must(x => !string.IsNullOrWhiteSpace(x))
            .WithMessage("Position is required.");
        RuleFor(x => x.Orders)
            .NotNull()
            .Must(r => r != null && r.Count > 0)
            .WithMessage("Employee must have at least one order.");
        RuleFor(x => x.Restaurant)
            .NotNull()
            .WithMessage("Employee must work at least one restaurant.");
    }
}