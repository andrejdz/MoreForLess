using FluentValidation;
using MoreForLess.BusinessLogic.Helpers;
using MoreForLess.BusinessLogic.Models;

namespace MoreForLess.BusinessLogic.Validation
{
    public class URLInfoValidator : AbstractValidator<URLInfo>
    {
        public URLInfoValidator()
        {
            this.RuleFor(i => i.Id)
                .NotEmpty()
                .WithMessage("Id is null or empty.");

            this.RuleFor(i => i.Platform)
                .NotEmpty()
                .WithMessage("Platform is null or empty.");

            this.RuleFor(i => i.AbsoluteUri)
                .Must(UrlValidator.CheckUrl)
                .WithMessage("AbsoluteUri has invalid format.");
        }
    }
}