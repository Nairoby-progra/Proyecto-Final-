using System.Data;
using System.Windows.Forms;
using VeterinariaClinica.Dominio;
using VeterinariaClinica.Persistencia;

namespace VeterinariaClinica.Formularios;

public sealed class PropietariosForm : EntidadFormBase
{
    private readonly TextBox _txtNombre = new() { Left = 150, Top = 20, Width = 320 };
    private readonly TextBox _txtTelefono = new() { Left = 150, Top = 55, Width = 320 };
    private readonly TextBox _txtEmail = new() { Left = 150, Top = 90, Width = 320 };
    private readonly TextBox _txtDireccion = new() { Left = 150, Top = 125, Width = 320 };

    public PropietariosForm(RepositorioVeterinaria repositorio) : base(repositorio, "Propietarios")
    {
        PanelCampos.Controls.Add(new Label { Text = "Nombre:", Left = 20, Top = 23, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Teléfono:", Left = 20, Top = 58, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Email:", Left = 20, Top = 93, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Dirección:", Left = 20, Top = 128, Width = 120 });
        PanelCampos.Controls.AddRange(new Control[] { _txtNombre, _txtTelefono, _txtEmail, _txtDireccion });
    }

    protected override DataTable ObtenerDatos() => Repositorio.ObtenerPropietarios();

    protected override void GuardarRegistro(int? id)
    {
        var propietario = new Propietario(_txtNombre.Text, _txtTelefono.Text, _txtEmail.Text, _txtDireccion.Text);
        if (id is null) Repositorio.CrearPropietario(propietario);
        else Repositorio.ActualizarPropietario(id.Value, propietario);
    }

    protected override void EliminarRegistro(int id) => Repositorio.EliminarPropietario(id);

    protected override void CargarSeleccionEnCamposImpl(DataGridViewRow fila)
    {
        _txtNombre.Text = fila.Cells["Nombre"].Value?.ToString() ?? "";
        _txtTelefono.Text = fila.Cells["Telefono"].Value?.ToString() ?? "";
        _txtEmail.Text = fila.Cells["Email"].Value?.ToString() ?? "";
        _txtDireccion.Text = fila.Cells["Direccion"].Value?.ToString() ?? "";
    }

    protected override void LimpiarCampos()
    {
        _txtNombre.Clear();
        _txtTelefono.Clear();
        _txtEmail.Clear();
        _txtDireccion.Clear();
    }
}
