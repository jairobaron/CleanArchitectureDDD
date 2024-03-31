using Microsoft.AspNetCore.Http;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Domain.Entities;

namespace CleanArchitectureDDD.Application.Languages.Commands.ImportLanguages;

public record ImportLanguagesCommand : IRequest<ICollection<LanguagesRecord>>
{
    public IFormFile ExcelFile { get; set; } = default!;
}

public class ImportLanguagesCommandHandler: IRequestHandler<ImportLanguagesCommand, ICollection<LanguagesRecord>>
{
    private readonly IConfigDbContext _context;
    private readonly IExcelFileImport _fileImport;

    public ImportLanguagesCommandHandler(IConfigDbContext context, IExcelFileImport fileImport)
    {
        _context = context;
        _fileImport = fileImport;
    }

    public async Task<ICollection<LanguagesRecord>> Handle(ImportLanguagesCommand request, CancellationToken cancellationToken)
    {
        Stream excelStream = request.ExcelFile.OpenReadStream();

        var languages = await _fileImport.ImportLanguagesFile(excelStream);

        foreach (var language in languages)
        {
            var entity = _context.TbMtLanguage
                .FirstOrDefault(x => x.DsPrefix == language.DsPrefix);
            if (entity == null)
            {
                var newEntity = new Language
                {
                    DsLanguage = language.DsLanguage,
                    DsPrefix = language.DsPrefix
                };

                _context.TbMtLanguage.Add(newEntity);
            }
            else
            {
                entity.DsLanguage = language.DsLanguage;
                entity.DsPrefix = language.DsPrefix;
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        return languages;
    }
}