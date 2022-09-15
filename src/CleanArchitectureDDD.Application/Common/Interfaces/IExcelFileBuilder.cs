using System;
using System.IO;
using System.Collections.Generic;
using CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;

namespace CleanArchitectureDDD.Application.Common.Interfaces;

public interface IExcelFileBuilder
{
    byte[] BuildLanguagesFile(IEnumerable<LanguageItemRecord> records);
}
