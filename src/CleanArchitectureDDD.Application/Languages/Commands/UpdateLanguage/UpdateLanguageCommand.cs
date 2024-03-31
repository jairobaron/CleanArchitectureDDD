using CleanArchitectureDDD.Application.Common.Interfaces;

namespace CleanArchitectureDDD.Application.Languages.Commands.UpdateLanguage;

public record UpdateLanguageCommand : IRequest
{
    public Guid Id { get; init; }
    public string? DsLanguage { get; init; }
    public string? DsPrefix { get; init; }
}

public class UpdateLanguageCommandHandler : IRequestHandler<UpdateLanguageCommand>
{
    private readonly IConfigDbContext _context;

    public UpdateLanguageCommandHandler(IConfigDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateLanguageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TbMtLanguage.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.DsLanguage = request.DsLanguage;
        entity.DsPrefix = request.DsPrefix;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
