using CleanArchitectureDDD.Application.Common.Interfaces;

namespace CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;

public class CreateLanguageCommandValidator : AbstractValidator<CreateLanguageCommand>
{
    private readonly IConfigDbContext _context;
    public CreateLanguageCommandValidator(IConfigDbContext context)
    {
        _context = context;

        RuleFor(c => c.DsLanguage)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified description already exists."); ;

        RuleFor(c => c.DsPrefix)
            .NotEmpty().WithMessage("Prefix is required.")
            .MaximumLength(10).WithMessage("Prefix must not exceed 200 characters.");
            
    }

    public async Task<bool> BeUniqueTitle(string DsLanguage, CancellationToken cancellationToken)
    {
        return await _context.TbMtLanguage
            .AllAsync(x => x.DsLanguage != DsLanguage, cancellationToken);
    }
}
