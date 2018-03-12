using FluentValidation;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Validation
{
    public class CategoryDomainModelValidator : AbstractValidator<CategoryDomainModel>
    {
        public CategoryDomainModelValidator()
        {
            this.RuleFor(c => c.IdAtStore)
                .NotEmpty()
                .WithMessage("Category id is null, empty or contains only white-space characters.");

            this.RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Name is null, empty or contains only white-space characters.");
        }
    }
}
