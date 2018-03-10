using FluentValidation;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Validation
{
    /// <summary>
    ///     Validates instance of <see cref="ScoreDomainModel"/>.
    /// </summary>
    class ScoreDomainModelValidator : AbstractValidator<ScoreDomainModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScoreDomainModelValidator"/> class.
        /// </summary>
        public ScoreDomainModelValidator()
        {
            this.RuleFor(s => s.GoodId)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Good's id less than zero.");

            this.RuleFor(s => s.Value)
                .GreaterThanOrEqualTo(1)
                .WithMessage("Score less than one.")
                .LessThanOrEqualTo(5)
                .WithMessage("Score grater than five.");
        }
    }
}
