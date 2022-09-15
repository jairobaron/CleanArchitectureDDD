using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Application.Common.Mappings;
using CleanArchitectureDDD.Application.Common.Models;
using MediatR;

namespace CleanArchitectureDDD.Application.Languages.Queries.GetLanguages;

public record GetLanguagesWithPaginationQuery : IRequest<PaginatedList<LanguageBriefDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetLanguageWithPaginationQueryHandler : IRequestHandler<GetLanguagesWithPaginationQuery, PaginatedList<LanguageBriefDto>>
{     
    private readonly IConfigDbContext _context;
    private readonly IMapper _mapper;

    public GetLanguageWithPaginationQueryHandler(IConfigDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PaginatedList<LanguageBriefDto>> Handle(GetLanguagesWithPaginationQuery request, CancellationToken cancellationToken)
    {
        return await _context.TbMtLanguage.Where(x => x.IsLogicalDelete == 0)
            .OrderBy(x => x.DsLanguage)
            .ProjectTo<LanguageBriefDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
