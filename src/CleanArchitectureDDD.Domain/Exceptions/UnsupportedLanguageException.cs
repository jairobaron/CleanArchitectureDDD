namespace CleanArchitectureDDD.Domain.Exceptions;

public class UnsupportedLanguageException(string code) : Exception($"Language \"{code}\" is unsupported.")
{
}
