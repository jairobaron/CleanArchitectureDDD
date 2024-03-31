using CleanArchitectureDDD.Application.Common.Interfaces;

namespace CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;

public record ExportExcelLanguagesQuery : IRequest<ExportLanguages>
{

}

public class ExportExcelLanguagesQueryHandler : IRequestHandler<ExportExcelLanguagesQuery, ExportLanguages>
{
    private readonly IConfigDbContext _context;
    private readonly IMapper _mapper;
    private readonly IExcelFileBuilder _fileBuilder;

    public ExportExcelLanguagesQueryHandler(IConfigDbContext context, IMapper mapper, IExcelFileBuilder fileBuilder)
    {
        _context = context;
        _mapper = mapper;
        _fileBuilder = fileBuilder;
    }

    public async Task<ExportLanguages> Handle(ExportExcelLanguagesQuery request, CancellationToken cancellationToken)
    {
        var records = await _context.TbMtLanguage
                .ProjectTo<LanguageItemRecord>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        var vm = new ExportLanguages(
            "Languages.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            _fileBuilder.BuildLanguagesFile(records));

        return vm;
    }
}
