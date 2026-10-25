using System.Data;
using System.Windows.Forms;
using VeterinariaClinica.Dominio;
using VeterinariaClinica.Persistencia;

namespace VeterinariaClinica.Formularios;

public sealed class ConsultasForm : EntidadFormBase
{
    private readonly ComboBox _cmbCita = new()
    { Left = 150, Top = 20, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList, ValueMember = "Id" };
    private readonly TextBox _txtDiagnostico = new() { Left = 150, Top = 55, Width = 320 };
    private readonly TextBox _txtTratamiento = new() { Left = 150, Top = 90, Width = 320 };
    private readonly TextBox _txtObservaciones = new()
    { Left = 150, Top = 125, Width = 320, Height = 55, Multiline = true };

    public ConsultasForm(RepositorioVeterinaria repositorio) : base(repositorio, "Consultas")
    {
        _cmbCita.DataSource = Repositorio.ObtenerCitasCombo();

        PanelCampos.Controls.Add(new Label { Text = "Cita:", Left = 20, Top = 23, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Diagnóstico:", Left = 20, Top = 58, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Tratamiento:", Left = 20, Top = 93, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Observaciones:", Left = 20, Top = 128, Width = 120 });
        PanelCampos.Controls.AddRange(new Control[] { _cmbCita, _txtDiagnostico, _txtTratamiento, _txtObservaciones });
    }

    protected override string[] ColumnasOcultas => new[] { "CitaId" };

    protected override DataTable ObtenerDatos() => Repositorio.ObtenerConsultas();

    protected override void GuardarRegistro(int? id)
    {
        if (_cmbCita.SelectedValue is not int citaId) throw new ArgumentException("Seleccione una cita.");

        var consulta = new Consulta(citaId, _txtDiagnostico.Text, _txtTratamiento.Text, _txtObservaciones.Text);
        if (id is null) Repositorio.CrearConsulta(consulta);
        else Repositorio.ActualizarConsulta(id.Value, consulta);
    }

    protected override void EliminarRegistro(int id) => Repositorio.EliminarConsulta(id);

    protected override void CargarSeleccionEnCamposImpl(DataGridViewRow fila)
    {
        _txtDiagnostico.Text = fila.Cells["Diagnostico"].Value?.ToString() ?? "";
        _txtTratamiento.Text = fila.Cells["Tratamiento"].Value?.ToString() ?? "";
        _txtObservaciones.Text = fila.Cells["Observaciones"].Value?.ToString() ?? "";
        if (fila.Cells["CitaId"].Value is not null)
            _cmbCita.SelectedValue = Convert.ToInt32(fila.Cells["CitaId"].Value);
    }

    protected override void LimpiarCampos()
    {
        _txtDiagnostico.Clear();
        _txtTratamiento.Clear();
        _txtObservaciones.Clear();
        if (_cmbCita.Items.Count > 0) _cmbCita.SelectedIndex = 0;
    }
}
