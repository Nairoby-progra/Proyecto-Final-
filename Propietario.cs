namespace VeterinariaClinica.Dominio;

public sealed class Propietario
{
    public string Nombre { get; }
    public string Telefono { get; }
    public string Email { get; }
    public string Direccion { get; }

    public Propietario(string nombre, string telefono, string email, string direccion)
    {
        Nombre = string.IsNullOrWhiteSpace(nombre)
            ? throw new ArgumentException("El nombre del propietario es obligatorio.")
            : nombre.Trim();
        Telefono = telefono?.Trim() ?? string.Empty;
        Email = email?.Trim() ?? string.Empty;
        Direccion = direccion?.Trim() ?? string.Empty;
    }
}
