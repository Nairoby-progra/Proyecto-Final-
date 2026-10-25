using System.Data;
using System.Windows.Forms;
using VeterinariaClinica.Dominio;
using VeterinariaClinica.Persistencia;

namespace VeterinariaClinica.Formularios;

public sealed class MascotasForm : EntidadFormBase
{
    private readonly ComboBox _cmbPropietario = new()
    { Left = 150, Top = 20, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList, ValueMember = "Id" };
    private readonly TextBox _txtNombre = new() { Left = 150, Top = 55, Width = 320 };
    private readonly TextBox _txtEspecie = new() { Left = 150, Top = 90, Width = 320 };
    private readonly TextBox _txtRaza = new() { Left = 150, Top = 125, Width = 320 };
    private readonly DateTimePicker _dtNacimiento = new()
    { Left = 150, Top = 160, Width = 320, Format = DateTimePickerFormat.Short };

    public MascotasForm(RepositorioVeterinaria repositorio) : base(repositorio, "Mascotas")
    {
        _cmbPropietario.DataSource = Repositorio.ObtenerPropietariosCombo();

        PanelCampos.Controls.Add(new Label { Text = "Propietario:", Left = 20, Top = 23, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Nombre:", Left = 20, Top = 58, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Especie:", Left = 20, Top = 93, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Raza:", Left = 20, Top = 128, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Fecha de nacimiento:", Left = 20, Top = 163, Width = 120 });
        PanelCampos.Controls.AddRange(new Control[] { _cmbPropietario, _txtNombre, _txtEspecie, _txtRaza, _dtNacimiento });
    }

    protected override string[] ColumnasOcultas => new[] { "PropietarioId" };

    protected override DataTable ObtenerDatos() => Repositorio.ObtenerMascotas();

    protected override void GuardarRegistro(int? id)
    {
        if (_cmbPropietario.SelectedValue is not int propietarioId)
            throw new ArgumentException("Seleccione un propietario.");

        var mascota = new Mascota(propietarioId, _txtNombre.Text, _txtEspecie.Text, _txtRaza.Text, _dtNacimiento.Value.Date);
        if (id is null) Repositorio.CrearMascota(mascota);
        else Repositorio.ActualizarMascota(id.Value, mascota);
    }

    protected override void EliminarRegistro(int id) => Repositorio.EliminarMascota(id);

    protected override void CargarSeleccionEnCamposImpl(DataGridViewRow fila)
    {
        _txtNombre.Text = fila.Cells["Nombre"].Value?.ToString() ?? "";
        _txtEspecie.Text = fila.Cells["Especie"].Value?.ToString() ?? "";
        _txtRaza.Text = fila.Cells["Raza"].Value?.ToString() ?? "";
        if (DateTime.TryParse(fila.Cells["FechaNacimiento"].Value?.ToString(), out var fecha))
            _dtNacimiento.Value = fecha;
        if (fila.Cells["PropietarioId"].Value is not null)
            _cmbPropietario.SelectedValue = Convert.ToInt32(fila.Cells["PropietarioId"].Value);
    }

    protected override void LimpiarCampos()
    {
        _txtNombre.Clear();
        _txtEspecie.Clear();
        _txtRaza.Clear();
        _dtNacimiento.Value = DateTime.Today;
        if (_cmbPropietario.Items.Count > 0) _cmbPropietario.SelectedIndex = 0;
    }
}
