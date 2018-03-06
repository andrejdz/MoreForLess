using FluentValidation;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Validation
{
    public class GoodDomainModelValidator : AbstractValidator<GoodDomainModel>
    {
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

            this.RuleFor(g => g.CategoryIdOnShop)
                .NotEmpty()
                .WithMessage("Category id at store is null, empty or contains only white-space characters.");
        }
    }
}
