using Bogus;
using CodeSourceGeneratorLib;

namespace SourceGenerratorAPI;

[RepositoryDependency]
public class Pays:IPays
{
    public IEnumerable<Personne> Population()
    {
        return GeneratePerson();
    }
    
    //generate persons using the bogus library
    private IEnumerable<Personne> GeneratePerson()
    {
        var faker = new Faker<Personne>()
            .RuleFor(p => p.Nom, f => f.Name.LastName())
            .RuleFor(p => p.Prenom, f => f.Name.FirstName())
            .RuleFor(p => p.Age, f => f.Random.Int(0, 100));
    
        return faker.Generate(100);
    }
    
}