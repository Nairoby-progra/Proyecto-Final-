namespace VeterinariaClinica.Dominio;

public sealed class Consulta
{
    public int CitaId { get; }
    public string Diagnostico { get; }
    public string Tratamiento { get; }
    public string Observaciones { get; }

    public Consulta(int citaId, string diagnostico, string tratamiento, string observaciones)
    {
        if (citaId <= 0) throw new ArgumentException("Seleccione una cita.");
        Diagnostico = string.IsNullOrWhiteSpace(diagnostico)
            ? throw new ArgumentException("El diagnóstico es obligatorio.")
            : diagnostico.Trim();
        CitaId = citaId;
        Tratamiento = tratamiento?.Trim() ?? string.Empty;
        Observaciones = observaciones?.Trim() ?? string.Empty;
    }
}
