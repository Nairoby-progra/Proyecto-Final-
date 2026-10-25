using System.Data;
using System.Windows.Forms;
using VeterinariaClinica.Dominio;
using VeterinariaClinica.Persistencia;

namespace VeterinariaClinica.Formularios;

public sealed class CitasForm : EntidadFormBase
{
    private readonly ComboBox _cmbMascota = new()
    { Left = 150, Top = 20, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList, ValueMember = "Id" };
    private readonly ComboBox _cmbVeterinario = new()
    { Left = 150, Top = 55, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList, ValueMember = "Id" };
    private readonly DateTimePicker _dtFechaHora = new()
    {
        Left = 150, Top = 90, Width = 320,
        Format = DateTimePickerFormat.Custom, CustomFormat = "dd/MM/yyyy HH:mm", ShowUpDown = true
    };
    private readonly TextBox _txtMotivo = new() { Left = 150, Top = 125, Width = 320 };
    private readonly ComboBox _cmbEstado = new()
    { Left = 150, Top = 160, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList };

    public CitasForm(RepositorioVeterinaria repositorio) : base(repositorio, "Citas")
    {
        _cmbMascota.DataSource = Repositorio.ObtenerMascotasCombo();
        _cmbVeterinario.DataSource = Repositorio.ObtenerVeterinariosCombo();
        _cmbEstado.Items.AddRange(new object[] { "Programada", "Atendida", "Cancelada" });
        _cmbEstado.SelectedIndex = 0;

        PanelCampos.Controls.Add(new Label { Text = "Mascota:", Left = 20, Top = 23, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Veterinario:", Left = 20, Top = 58, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Fecha y hora:", Left = 20, Top = 93, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Motivo:", Left = 20, Top = 128, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Estado:", Left = 20, Top = 163, Width = 120 });
        PanelCampos.Controls.AddRange(new Control[] { _cmbMascota, _cmbVeterinario, _dtFechaHora, _txtMotivo, _cmbEstado });
    }

    protected override string[] ColumnasOcultas => new[] { "MascotaId", "VeterinarioId" };

    protected override DataTable ObtenerDatos() => Repositorio.ObtenerCitas();

    protected override void GuardarRegistro(int? id)
    {
        if (_cmbMascota.SelectedValue is not int mascotaId) throw new ArgumentException("Seleccione una mascota.");
        if (_cmbVeterinario.SelectedValue is not int veterinarioId) throw new ArgumentException("Seleccione un veterinario.");

        var cita = new Cita(mascotaId, veterinarioId, _dtFechaHora.Value, _txtMotivo.Text, _cmbEstado.Text);
        if (id is null) Repositorio.CrearCita(cita);
        else Repositorio.ActualizarCita(id.Value, cita);
    }

    protected override void EliminarRegistro(int id) => Repositorio.EliminarCita(id);

    protected override void CargarSeleccionEnCamposImpl(DataGridViewRow fila)
    {
        _txtMotivo.Text = fila.Cells["Motivo"].Value?.ToString() ?? "";
        _cmbEstado.Text = fila.Cells["Estado"].Value?.ToString() ?? "Programada";
        if (DateTime.TryParse(fila.Cells["FechaHora"].Value?.ToString(), out var fecha))
            _dtFechaHora.Value = fecha;
        if (fila.Cells["MascotaId"].Value is not null)
            _cmbMascota.SelectedValue = Convert.ToInt32(fila.Cells["MascotaId"].Value);
        if (fila.Cells["VeterinarioId"].Value is not null)
            _cmbVeterinario.SelectedValue = Convert.ToInt32(fila.Cells["VeterinarioId"].Value);
    }

    protected override void LimpiarCampos()
    {
        _txtMotivo.Clear();
        _dtFechaHora.Value = DateTime.Now;
        _cmbEstado.SelectedIndex = 0;
        if (_cmbMascota.Items.Count > 0) _cmbMascota.SelectedIndex = 0;
        if (_cmbVeterinario.Items.Count > 0) _cmbVeterinario.SelectedIndex = 0;
    }
}
