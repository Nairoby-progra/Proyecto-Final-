namespace VeterinariaClinica.Dominio;

public sealed class Cita
{
    public int MascotaId { get; }
    public int VeterinarioId { get; }
    public DateTime FechaHora { get; }
    public string Motivo { get; }
    public string Estado { get; }

    public Cita(int mascotaId, int veterinarioId, DateTime fechaHora, string motivo, string estado)
    {
        if (mascotaId <= 0) throw new ArgumentException("Seleccione una mascota.");
        if (veterinarioId <= 0) throw new ArgumentException("Seleccione un veterinario.");
        MascotaId = mascotaId;
        VeterinarioId = veterinarioId;
        FechaHora = fechaHora;
        Motivo = motivo?.Trim() ?? string.Empty;
        Estado = string.IsNullOrWhiteSpace(estado) ? "Programada" : estado.Trim();
    }
}
