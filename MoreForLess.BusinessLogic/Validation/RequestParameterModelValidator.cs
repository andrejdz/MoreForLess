using FluentValidation;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Validation
{
    class RequestParametersModelValidator : AbstractValidator<RequestParametersModel>
    {
        public RequestParametersModelValidator()
        {
            this.RuleFor(r => r.Page)
                .GreaterThan(0)
                .WithMessage("Page less than or equal to zero.");

            this.RuleFor(r => r.Page)
                .LessThanOrEqualTo(10)
                .WithMessage("Page grater than ten.");

            this.RuleFor(r => r.MinPrice)
                .GreaterThanOrEqualTo(100)
                .WithMessage("Minimum price less than 100 (1.00 dollar).");

            this.RuleFor(r => r.MinPrice)
                .LessThanOrEqualTo(int.MaxValue)
                .WithMessage("Minimum price grater than maximum value for int.");

            this.RuleFor(r => r.MaxPrice)
                .GreaterThanOrEqualTo(200)
                .WithMessage("Maximum price less than 200 (2.00 dollars).");

            this.RuleFor(r => r.MaxPrice)
                .LessThanOrEqualTo(int.MaxValue)
                .WithMessage("Maximum price grater than maximum value for int.");
        }
    }
}
