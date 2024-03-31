using CleanArchitectureDDD.Application.Common.Interfaces;

namespace CleanArchitectureDDD.Application.Languages.Commands.UpdateLanguage;

public class UpdateLanguageCommandValidator : AbstractValidator<UpdateLanguageCommand>
{
    private readonly IConfigDbContext _context;
    public UpdateLanguageCommandValidator(IConfigDbContext context)
    {
        _context = context;

        RuleFor(v => v.DsLanguage)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified description already exists.");

        RuleFor(v => v.DsPrefix)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(10).WithMessage("Description must not exceed 200 characters.");
    }

    public async Task<bool> BeUniqueTitle(UpdateLanguageCommand model, string DsLanguage, CancellationToken cancellationToken)
    {
        return await _context.TbMtLanguage
            .Where(x => x.Id != model.Id)
            .AllAsync(x => x.DsLanguage != DsLanguage, cancellationToken);
    }
}
