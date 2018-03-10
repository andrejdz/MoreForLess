using FluentValidation;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Validation
{
    /// <summary>
    ///     Validates instance of <see cref="CommentDomainModel"/>.
    /// </summary>
    public class CommentDomainModelValidator : AbstractValidator<CommentDomainModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CommentDomainModelValidator"/> class.
        /// </summary>
        public CommentDomainModelValidator()
        {
            this.RuleFor(c => c.GoodId)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Good's id less than zero.");

            this.RuleFor(c => c.Text)
                .NotEmpty()
                .WithMessage("Comment is null, empty or contains only white-space characters.")
                .MaximumLength(2000)
                .WithMessage("Length of comment's text grater than 2000 characters.");
        }
    }
}
