namespace CleanArchitectureDDD.Domain.ValueObjects;

public class Prefix(string code) : ValueObject
{
    public static Prefix From(string code)
    {
        var prefix = new Prefix(code);

        if (!SupportedLanguages.Contains(prefix))
        {
            throw new UnsupportedLanguageException(code);
        }

        return prefix;
    }

    public static Prefix English => new("ENG");

    public static Prefix Español => new("ESP");

    public static Prefix Portugues => new("POR");

    public string Code { get; private set; } = string.IsNullOrWhiteSpace(code) ? "ENG" : code;

    public static implicit operator string(Prefix prefix)
    {
        return prefix.ToString();
    }

    public static explicit operator Prefix(string code)
    {
        return From(code);
    }

    public override string ToString()
    {
        return Code;
    }

    protected static IEnumerable<Prefix> SupportedLanguages
    {
        get
        {
            yield return English;
            yield return Español;
            yield return Portugues;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
