using FluentValidation;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderCommandValidator()
        {
            RuleFor(x => x.Order.UserName).NotEmpty()
                                          .WithMessage("Username cant be empty")
                                          .NotNull()
                                          .MaximumLength(50)
                                          .WithMessage("Username max length is 50");

            RuleFor(x => x.Order.EmailAddress).NotNull()
                                              .WithMessage("Email cant be empty")
                                              .EmailAddress()
                                              .WithMessage("Incorrect email address");

            RuleFor(x => x.Order.TotalPrice).NotEmpty()
                                            .WithMessage("Totalprice is required")
                                            .GreaterThan(0)
                                            .WithMessage("Totalprice should be positive number");
        }
    }
}
