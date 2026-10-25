using System.Data;
using System.Drawing;
using System.Windows.Forms;
using VeterinariaClinica.Persistencia;

namespace VeterinariaClinica.Formularios;

/// <summary>
/// Formulario base con la mecánica común a todas las pantallas CRUD: una
/// grilla arriba, un panel de campos propio de cada entidad (lo llenan las
/// clases derivadas) y los botones Nuevo/Guardar/Eliminar/Refrescar abajo.
/// </summary>
public abstract class EntidadFormBase : Form
{
    protected readonly RepositorioVeterinaria Repositorio;
    protected readonly DataGridView Grid;
    protected readonly Panel PanelCampos;
    protected int? IdSeleccionado;

    protected EntidadFormBase(RepositorioVeterinaria repositorio, string titulo)
    {
        Repositorio = repositorio;
        Text = titulo;
        Width = 780;
        Height = 580;
        StartPosition = FormStartPosition.CenterParent;
        MinimumSize = new Size(640, 480);

        Grid = new DataGridView
        {
            Dock = DockStyle.Top,
            Height = 250,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            MultiSelect = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        Grid.SelectionChanged += (_, _) => CargarSeleccionEnCampos();
        Controls.Add(Grid);

        PanelCampos = new Panel { Dock = DockStyle.Top, Height = 200, Padding = new Padding(12) };
        Controls.Add(PanelCampos);

        var panelBotones = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 54, Padding = new Padding(10) };
        var btnNuevo = new Button { Text = "Nuevo", Width = 110, Height = 32 };
        var btnGuardar = new Button { Text = "Guardar", Width = 110, Height = 32 };
        var btnEliminar = new Button { Text = "Eliminar", Width = 110, Height = 32 };
        var btnRefrescar = new Button { Text = "Refrescar", Width = 110, Height = 32 };
        btnNuevo.Click += (_, _) => { IdSeleccionado = null; Grid.ClearSelection(); LimpiarCampos(); };
        btnGuardar.Click += (_, _) => ManejarGuardar();
        btnEliminar.Click += (_, _) => ManejarEliminar();
        btnRefrescar.Click += (_, _) => Refrescar();
        panelBotones.Controls.AddRange(new Control[] { btnNuevo, btnGuardar, btnEliminar, btnRefrescar });
        Controls.Add(panelBotones);

        // El panel de botones debe quedar pegado abajo del todo, y la grilla
        // debe crecer para llenar el espacio restante.
        Grid.Dock = DockStyle.Fill;

        Load += (_, _) => Refrescar();
    }

    /// <summary>Nombre de la columna que identifica cada fila en la grilla (normalmente "Id").</summary>
    protected virtual string ColumnaId => "Id";

    /// <summary>Columnas auxiliares (claves foráneas) que no deben mostrarse al usuario.</summary>
    protected virtual string[] ColumnasOcultas => Array.Empty<string>();

    protected abstract DataTable ObtenerDatos();
    protected abstract void GuardarRegistro(int? id);
    protected abstract void EliminarRegistro(int id);
    protected abstract void CargarSeleccionEnCamposImpl(DataGridViewRow fila);
    protected abstract void LimpiarCampos();

    protected void Refrescar()
    {
        Grid.DataSource = ObtenerDatos();
        foreach (var nombre in ColumnasOcultas)
        {
            if (Grid.Columns.Contains(nombre)) Grid.Columns[nombre]!.Visible = false;
        }
        IdSeleccionado = null;
        LimpiarCampos();
    }

    private void CargarSeleccionEnCampos()
    {
        if (Grid.CurrentRow is null || Grid.CurrentRow.IsNewRow) return;
        IdSeleccionado = Convert.ToInt32(Grid.CurrentRow.Cells[ColumnaId].Value);
        CargarSeleccionEnCamposImpl(Grid.CurrentRow);
    }

    private void ManejarGuardar()
    {
        try
        {
            GuardarRegistro(IdSeleccionado);
            Refrescar();
        }
        catch (Exception error)
        {
            MessageBox.Show(error.Message, "Error al guardar", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void ManejarEliminar()
    {
        if (IdSeleccionado is null)
        {
            MessageBox.Show("Seleccione un registro de la tabla.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var confirmar = MessageBox.Show(
            "¿Eliminar el registro seleccionado? Esta acción no se puede deshacer.",
            "Confirmar",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);
        if (confirmar != DialogResult.Yes) return;

        try
        {
            EliminarRegistro(IdSeleccionado.Value);
            Refrescar();
        }
        catch (Exception error)
        {
            MessageBox.Show(error.Message, "Error al eliminar", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
