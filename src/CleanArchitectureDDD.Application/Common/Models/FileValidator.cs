using Microsoft.AspNetCore.Http;

namespace CleanArchitectureDDD.Application.Common.Models;

public class FileValidator : AbstractValidator<IFormFile>
{
    public FileValidator()
    {
        RuleFor(x => x.Length).NotNull().LessThanOrEqualTo(5 * 1024 * 1024)
            .WithMessage("File size is larger than allowed");

        RuleFor(x => x.ContentType).NotNull()
            .Must(x => x.Contains("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"))
            .WithMessage("File type is larger than allowed");

        RuleFor(x => Path.GetExtension(x.FileName)).NotNull()
            .Must(x => x.Equals(".xls") || x.Equals(".xlsx") || x.Equals(".csv"))
            .WithMessage("File Extension not supported");

    }
}