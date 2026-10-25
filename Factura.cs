namespace VeterinariaClinica.Dominio;

public sealed class Factura
{
    public int ConsultaId { get; }
    public decimal Monto { get; }
    public DateTime FechaEmision { get; }
    public bool Pagada { get; }

    public Factura(int consultaId, decimal monto, DateTime fechaEmision, bool pagada)
    {
        if (consultaId <= 0) throw new ArgumentException("Seleccione una consulta.");
        if (monto <= 0) throw new ArgumentException("El monto debe ser mayor que cero.");
        ConsultaId = consultaId;
        Monto = monto;
        FechaEmision = fechaEmision;
        Pagada = pagada;
    }
}
