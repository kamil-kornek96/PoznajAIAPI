using AutoMapper;
using PoznajAI.Data.Models;
using PoznajAI.Models.User;

public class IsAdminResolver : IValueResolver<User, UserDto, bool>
{
    public bool Resolve(User source, UserDto destination, bool destMember, ResolutionContext context)
    {
        return source.Roles.Any(r => r.Name == UserRole.Admin);
    }
}
