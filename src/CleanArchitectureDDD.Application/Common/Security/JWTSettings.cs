﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDDD.Application.Common.Security;

/// <summary>
/// Specifies the class this JWT Settings.
/// </summary>

public class JWTSettings
{
    public string JWTKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
}
