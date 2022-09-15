using System.Globalization;
using CleanArchitectureDDD.Application.Languages.Queries.ExportLanguages;
using CsvHelper.Configuration;

namespace CleanArchitectureDDD.Infrastructure.Files.Maps;

public class LanguageRecordMap : ClassMap<LanguageItemRecord>
{
    public LanguageRecordMap()
    {
        AutoMap(CultureInfo.InvariantCulture);

    }
}
