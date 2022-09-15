using FluentValidation;
using CleanArchitectureDDD.Application.Common.Models;
using CleanArchitectureDDD.Application.Languages.Commands.ImportLanguages;

namespace CleanArchitectureDDD.Application.Languages.Commands.ImportLanguage;

public class ImportLanguagesCommandValidator : AbstractValidator<ImportLanguagesCommand>
{
    public ImportLanguagesCommandValidator()
    {
        RuleFor(c => c.ExcelFile)
            .SetValidator(new FileValidator());
    }
}
