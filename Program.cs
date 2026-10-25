using VeterinariaClinica.Formularios;
using VeterinariaClinica.Persistencia;

namespace VeterinariaClinica;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        try
        {
            var inicializador = new InicializadorBaseDatos();
            inicializador.Inicializar();
            var repositorio = new RepositorioVeterinaria(inicializador.CadenaConexion);
            Application.Run(new MainForm(repositorio));
        }
        catch (Exception error)
        {
            MessageBox.Show(
                $"No fue posible iniciar el sistema: {error.Message}",
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
