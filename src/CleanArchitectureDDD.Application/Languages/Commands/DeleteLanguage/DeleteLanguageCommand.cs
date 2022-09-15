using CleanArchitectureDDD.Application.Common.Exceptions;
using CleanArchitectureDDD.Application.Common.Interfaces;
using CleanArchitectureDDD.Domain.Entities;
using MediatR;

namespace CleanArchitectureDDD.Application.Languages.Commands.DeleteLanguage;

public record DeleteLanguageCommand : IRequest
{
    public long CdLanguage { get; set; }
}

public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand>
{
    private readonly IConfigDbContext _context;

    public DeleteLanguageCommandHandler(IConfigDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TbMtLanguage.FindAsync(new object[] { request.CdLanguage }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Language), request.CdLanguage);
        }

        entity.IsLogicalDelete = 1;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
