using Modelos.Entidades;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Vista.GerenteAyuda;
using Vista.GerenteCalendario;
using Vista.GerenteClientes;
using Vista.GerenteDocumentacion;
using Vista.GerenteEmpleados;
using Vista.GerenteInventario;
using Vista.GerenteProveedores;
using Vista.GerenteRegistroPagos;
using Vista.GerenteRegistroVentas;
using Vista.GerenteReportes;
using Vista.IniciarSesion;
using Vista.UsuariosCreados;
using Vista.Utilidades;

namespace Vista.GerenteInicio
{
    public partial class frmMenuGerente : Form
    {
        public frmMenuGerente()
        {
            try
            {
                InitializeComponent();

                Redondeo.RedondearFig(btnCerrarSesionGere, 16);
                QuestPDF.Settings.License = LicenseType.Community;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al inicializar el formulario: " + ex.Message,
                        "ERROR-FORMULARIO-101", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Cambio de formularios
        private Form activeForm = null;

        private void abrirForm(Form formularioAbrir)
        {
            try
            {
                if (activeForm != null && activeForm.GetType() == formularioAbrir.GetType())
                {
                    return;
                }

                if (activeForm != null)
                {
                    activeForm.Close();
                    pnlContenedorVistas.Controls.Remove(activeForm);
                    activeForm.Dispose();
                }

                activeForm = formularioAbrir;
                formularioAbrir.TopLevel = false;
                formularioAbrir.FormBorderStyle = FormBorderStyle.None;
                formularioAbrir.Dock = DockStyle.Fill;

                pnlContenedorVistas.Controls.Add(formularioAbrir);
                formularioAbrir.BringToFront();
                formularioAbrir.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cambiar de formulario: " + ex.Message,
                        "ERROR-CAMBIO-103", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnInicioGere_Click(object sender, EventArgs e)
        {
            abrirForm(new frmInicioGerente());
            ActivarBoton(btnInicioGere);
        }

        private void btnInventarioGere_Click(object sender, EventArgs e)
        {
            abrirForm(new frmInventario());
            ActivarBoton(btnInventarioGere);
        }

        private void btnClienteGere_Click(object sender, EventArgs e)
        {
            abrirForm(new frmClientesTotales());
            ActivarBoton(btnClienteGere);
        }

        private void btnCalendarioGere_Click(object sender, EventArgs e)
        {
            abrirForm(new frmCalendario());
            ActivarBoton(btnCalendarioGere);
        }

        private void btnRegisPagoGere_Click(object sender, EventArgs e)
        {
            abrirForm(new frmRegistroPago());
            ActivarBoton(btnRegisPagoGere);
        }

        private void btnRegistroVentas_Click(object sender, EventArgs e)
        {
            abrirForm(new frmRegistroVentas());
            ActivarBoton(btnRegistroVentas);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            abrirForm(new frmDocumentos());
            ActivarBoton(button3);
        }

        private void btnEmpleado_Click(object sender, EventArgs e)
        {
            abrirForm(new frmEmpleadosGerente());
            ActivarBoton(btnEmpleado);
        }

        private void btnProveedores_Click(object sender, EventArgs e)
        {
            abrirForm(new frmProveedores());
            ActivarBoton(btnProveedores);
        }

        private void btnReportes_Click(object sender, EventArgs e)
        {
            abrirForm(new frmReportes());
            ActivarBoton(btnReportes);
        }

        private void btnUsuario_Click(object sender, EventArgs e)
        {
            abrirForm(new frmUsuarios());
            ActivarBoton(btnUsuario);
        }

        private void btnCerrarSesionGere_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult respuesta = MessageBox.Show("¿Está seguro que desea cerrar sesión?",
                   "INFO-CERRAR-SESION", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (respuesta != DialogResult.Yes) return;

                this.Close();

                activeForm = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cerrar sesión: " + ex.Message,
                        "ERROR-CERRARSESION-014", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RestaurarBotones()
        {
            try
            {
                btnInicioGere.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnInventarioGere.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnClienteGere.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnCalendarioGere.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnRegisPagoGere.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnRegistroVentas.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                button3.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnEmpleado.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnProveedores.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnUsuario.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnAyyuda.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnUsuario.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
                btnReportes.BackColor = System.Drawing.Color.FromArgb(241, 206, 184);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al restaurar botones: " + ex.Message);
            }
        }

        private void ActivarBoton(Button botones)
        {
            RestaurarBotones();

            botones.BackColor = System.Drawing.Color.FromArgb(253, 241, 217);
        }

        private void btnAyyuda_Click(object sender, EventArgs e)
        {
            abrirForm(new frmAyuda());
            ActivarBoton(btnAyyuda);
        }

        private void frmMenuGerente_Load(object sender, EventArgs e)
        {
            try
            {
                CargarVentasMensuales();
                CargarProductosMasVendidos();
                CargarEmpleadosMasVentas();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar el dashboard: " + ex.Message,
                        "ERROR-CARGADATOS-008", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarVentasMensuales()
        {
            try
            {
                DataTable dt = ReportesGraficos.VentasMensuales();

                chartVentasMensuales.Series.Clear();
                chartVentasMensuales.Titles.Clear();
                chartVentasMensuales.ChartAreas.Clear();

                ChartArea area = new ChartArea("Main");
                area.BackColor = System.Drawing.Color.FromArgb(253, 241, 217);
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(230, 210, 190);
                area.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(64, 6, 6);
                area.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(64, 6, 6);
                area.AxisX.LabelStyle.Font = new Font("Book Antiqua", 9);
                area.AxisY.LabelStyle.Font = new Font("Book Antiqua", 9);
                chartVentasMensuales.ChartAreas.Add(area);

                Series serie = new Series("Ingresos")
                {
                    ChartType = SeriesChartType.Line,
                    Color = System.Drawing.Color.FromArgb(30, 110, 200),
                    BorderWidth = 3,
                    MarkerStyle = MarkerStyle.Circle,
                    MarkerSize = 7,
                    MarkerColor = System.Drawing.Color.FromArgb(208, 112, 3)
                };

                foreach (DataRow fila in dt.Rows)
                {
                    serie.Points.AddXY(fila["EtiquetaMes"].ToString(),
                                       Convert.ToDecimal(fila["TotalVentas"]));
                }

                chartVentasMensuales.Series.Add(serie);
                chartVentasMensuales.Titles.Add("Ingresos");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error gráfico ventas: " + ex.Message);
            }
        }

        private void CargarProductosMasVendidos()
        {
            try
            {
                DataTable dt = ReportesGraficos.ProductosMasVendidos();

                chartProductosVendidos.Series.Clear();
                chartProductosVendidos.Titles.Clear();
                chartProductosVendidos.ChartAreas.Clear();

                ChartArea area = new ChartArea("Main");
                area.BackColor = System.Drawing.Color.FromArgb(253, 241, 217);
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisX.LabelStyle.Angle = -45;
                area.AxisX.LabelStyle.Font = new Font("Book Antiqua", 8);
                area.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(64, 6, 6);
                area.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(64, 6, 6);
                chartProductosVendidos.ChartAreas.Add(area);

                Series serie = new Series("Productos")
                {
                    ChartType = SeriesChartType.Column,
                    Color = System.Drawing.Color.FromArgb(180, 120, 220),
                    IsValueShownAsLabel = true
                };

                foreach (DataRow fila in dt.Rows)
                {
                    serie.Points.AddXY(fila["NombreProducto"].ToString(),
                                       Convert.ToInt32(fila["TotalVendido"]));
                }

                chartProductosVendidos.Series.Add(serie);
                chartProductosVendidos.Titles.Add("Productos más vendidos");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error gráfico productos: " + ex.Message);
            }
        }

        private void CargarEmpleadosMasVentas()
        {
            try
            {
                DataTable dt = ReportesGraficos.EmpleadosMasVentas();

                chartEmpleadosVentas.Series.Clear();
                chartEmpleadosVentas.Titles.Clear();
                chartEmpleadosVentas.ChartAreas.Clear();

                ChartArea area = new ChartArea("Main");
                area.BackColor = System.Drawing.Color.FromArgb(253, 241, 217);
                area.AxisX.MajorGrid.Enabled = false;
                area.AxisX.LabelStyle.Angle = -35;
                area.AxisX.LabelStyle.Font = new Font("Book Antiqua", 8);
                area.AxisX.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(64, 6, 6);
                area.AxisY.LabelStyle.ForeColor = System.Drawing.Color.FromArgb(64, 6, 6);
                chartEmpleadosVentas.ChartAreas.Add(area);

                Series serie = new Series("Ventas")
                {
                    ChartType = SeriesChartType.Column,
                    Color = System.Drawing.Color.FromArgb(60, 130, 220),
                    IsValueShownAsLabel = true,
                    LabelFormat = "C0"
                };

                foreach (DataRow fila in dt.Rows)
                {
                    serie.Points.AddXY(fila["NombreEmpleado"].ToString(),
                                       Convert.ToDecimal(fila["TotalVendido"]));
                }

                chartEmpleadosVentas.Series.Add(serie);
                chartEmpleadosVentas.Titles.Add("Empleados con más ventas");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error gráfico empleados: " + ex.Message);
            }
        }

        private bool GenerarReporteConConfirmacion(string nombreArchivo, Action<string> generadorPdf)
        {
            try
            {
                DialogResult conf = MessageBox.Show(
                    "¿Deseas generar este reporte ahora?",
                    "Confirmar generación",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);

                if (conf != DialogResult.Yes) return false;

                string carpeta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "ReportesEventosMaquilishuat");

                if (!Directory.Exists(carpeta))
                    Directory.CreateDirectory(carpeta);

                string rutaCompleta = Path.Combine(carpeta, nombreArchivo);

                generadorPdf(rutaCompleta);

                AbrirPdf(rutaCompleta);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al generar el reporte: " + ex.Message,
                        "ERROR-PDF", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void AbrirPdf(string rutaCompleta)
        {
            try
            {
                if (File.Exists(rutaCompleta))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = rutaCompleta,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("No se pudo abrir el PDF: " + ex.Message);
            }
        }

        private void btnReporteVentas_Click(object sender, EventArgs e)
        {
            string nombre = $"Reporte_Ventas_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            GenerarReporteConConfirmacion(nombre, GenerarPdfVentas);
        }

        private void GenerarPdfVentas(string rutaCompleta)
        {
            DataSet ds = ReportesGraficos.ReporteVentasTotales();
            DataTable resumen = ds.Tables[0];
            DataTable detalle = ds.Tables[1];

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4.Landscape());
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Reporte de Ventas Totales")
                            .FontSize(20).Bold().FontColor(Colors.Brown.Darken3);
                        col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(9).FontColor(Colors.Grey.Darken1);
                        col.Item().PaddingTop(10);
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().PaddingBottom(10).Text("Resumen General").FontSize(14).Bold();

                        if (resumen.Rows.Count > 0)
                        {
                            DataRow r = resumen.Rows[0];
                            col.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(c =>
                                {
                                    c.RelativeColumn();
                                    c.RelativeColumn();
                                });

                                tabla.Cell().Text("Total de ventas:").Bold();
                                tabla.Cell().Text(r["TotalVentas"].ToString());

                                tabla.Cell().Text("Total ingresos:").Bold();
                                tabla.Cell().Text(Convert.ToDecimal(r["TotalIngresos"]).ToString("C2"));

                                tabla.Cell().Text("Promedio por venta:").Bold();
                                tabla.Cell().Text(Convert.ToDecimal(r["PromedioVenta"]).ToString("C2"));

                                tabla.Cell().Text("Total IVA:").Bold();
                                tabla.Cell().Text(Convert.ToDecimal(r["TotalIVA"]).ToString("C2"));

                                tabla.Cell().Text("Total descuentos:").Bold();
                                tabla.Cell().Text(Convert.ToDecimal(r["TotalDescuentos"]).ToString("C2"));
                            });
                        }

                        col.Item().PaddingTop(20).Text("Detalle de Ventas").FontSize(14).Bold();

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(50);
                                c.RelativeColumn();
                                c.RelativeColumn(2);
                                c.RelativeColumn(2);
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                            });

                            tabla.Header(h =>
                            {
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("ID").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Fecha").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Cliente").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Empleado").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Estado").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Pago").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).AlignRight().Text("Total").Bold();
                            });

                            foreach (DataRow f in detalle.Rows)
                            {
                                tabla.Cell().Padding(3).Text(f["IdVenta"].ToString());
                                tabla.Cell().Padding(3).Text(Convert.ToDateTime(f["FechaVenta"]).ToString("dd/MM/yyyy"));
                                tabla.Cell().Padding(3).Text(f["Cliente"].ToString());
                                tabla.Cell().Padding(3).Text(f["Empleado"].ToString());
                                tabla.Cell().Padding(3).Text(f["EstadoVenta"].ToString());
                                tabla.Cell().Padding(3).Text(f["TipoPago"].ToString());
                                tabla.Cell().Padding(3).AlignRight().Text(Convert.ToDecimal(f["TotalVenta"]).ToString("C2"));
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Página ");
                        t.CurrentPageNumber();
                        t.Span(" de ");
                        t.TotalPages();
                    });
                });
            }).GeneratePdf(rutaCompleta);
        }

        private void btnReportePagos_Click(object sender, EventArgs e)
        {
            string nombre = $"Reporte_Pagos_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            GenerarReporteConConfirmacion(nombre, GenerarPdfPagos);
        }

        private void GenerarPdfPagos(string rutaCompleta)
        {
            DataSet ds = ReportesGraficos.ReportePagos();
            DataTable resumen = ds.Tables[0];
            DataTable detalle = ds.Tables[1];

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Reporte de Pagos")
                            .FontSize(20).Bold().FontColor(Colors.Brown.Darken3);
                        col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(9).FontColor(Colors.Grey.Darken1);
                        col.Item().PaddingTop(10);
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().PaddingBottom(10).Text("Resumen por Estado").FontSize(14).Bold();

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(c =>
                            {
                                c.RelativeColumn();
                                c.RelativeColumn();
                                c.RelativeColumn();
                            });

                            tabla.Header(h =>
                            {
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Estado").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).AlignRight().Text("Cantidad").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).AlignRight().Text("Monto").Bold();
                            });

                            foreach (DataRow f in resumen.Rows)
                            {
                                tabla.Cell().Padding(3).Text(f["EstadoPago"].ToString());
                                tabla.Cell().Padding(3).AlignRight().Text(f["Cantidad"].ToString());
                                tabla.Cell().Padding(3).AlignRight().Text(Convert.ToDecimal(f["MontoTotal"]).ToString("C2"));
                            }
                        });

                        col.Item().PaddingTop(20).Text("Detalle de Pagos").FontSize(14).Bold();

                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(40);
                                c.RelativeColumn();
                                c.RelativeColumn(2);
                                c.RelativeColumn(2);
                                c.RelativeColumn();
                                c.RelativeColumn();
                            });

                            tabla.Header(h =>
                            {
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("ID").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Fecha").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Cliente").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Servicio").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Estado").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).AlignRight().Text("Monto").Bold();
                            });

                            foreach (DataRow f in detalle.Rows)
                            {
                                tabla.Cell().Padding(3).Text(f["IdRegistroPago"].ToString());
                                tabla.Cell().Padding(3).Text(Convert.ToDateTime(f["FechaPago"]).ToString("dd/MM/yyyy"));
                                tabla.Cell().Padding(3).Text(f["Cliente"].ToString());
                                tabla.Cell().Padding(3).Text(f["NombreServicio"].ToString());
                                tabla.Cell().Padding(3).Text(f["EstadoPago"].ToString());
                                tabla.Cell().Padding(3).AlignRight().Text(Convert.ToDecimal(f["TotalVenta"]).ToString("C2"));
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Página ");
                        t.CurrentPageNumber();
                        t.Span(" de ");
                        t.TotalPages();
                    });
                });
            }).GeneratePdf(rutaCompleta);
        }

        private void btnReporteTopProductos_Click(object sender, EventArgs e)
        {
            string nombre = $"Reporte_Top15Productos_{DateTime.Now:yyyyMMdd_HHmm}.pdf";
            GenerarReporteConConfirmacion(nombre, GenerarPdfTopProductos);
        }

        private void GenerarPdfTopProductos(string rutaCompleta)
        {
            DataSet ds = ReportesGraficos.ReporteTopProductos();
            DataTable detalle = ds.Tables[0];

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(30);
                    page.DefaultTextStyle(t => t.FontSize(10));

                    page.Header().Column(col =>
                    {
                        col.Item().Text("Top 15 Productos Más Vendidos")
                            .FontSize(20).Bold().FontColor(Colors.Brown.Darken3);
                        col.Item().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                            .FontSize(9).FontColor(Colors.Grey.Darken1);
                        col.Item().PaddingTop(10);
                    });

                    page.Content().Column(col =>
                    {
                        col.Item().Table(tabla =>
                        {
                            tabla.ColumnsDefinition(c =>
                            {
                                c.ConstantColumn(30);
                                c.RelativeColumn(3);
                                c.RelativeColumn(2);
                                c.RelativeColumn(2);
                                c.RelativeColumn();
                                c.RelativeColumn();
                            });

                            tabla.Header(h =>
                            {
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("#").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Producto").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Servicio").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).Text("Proveedor").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).AlignRight().Text("Cantidad").Bold();
                                h.Cell().Background(Colors.Brown.Lighten3).Padding(4).AlignRight().Text("Total").Bold();
                            });

                            int i = 1;
                            foreach (DataRow f in detalle.Rows)
                            {
                                tabla.Cell().Padding(3).Text(i.ToString());
                                tabla.Cell().Padding(3).Text(f["NombreProducto"].ToString());
                                tabla.Cell().Padding(3).Text(f["NombreServicio"].ToString());
                                tabla.Cell().Padding(3).Text(f["NombreProveedor"].ToString());
                                tabla.Cell().Padding(3).AlignRight().Text(f["TotalVendido"].ToString());
                                tabla.Cell().Padding(3).AlignRight().Text(Convert.ToDecimal(f["TotalGenerado"]).ToString("C2"));
                                i++;
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(t =>
                    {
                        t.Span("Página ");
                        t.CurrentPageNumber();
                        t.Span(" de ");
                        t.TotalPages();
                    });
                });
            }).GeneratePdf(rutaCompleta);
        }
    }
}
