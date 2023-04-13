using BenchmarkingPortal.Dal.Dtos;
using MediatR;

namespace BenchmarkingPortal.Bll.Features.Configuration.Commands;

public class CreateConfigurationCommand : IRequest<ConfigurationHeader>
{
    public struct Config
    {
        public Config(string scope, string key, string value)
        {
            Scope = scope;
            Key = key;
            Value = value;
        }

        public string Scope { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public struct Constraint
    {
        public Constraint(string premise, string consequence)
        {
            Premise = premise;
            Consequence = consequence;
        }

        public string Premise { get; set; }
        public string Consequence { get; set; }
    }
    
    public List<Config> Configurations { get; set; } = new List<Config>();

    public List<Constraint> Constraints { get; set; } = new List<Constraint>();

    public void AddConfig(Config config)
    {
        Configurations.Add(config);
    }

    public void AddConstraint(Constraint constraint)
    {
        Constraints.Add(constraint);
    }
}