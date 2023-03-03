namespace SourceGenerratorAPI;

public class Personne
{
    public string Nom { get; set; }
    public string Prenom { get; set; }
    public int Age { get; set; }

    public Personne(string nom, string prenom, int age)
    {
        Nom = nom ?? throw new ArgumentNullException(nameof(nom));
        Prenom = prenom ?? throw new ArgumentNullException(nameof(prenom));
        Age = age;
    }
    
    public Personne()
    {
    }
}