using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDDD.Domain.Entities;

public class Language: AuditableEntity
{
    public long CdLanguage { get; set; }
    public string? DsLanguage { get; set; }
    public string? DsPrefix { get; set; }
}
