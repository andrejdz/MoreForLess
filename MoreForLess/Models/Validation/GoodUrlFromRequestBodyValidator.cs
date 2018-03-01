using FluentValidation;
using MoreForLess.BusinessLogic.Helpers;

namespace MoreForLess.Models.Validation
{
    public class GoodUrlFromRequestBodyValidator : AbstractValidator<GoodUrlFromRequestBody>
    {
        public GoodUrlFromRequestBodyValidator()
        {
            this.RuleFor(g => g.Url)
                .NotEmpty()
                .WithMessage("Good's url is null, empty or contains only white-space characters.")
                .DependentRules(() =>
                {
                    this.RuleFor(g => g.Url)
                        .Must(UrlValidator.CheckUrl)
                        .WithMessage("Good's url has invalid format.");
                });
        }
    }
}