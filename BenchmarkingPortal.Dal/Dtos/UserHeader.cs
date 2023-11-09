using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class UserHeader
{
    public UserHeader()
    {
    }

    public UserHeader(User u)
    {
        Id = u.Id;
        UserName = u.UserName;
        Email = u.Email;
        Subscription = u.Subscription;
    }

    public int Id { get; init; }
    public string? UserName { get; init; }
    public string? Email { get; init; }
    public List<string> Roles { get; init; } = new();

    public bool Subscription { get; init; }
}