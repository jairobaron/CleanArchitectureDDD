using CleanArchitectureDDD.Application.Common.Exceptions;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Domain.Entities;
using MediatR;

namespace CleanArchitectureDDD.Application.Languages.Commands.UpdateLanguage;

public record UpdateLanguageCommand : IRequest
{
    public long CdLanguage { get; set; }
    public string? DsLanguage { get; set; }
    public string? DsPrefix { get; set; }
}

public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand>
{
    private readonly IConfigDbContext _context;

    public UpdateLanguageCommandHandler(IConfigDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TbMtLanguage.FindAsync(new object[] { request.CdLanguage }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Language), request.CdLanguage);
        }

        entity.DsLanguage = request.DsLanguage;
        entity.DsPrefix = request.DsPrefix;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
