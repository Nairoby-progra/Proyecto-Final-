using System.Data;
using Microsoft.Data.Sqlite;
using VeterinariaClinica.Dominio;

namespace VeterinariaClinica.Persistencia;

/// <summary>Acceso a datos (CRUD) para todas las entidades del sistema, vía SQLite.</summary>
public sealed class RepositorioVeterinaria
{
    private readonly string _cadenaConexion;

    public RepositorioVeterinaria(string cadenaConexion) => _cadenaConexion = cadenaConexion;

    private SqliteConnection AbrirConexion()
    {
        var conexion = new SqliteConnection(_cadenaConexion);
        conexion.Open();
        using var pragma = conexion.CreateCommand();
        pragma.CommandText = "PRAGMA foreign_keys = ON;";
        pragma.ExecuteNonQuery();
        return conexion;
    }

    private static DataTable Llenar(SqliteCommand comando)
    {
        using var lector = comando.ExecuteReader();
        var tabla = new DataTable();
        tabla.Load(lector);
        return tabla;
    }

    // ===================== PROPIETARIOS =====================

    public DataTable ObtenerPropietarios()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Nombre, Telefono, Email, Direccion FROM Propietarios ORDER BY Nombre;";
        return Llenar(comando);
    }

    public List<ComboItem> ObtenerPropietariosCombo()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Nombre FROM Propietarios ORDER BY Nombre;";
        using var lector = comando.ExecuteReader();
        var lista = new List<ComboItem>();
        while (lector.Read()) lista.Add(new ComboItem(lector.GetInt32(0), lector.GetString(1)));
        return lista;
    }

    public void CrearPropietario(Propietario p)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "INSERT INTO Propietarios (Nombre, Telefono, Email, Direccion) VALUES ($nombre,$telefono,$email,$direccion);";
        comando.Parameters.AddWithValue("$nombre", p.Nombre);
        comando.Parameters.AddWithValue("$telefono", p.Telefono);
        comando.Parameters.AddWithValue("$email", p.Email);
        comando.Parameters.AddWithValue("$direccion", p.Direccion);
        comando.ExecuteNonQuery();
    }

    public void ActualizarPropietario(int id, Propietario p)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Propietarios SET Nombre=$nombre, Telefono=$telefono, Email=$email, Direccion=$direccion WHERE Id=$id;";
        comando.Parameters.AddWithValue("$nombre", p.Nombre);
        comando.Parameters.AddWithValue("$telefono", p.Telefono);
        comando.Parameters.AddWithValue("$email", p.Email);
        comando.Parameters.AddWithValue("$direccion", p.Direccion);
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    public void EliminarPropietario(int id)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Propietarios WHERE Id=$id;";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    // ===================== VETERINARIOS =====================

    public DataTable ObtenerVeterinarios()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Nombre, Especialidad, Telefono FROM Veterinarios ORDER BY Nombre;";
        return Llenar(comando);
    }

    public List<ComboItem> ObtenerVeterinariosCombo()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "SELECT Id, Nombre FROM Veterinarios ORDER BY Nombre;";
        using var lector = comando.ExecuteReader();
        var lista = new List<ComboItem>();
        while (lector.Read()) lista.Add(new ComboItem(lector.GetInt32(0), lector.GetString(1)));
        return lista;
    }

    public void CrearVeterinario(Veterinario v)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "INSERT INTO Veterinarios (Nombre, Especialidad, Telefono) VALUES ($nombre,$especialidad,$telefono);";
        comando.Parameters.AddWithValue("$nombre", v.Nombre);
        comando.Parameters.AddWithValue("$especialidad", v.Especialidad);
        comando.Parameters.AddWithValue("$telefono", v.Telefono);
        comando.ExecuteNonQuery();
    }

    public void ActualizarVeterinario(int id, Veterinario v)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Veterinarios SET Nombre=$nombre, Especialidad=$especialidad, Telefono=$telefono WHERE Id=$id;";
        comando.Parameters.AddWithValue("$nombre", v.Nombre);
        comando.Parameters.AddWithValue("$especialidad", v.Especialidad);
        comando.Parameters.AddWithValue("$telefono", v.Telefono);
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    public void EliminarVeterinario(int id)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Veterinarios WHERE Id=$id;";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    // ===================== MASCOTAS =====================

    public DataTable ObtenerMascotas()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = @"
            SELECT m.Id, m.Nombre, m.Especie, m.Raza, m.FechaNacimiento, p.Nombre AS Propietario, m.PropietarioId
            FROM Mascotas m
            JOIN Propietarios p ON p.Id = m.PropietarioId
            ORDER BY m.Nombre;";
        return Llenar(comando);
    }

    public List<ComboItem> ObtenerMascotasCombo()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = @"
            SELECT m.Id, m.Nombre || ' (' || p.Nombre || ')'
            FROM Mascotas m
            JOIN Propietarios p ON p.Id = m.PropietarioId
            ORDER BY m.Nombre;";
        using var lector = comando.ExecuteReader();
        var lista = new List<ComboItem>();
        while (lector.Read()) lista.Add(new ComboItem(lector.GetInt32(0), lector.GetString(1)));
        return lista;
    }

    public void CrearMascota(Mascota m)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "INSERT INTO Mascotas (PropietarioId, Nombre, Especie, Raza, FechaNacimiento) VALUES ($propietarioId,$nombre,$especie,$raza,$fecha);";
        comando.Parameters.AddWithValue("$propietarioId", m.PropietarioId);
        comando.Parameters.AddWithValue("$nombre", m.Nombre);
        comando.Parameters.AddWithValue("$especie", m.Especie);
        comando.Parameters.AddWithValue("$raza", m.Raza);
        comando.Parameters.AddWithValue("$fecha", m.FechaNacimiento.ToString("yyyy-MM-dd"));
        comando.ExecuteNonQuery();
    }

    public void ActualizarMascota(int id, Mascota m)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Mascotas SET PropietarioId=$propietarioId, Nombre=$nombre, Especie=$especie, Raza=$raza, FechaNacimiento=$fecha WHERE Id=$id;";
        comando.Parameters.AddWithValue("$propietarioId", m.PropietarioId);
        comando.Parameters.AddWithValue("$nombre", m.Nombre);
        comando.Parameters.AddWithValue("$especie", m.Especie);
        comando.Parameters.AddWithValue("$raza", m.Raza);
        comando.Parameters.AddWithValue("$fecha", m.FechaNacimiento.ToString("yyyy-MM-dd"));
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    public void EliminarMascota(int id)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Mascotas WHERE Id=$id;";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    // ===================== CITAS =====================

    public DataTable ObtenerCitas()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = @"
            SELECT c.Id, pa.Nombre AS Mascota, v.Nombre AS Veterinario, c.FechaHora, c.Motivo, c.Estado,
                   c.MascotaId, c.VeterinarioId
            FROM Citas c
            JOIN Mascotas pa ON pa.Id = c.MascotaId
            JOIN Veterinarios v ON v.Id = c.VeterinarioId
            ORDER BY c.FechaHora DESC;";
        return Llenar(comando);
    }

    public List<ComboItem> ObtenerCitasCombo()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = @"
            SELECT c.Id, m.Nombre || ' - ' || c.FechaHora
            FROM Citas c
            JOIN Mascotas m ON m.Id = c.MascotaId
            ORDER BY c.FechaHora DESC;";
        using var lector = comando.ExecuteReader();
        var lista = new List<ComboItem>();
        while (lector.Read()) lista.Add(new ComboItem(lector.GetInt32(0), lector.GetString(1)));
        return lista;
    }

    public void CrearCita(Cita c)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "INSERT INTO Citas (MascotaId, VeterinarioId, FechaHora, Motivo, Estado) VALUES ($mascotaId,$veterinarioId,$fecha,$motivo,$estado);";
        comando.Parameters.AddWithValue("$mascotaId", c.MascotaId);
        comando.Parameters.AddWithValue("$veterinarioId", c.VeterinarioId);
        comando.Parameters.AddWithValue("$fecha", c.FechaHora.ToString("yyyy-MM-dd HH:mm"));
        comando.Parameters.AddWithValue("$motivo", c.Motivo);
        comando.Parameters.AddWithValue("$estado", c.Estado);
        comando.ExecuteNonQuery();
    }

    public void ActualizarCita(int id, Cita c)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Citas SET MascotaId=$mascotaId, VeterinarioId=$veterinarioId, FechaHora=$fecha, Motivo=$motivo, Estado=$estado WHERE Id=$id;";
        comando.Parameters.AddWithValue("$mascotaId", c.MascotaId);
        comando.Parameters.AddWithValue("$veterinarioId", c.VeterinarioId);
        comando.Parameters.AddWithValue("$fecha", c.FechaHora.ToString("yyyy-MM-dd HH:mm"));
        comando.Parameters.AddWithValue("$motivo", c.Motivo);
        comando.Parameters.AddWithValue("$estado", c.Estado);
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    public void EliminarCita(int id)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Citas WHERE Id=$id;";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    // ===================== CONSULTAS =====================

    public DataTable ObtenerConsultas()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = @"
            SELECT co.Id, ci.FechaHora AS FechaCita, m.Nombre AS Mascota, co.Diagnostico, co.Tratamiento,
                   co.Observaciones, co.CitaId
            FROM Consultas co
            JOIN Citas ci ON ci.Id = co.CitaId
            JOIN Mascotas m ON m.Id = ci.MascotaId
            ORDER BY ci.FechaHora DESC;";
        return Llenar(comando);
    }

    public List<ComboItem> ObtenerConsultasCombo()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = @"
            SELECT co.Id, 'Consulta #' || co.Id || ' - ' || m.Nombre
            FROM Consultas co
            JOIN Citas ci ON ci.Id = co.CitaId
            JOIN Mascotas m ON m.Id = ci.MascotaId
            ORDER BY co.Id DESC;";
        using var lector = comando.ExecuteReader();
        var lista = new List<ComboItem>();
        while (lector.Read()) lista.Add(new ComboItem(lector.GetInt32(0), lector.GetString(1)));
        return lista;
    }

    public void CrearConsulta(Consulta c)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "INSERT INTO Consultas (CitaId, Diagnostico, Tratamiento, Observaciones) VALUES ($citaId,$diagnostico,$tratamiento,$observaciones);";
        comando.Parameters.AddWithValue("$citaId", c.CitaId);
        comando.Parameters.AddWithValue("$diagnostico", c.Diagnostico);
        comando.Parameters.AddWithValue("$tratamiento", c.Tratamiento);
        comando.Parameters.AddWithValue("$observaciones", c.Observaciones);
        comando.ExecuteNonQuery();
    }

    public void ActualizarConsulta(int id, Consulta c)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Consultas SET CitaId=$citaId, Diagnostico=$diagnostico, Tratamiento=$tratamiento, Observaciones=$observaciones WHERE Id=$id;";
        comando.Parameters.AddWithValue("$citaId", c.CitaId);
        comando.Parameters.AddWithValue("$diagnostico", c.Diagnostico);
        comando.Parameters.AddWithValue("$tratamiento", c.Tratamiento);
        comando.Parameters.AddWithValue("$observaciones", c.Observaciones);
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    public void EliminarConsulta(int id)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Consultas WHERE Id=$id;";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    // ===================== FACTURAS =====================

    public DataTable ObtenerFacturas()
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = @"
            SELECT f.Id, f.Monto, f.FechaEmision, f.Pagada, co.Diagnostico AS Consulta, f.ConsultaId
            FROM Facturas f
            JOIN Consultas co ON co.Id = f.ConsultaId
            ORDER BY f.FechaEmision DESC;";
        return Llenar(comando);
    }

    public void CrearFactura(Factura f)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "INSERT INTO Facturas (ConsultaId, Monto, FechaEmision, Pagada) VALUES ($consultaId,$monto,$fecha,$pagada);";
        comando.Parameters.AddWithValue("$consultaId", f.ConsultaId);
        comando.Parameters.AddWithValue("$monto", f.Monto);
        comando.Parameters.AddWithValue("$fecha", f.FechaEmision.ToString("yyyy-MM-dd"));
        comando.Parameters.AddWithValue("$pagada", f.Pagada ? 1 : 0);
        comando.ExecuteNonQuery();
    }

    public void ActualizarFactura(int id, Factura f)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "UPDATE Facturas SET ConsultaId=$consultaId, Monto=$monto, FechaEmision=$fecha, Pagada=$pagada WHERE Id=$id;";
        comando.Parameters.AddWithValue("$consultaId", f.ConsultaId);
        comando.Parameters.AddWithValue("$monto", f.Monto);
        comando.Parameters.AddWithValue("$fecha", f.FechaEmision.ToString("yyyy-MM-dd"));
        comando.Parameters.AddWithValue("$pagada", f.Pagada ? 1 : 0);
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }

    public void EliminarFactura(int id)
    {
        using var conexion = AbrirConexion();
        using var comando = conexion.CreateCommand();
        comando.CommandText = "DELETE FROM Facturas WHERE Id=$id;";
        comando.Parameters.AddWithValue("$id", id);
        comando.ExecuteNonQuery();
    }
}
