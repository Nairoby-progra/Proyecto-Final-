namespace VeterinariaClinica.Dominio;

public sealed class Mascota
{
    public int PropietarioId { get; }
    public string Nombre { get; }
    public string Especie { get; }
    public string Raza { get; }
    public DateTime FechaNacimiento { get; }

    public Mascota(int propietarioId, string nombre, string especie, string raza, DateTime fechaNacimiento)
    {
        if (propietarioId <= 0) throw new ArgumentException("Seleccione un propietario.");
        Nombre = string.IsNullOrWhiteSpace(nombre)
            ? throw new ArgumentException("El nombre de la mascota es obligatorio.")
            : nombre.Trim();
        PropietarioId = propietarioId;
        Especie = especie?.Trim() ?? string.Empty;
        Raza = raza?.Trim() ?? string.Empty;
        FechaNacimiento = fechaNacimiento;
    }
}
