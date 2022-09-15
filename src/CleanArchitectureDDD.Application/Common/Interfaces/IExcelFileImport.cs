using System;
using System.IO;
using System.Collections.Generic;
using CleanArchitectureDDD.Application.Languages.Commands.ImportLanguages;
using Microsoft.AspNetCore.Http;

namespace CleanArchitectureDDD.Application.Common.Interfaces;

public interface IExcelFileImport
{
    Task<List<LanguagesRecord>> ImportLanguagesFile(Stream excelFile);
}
