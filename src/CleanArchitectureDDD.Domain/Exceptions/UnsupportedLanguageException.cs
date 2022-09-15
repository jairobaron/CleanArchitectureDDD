using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitectureDDD.Domain.Exceptions;

public class UnsupportedLanguageException : Exception
{
    public UnsupportedLanguageException(string code)
        : base($"Language \"{code}\" is unsupported.")
    {
    }
}
