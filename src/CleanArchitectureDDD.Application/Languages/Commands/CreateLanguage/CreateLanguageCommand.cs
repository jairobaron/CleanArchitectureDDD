using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Domain.Entities;
using CleanArchitectureDDD.Domain.Events;
using CleanArchitectureDDD.Domain.ValueObjects;

namespace CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;

public record CreateLanguageCommand : IRequest<Guid>
{
    public string? DsLanguage { get; set; }
    public string? DsPrefix { get; set; }
}

public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, Guid>
{
    private readonly IConfigDbContext _context;

    public CreateLanguageCommandHandler(IConfigDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        var entity = new Language
        {
            Id = Guid.NewGuid(),
            DsLanguage = request.DsLanguage,
            DsPrefix = Prefix.From(request.DsPrefix??"")
        };

        entity.AddDomainEvent(new LanguageCreatedEvent(entity));

        _context.TbMtLanguage.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
