using FluentValidation;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Validation
{
    /// <summary>
    ///     Validates instance of <see cref="GoodDomainModel"/>.
    /// </summary>
    public class GoodDomainModelValidator : AbstractValidator<GoodDomainModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="GoodDomainModelValidator"/> class.
        /// </summary>
        public GoodDomainModelValidator()
        {
            this.RuleFor(g => g.Name)
                .NotEmpty()
                .WithMessage("Name is null, empty or contains only white-space characters.");

            this.RuleFor(g => g.Price)
                .GreaterThan(decimal.Zero)
                .WithMessage("Price less than or equal to zero.");

            this.RuleFor(g => g.LinkOnProduct)
                .NotEmpty()
                .WithMessage("Good's url is null, empty or contains only white-space characters.");

            this.RuleFor(g => g.IdGoodOnShop)
                .NotEmpty()
                .WithMessage("Good's id in store is null, empty or contains only white-space characters.");

            this.RuleFor(g => g.ShopName)
                .NotEmpty()
                .WithMessage("Name of store is null, empty or contains only white-space characters.");

            this.RuleFor(g => g.CurrencyName)
                .NotEmpty()
                .WithMessage("Currency is null, empty or contains only white-space characters.");

            //this.RuleFor(g => g.CategoryId)
            //    .NotEmpty()
            //    .WithMessage("Id of category is null, empty or contains only white-space characters.");

            this.RuleFor(g => g.Average.Value)
                .GreaterThanOrEqualTo(1.0)
                .WithMessage("Average value of scores less than 1.0.")
                .LessThanOrEqualTo(5.0)
                .WithMessage("Average value of scores grater than 5.0.");

        }
    }
}
