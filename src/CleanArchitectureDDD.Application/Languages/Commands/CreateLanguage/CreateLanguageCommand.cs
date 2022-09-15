using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Domain.Entities;
using MediatR;
using CleanArchitectureDDD.Domain.ValueObjects;

namespace CleanArchitectureDDD.Application.Languages.Commands.CreateLanguage;

public record CreateLanguageCommand : IRequest<long>
{
    public string? DsLanguage { get; set; }
    public string? DsPrefix { get; set; }
}

public class CreateLanguageCommandHandler : IRequestHandler<CreateLanguageCommand, long>
{
    private readonly IConfigDbContext _context;

    public CreateLanguageCommandHandler(IConfigDbContext context)
    {
        _context = context;
    }

    public async Task<long> Handle(CreateLanguageCommand request, CancellationToken cancellationToken)
    {
        var entity = new Language
        {
            DsLanguage = request.DsLanguage,
            DsPrefix = Prefix.From(request.DsPrefix??"")
        };

        //entity.AddDomainEvents(new LanguageCreatedEvent(entity));

        _context.TbMtLanguage.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.CdLanguage;
    }
}
