using FluentValidation;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Validation
{
    /// <summary>
    ///     Validates instance of <see cref="PagingDomainModel"/>.
    /// </summary>
    public class PagingDomainModelValidator : AbstractValidator<PagingDomainModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PagingDomainModelValidator"/> class.
        /// </summary>
        public PagingDomainModelValidator()
        {
            this.RuleFor(p => p.CurrentPage)
                .GreaterThan(0)
                .WithMessage(p => $"Value: {nameof(p.CurrentPage)} less than ot equal to zero.")
                .LessThanOrEqualTo(p => p.TotalPages)
                .WithMessage(p => $"Value: {nameof(p.CurrentPage)} grater than value: {nameof(p.TotalPages)}.");

            this.RuleFor(p => p.ItemsPerPage)
                .GreaterThan(0)
                .WithMessage(p => $"Value: {nameof(p.ItemsPerPage)} less than or equal to zero.");

            this.RuleFor(p => p.TotalItems)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Total items less than zero.");

            this.RuleFor(p => p.TotalPages)
                .GreaterThan(0)
                .WithMessage(p => $"Value: {nameof(p.TotalPages)} less than or equal to zero.");
        }
    }
}
