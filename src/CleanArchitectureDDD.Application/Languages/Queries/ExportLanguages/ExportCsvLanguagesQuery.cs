using CleanArchitectureDDD.Application.Common.Interfaces;

namespace CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;

public record ExportCsvLanguagesQuery : IRequest<ExportLanguages>
{

}

public class ExportCsvLanguagesQueryHandler : IRequestHandler<ExportCsvLanguagesQuery, ExportLanguages>
{
    private readonly IConfigDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICsvFileBuilder _fileBuilder;

    public ExportCsvLanguagesQueryHandler(IConfigDbContext context, IMapper mapper, ICsvFileBuilder fileBuilder)
    {
        _context = context;
        _mapper = mapper;
        _fileBuilder = fileBuilder;
    }

    public async Task<ExportLanguages> Handle(ExportCsvLanguagesQuery request, CancellationToken cancellationToken)
    {
        var records = await _context.TbMtLanguage
                .ProjectTo<LanguageItemRecord>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);

        var vm = new ExportLanguages(
            "Languages.csv",
            "text/csv",
            _fileBuilder.BuildLanguagesFile(records));

        return vm;
    }
}
