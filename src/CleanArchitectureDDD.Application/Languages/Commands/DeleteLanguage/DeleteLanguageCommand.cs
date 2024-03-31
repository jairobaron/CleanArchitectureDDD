using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Domain.Events;

namespace CleanArchitectureDDD.Application.Languages.Commands.DeleteLanguage;

public record DeleteLanguageCommand(Guid Id) : IRequest;

public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand>
{
    private readonly IConfigDbContext _context;

    public DeleteLanguageCommandHandler(IConfigDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TbMtLanguage.FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.IsLogicalDelete = 1;

        entity.AddDomainEvent(new LanguageDeletedEvent(entity));

        await _context.SaveChangesAsync(cancellationToken);
    }
}
