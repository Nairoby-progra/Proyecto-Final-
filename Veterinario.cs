namespace VeterinariaClinica.Dominio;

public sealed class Veterinario
{
    public string Nombre { get; }
    public string Especialidad { get; }
    public string Telefono { get; }

    public Veterinario(string nombre, string especialidad, string telefono)
    {
        Nombre = string.IsNullOrWhiteSpace(nombre)
            ? throw new ArgumentException("El nombre del veterinario es obligatorio.")
            : nombre.Trim();
        Especialidad = especialidad?.Trim() ?? string.Empty;
        Telefono = telefono?.Trim() ?? string.Empty;
    }
}
