using System.Reflection;
using Microsoft.Data.Sqlite;

namespace VeterinariaClinica.Persistencia;

/// <summary>Crea (si no existe) el archivo clinica.db y sus tablas a partir del script embebido.</summary>
public sealed class InicializadorBaseDatos
{
    public string CadenaConexion { get; }

    public InicializadorBaseDatos()
    {
        var ruta = Path.Combine(AppContext.BaseDirectory, "clinica.db");
        CadenaConexion = $"Data Source={ruta}";
    }

    public void Inicializar()
    {
        using var conexion = new SqliteConnection(CadenaConexion);
        conexion.Open();

        using (var pragma = conexion.CreateCommand())
        {
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();
        }

        var script = LeerScriptEmbebido();
        foreach (var sentencia in script.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            if (string.IsNullOrWhiteSpace(sentencia)) continue;
            using var comando = conexion.CreateCommand();
            comando.CommandText = sentencia;
            comando.ExecuteNonQuery();
        }
    }

    private static string LeerScriptEmbebido()
    {
        var ensamblado = Assembly.GetExecutingAssembly();
        var nombreRecurso = ensamblado.GetManifestResourceNames()
            .First(nombre => nombre.EndsWith("crear_tablas.sql", StringComparison.OrdinalIgnoreCase));
        using var flujo = ensamblado.GetManifestResourceStream(nombreRecurso)
            ?? throw new InvalidOperationException("No se encontró el script de creación de tablas.");
        using var lector = new StreamReader(flujo);
        return lector.ReadToEnd();
    }
}
