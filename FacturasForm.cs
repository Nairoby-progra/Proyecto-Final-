using System.Data;
using System.Windows.Forms;
using VeterinariaClinica.Dominio;
using VeterinariaClinica.Persistencia;

namespace VeterinariaClinica.Formularios;

public sealed class FacturasForm : EntidadFormBase
{
    private readonly ComboBox _cmbConsulta = new()
    { Left = 150, Top = 20, Width = 320, DropDownStyle = ComboBoxStyle.DropDownList, ValueMember = "Id" };
    private readonly NumericUpDown _numMonto = new()
    { Left = 150, Top = 55, Width = 160, DecimalPlaces = 2, Maximum = 1000000, Minimum = 0 };
    private readonly DateTimePicker _dtEmision = new()
    { Left = 150, Top = 90, Width = 320, Format = DateTimePickerFormat.Short };
    private readonly CheckBox _chkPagada = new() { Left = 150, Top = 125, Width = 160, Text = "Pagada" };

    public FacturasForm(RepositorioVeterinaria repositorio) : base(repositorio, "Facturas")
    {
        _cmbConsulta.DataSource = Repositorio.ObtenerConsultasCombo();

        PanelCampos.Controls.Add(new Label { Text = "Consulta:", Left = 20, Top = 23, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Monto:", Left = 20, Top = 58, Width = 120 });
        PanelCampos.Controls.Add(new Label { Text = "Fecha de emisión:", Left = 20, Top = 93, Width = 120 });
        PanelCampos.Controls.AddRange(new Control[] { _cmbConsulta, _numMonto, _dtEmision, _chkPagada });
    }

    protected override string[] ColumnasOcultas => new[] { "ConsultaId" };

    protected override DataTable ObtenerDatos() => Repositorio.ObtenerFacturas();

    protected override void GuardarRegistro(int? id)
    {
        if (_cmbConsulta.SelectedValue is not int consultaId) throw new ArgumentException("Seleccione una consulta.");

        var factura = new Factura(consultaId, _numMonto.Value, _dtEmision.Value.Date, _chkPagada.Checked);
        if (id is null) Repositorio.CrearFactura(factura);
        else Repositorio.ActualizarFactura(id.Value, factura);
    }

    protected override void EliminarRegistro(int id) => Repositorio.EliminarFactura(id);

    protected override void CargarSeleccionEnCamposImpl(DataGridViewRow fila)
    {
        if (decimal.TryParse(fila.Cells["Monto"].Value?.ToString(), out var monto)) _numMonto.Value = monto;
        if (DateTime.TryParse(fila.Cells["FechaEmision"].Value?.ToString(), out var fecha)) _dtEmision.Value = fecha;
        _chkPagada.Checked = fila.Cells["Pagada"].Value?.ToString() == "1";
        if (fila.Cells["ConsultaId"].Value is not null)
            _cmbConsulta.SelectedValue = Convert.ToInt32(fila.Cells["ConsultaId"].Value);
    }

    protected override void LimpiarCampos()
    {
        _numMonto.Value = 0;
        _dtEmision.Value = DateTime.Today;
        _chkPagada.Checked = false;
        if (_cmbConsulta.Items.Count > 0) _cmbConsulta.SelectedIndex = 0;
    }
}
