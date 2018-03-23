using FluentValidation;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Validation
{
    class PageCategoryModelValidator : AbstractValidator<PageCategoryModel>
    {
        public PageCategoryModelValidator()
        {
            this.RuleFor(p => p.CurrentPage)
                .GreaterThan(0)
                .WithMessage(p => $"Value: {nameof(p.CurrentPage)} less than or equal to zero.");

            this.RuleFor(p => p.ItemsPerPage)
                .GreaterThan(0)
                .WithMessage(p => $"Value: {nameof(p.ItemsPerPage)} less than or equal to zero.");

            this.RuleFor(c => c.CategoryId)
                .NotEmpty()
                .WithMessage("Category id is null, empty or contains only white-space characters.");
        }
    }
}
