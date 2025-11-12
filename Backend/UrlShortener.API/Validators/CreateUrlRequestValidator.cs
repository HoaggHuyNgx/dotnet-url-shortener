using FluentValidation;
using UrlShortener.API.DTOs;

namespace UrlShortener.API.Validators;

public class CreateUrlRequestValidator : AbstractValidator<CreateUrlRequest>
{
    public CreateUrlRequestValidator()
    {
        RuleFor(x => x.OriginalUrl)
            .NotEmpty().WithMessage("Original URL is required.")
            .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("A valid URL must be provided.");

        RuleFor(x => x.CustomCode)
            .Matches("^[a-zA-Z0-9-]*$").WithMessage("Custom code can only contain letters, numbers, and hyphens.")
            .MaximumLength(20).WithMessage("Custom code cannot be longer than 20 characters.");
    }
}
