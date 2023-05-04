namespace BenchmarkingPortal.Dal.Dtos;

public class UserHeader
{
    public int Id { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public string? Role { get; set; }
    
    public bool Subscription { get; set; }
    
}