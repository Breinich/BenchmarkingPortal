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
    
    public string? Key { get; set; }

    public string? Value { get; set; }

    public Scope Scope { get; set; }

    public int ConfigurationId { get; set; }
}