using AutoMapper;
using System;

public class StringToGuidConverter : ITypeConverter<string, Guid>
{
    public Guid Convert(string source, Guid destination, ResolutionContext context)
    {
        if (Guid.TryParse(source, out Guid result))
        {
            return result;
        }
        return Guid.Empty;
    }
}

public class GuidToStringConverter : ITypeConverter<Guid, string>
{
    public string Convert(Guid source, string destination, ResolutionContext context)
    {
        return source.ToString();
    }
}
