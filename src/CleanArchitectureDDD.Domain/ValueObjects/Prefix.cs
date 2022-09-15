namespace CleanArchitectureDDD.Domain.ValueObjects;

public class Prefix : ValueObject
{
    static Prefix()
    {
    }

    private Prefix()
    {
    }

    private Prefix(string code)
    {
        Code = code;
    }

    public static Prefix From(string code)
    {
        var prefix = new Prefix { Code = code };

        if (!SupportedLanguages.Contains(prefix))
        {
            throw new UnsupportedLanguageException(code);
        }

        return prefix;
    }

    public static Prefix English => new("ENG");

    public static Prefix Español => new("ESP");

    public static Prefix Chinese => new("CHN");

    public string Code { get; private set; } = "ENG";

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
            yield return Chinese;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
    }
}
