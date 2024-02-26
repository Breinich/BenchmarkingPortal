using BenchmarkingPortal.Dal.Entities;

namespace BenchmarkingPortal.Dal.Dtos;

public class ConfigurationItemHeader
{
    public ConfigurationItemHeader()
    { }
    
    public ConfigurationItemHeader(ConfigurationItem ci)
    {
        ConfigurationId = ci.ConfigurationId;
        Key = ci.Key;
        Value = ci.Value;
        Scope = ci.Scope;
    }
    
    public string? Key { get; init; }
    public string? Value { get; init; }
    public Scope Scope { get; init; }
    public int ConfigurationId { get; init; }
}