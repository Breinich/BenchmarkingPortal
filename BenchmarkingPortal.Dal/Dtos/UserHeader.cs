using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class UserHeader
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = new();
    
    public bool Subscription { get; set; }
    
    public UserHeader() { }
    
    public UserHeader(User u)
    {
        Id = u.Id;
        UserName = u.UserName;
        Email = u.Email;
        Subscription = u.Subscription;
    }
    
}