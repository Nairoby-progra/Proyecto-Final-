using System.Drawing;
using System.Windows.Forms;
using VeterinariaClinica.Persistencia;

namespace VeterinariaClinica.Formularios;

public sealed class MainForm : Form
{
    private readonly RepositorioVeterinaria _repositorio;

    public MainForm(RepositorioVeterinaria repositorio)
    {
        _repositorio = repositorio;
        Text = "Sistema de Control Clínico Veterinario";
        Width = 440;
        Height = 460;
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        var titulo = new Label
        {
            Text = "Clínica Veterinaria",
            Font = new Font("Segoe UI", 18, FontStyle.Bold),
            AutoSize = true,
            Left = 30,
            Top = 25
        };
        var subtitulo = new Label
        {
            Text = "Seleccione un módulo para administrar:",
            Font = new Font("Segoe UI", 9, FontStyle.Regular),
            AutoSize = true,
            Left = 32,
            Top = 65
        };
        Controls.Add(titulo);
        Controls.Add(subtitulo);

        AgregarBoton("Propietarios", 100, () => new PropietariosForm(_repositorio));
        AgregarBoton("Mascotas", 148, () => new MascotasForm(_repositorio));
        AgregarBoton("Veterinarios", 196, () => new VeterinariosForm(_repositorio));
        AgregarBoton("Citas", 244, () => new CitasForm(_repositorio));
        AgregarBoton("Consultas", 292, () => new ConsultasForm(_repositorio));
        AgregarBoton("Facturas", 340, () => new FacturasForm(_repositorio));
    }

    private void AgregarBoton(string texto, int top, Func<Form> crearFormulario)
    {
        var boton = new Button { Text = texto, Left = 30, Top = top, Width = 360, Height = 38 };
        boton.Click += (_, _) =>
        {
            using var formulario = crearFormulario();
            formulario.ShowDialog(this);
        };
        Controls.Add(boton);
    }
}
