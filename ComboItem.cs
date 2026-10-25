namespace VeterinariaClinica.Dominio;

/// <summary>Elemento simple (Id + texto) para enlazar ComboBox a datos de la base.</summary>
public sealed class ComboItem
{
    public int Id { get; }
    public string Texto { get; }

    public ComboItem(int id, string texto)
    {
        Id = id;
        Texto = texto;
    }

    public override string ToString() => Texto;
}
