using System.Data;
using System.Windows.Forms;
using VeterinariaClinica.Dominio;
using VeterinariaClinica.Persistencia;

namespace VeterinariaClinica.Formularios;

public sealed class VeterinariosForm : EntidadFormBase
{
    private readonly TextBox _txtNombre = new() { Left = 150, Top = 20, Width = 320 };
    private readonly TextBox _txtEspecialidad = new() { Left = 150, Top = 55, Width = 320 };
    private readonly TextBox _txtTelefono = new() { Left = 150, Top = 90, Width = 320 };

    public VeterinariosForm(RepositorioVeterinaria repositorio) : base(repositorio, "Veterinarios")
    {
        PanelCampos.Controls.Add(new Label { Text = "Nombre:", Left = 20, Top = 23, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Especialidad:", Left = 20, Top = 58, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Teléfono:", Left = 20, Top = 93, Width = 120 });
        PanelCampos.Controls.AddRange(new Control[] { _txtNombre, _txtEspecialidad, _txtTelefono });
    }

    protected override DataTable ObtenerDatos() => Repositorio.ObtenerVeterinarios();

    protected override void GuardarRegistro(int? id)
    {
        var veterinario = new Veterinario(_txtNombre.Text, _txtEspecialidad.Text, _txtTelefono.Text);
        if (id is null) Repositorio.CrearVeterinario(veterinario);
        else Repositorio.ActualizarVeterinario(id.Value, veterinario);
    }

    protected override void EliminarRegistro(int id) => Repositorio.EliminarVeterinario(id);

    protected override void CargarSeleccionEnCamposImpl(DataGridViewRow fila)
    {
        _txtNombre.Text = fila.Cells["Nombre"].Value?.ToString() ?? "";
        _txtEspecialidad.Text = fila.Cells["Especialidad"].Value?.ToString() ?? "";
        _txtTelefono.Text = fila.Cells["Telefono"].Value?.ToString() ?? "";
    }

    protected override void LimpiarCampos()
    {
        _txtNombre.Clear();
        _txtEspecialidad.Clear();
        _txtTelefono.Clear();
    }
}
