using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Document = QuestPDF.Fluent.Document;

namespace Modelos.Entidades
{
    public class DocumentoPDF
    {
        public static string GenerarFacturaPDF(FacturaCompleta factura)
        {
            try
            {
                if (factura == null)
                    throw new ArgumentNullException(nameof(factura));

                // ============================================
                //   CARPETA Y NOMBRE DE ARCHIVO
                // ============================================
                string carpetaFactura = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Facturas");

                if (!Directory.Exists(carpetaFactura))
                    Directory.CreateDirectory(carpetaFactura);

                string nombreArchivo = $"Factura_{factura.NumeroFactura}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string rutaCompleta = Path.Combine(carpetaFactura, nombreArchivo);

                // ============================================
                //   DATOS DE LA EMPRESA (opcional)
                // ============================================
                var empresa = DatosEmpresa.Obtener();

                string nombreEmpresa = empresa?.NombreEmpresa ?? "Eventos y Alquileres Maquillishuat";
                string direccionEmpresa = empresa?.DireccionEmpresa ?? "C. Berlin No.247, San Salvador";
                string correoEmpresa = empresa?.CorreoEmpresa ?? "example@empresa.com";
                string telefonoEmpresa = empresa?.TelefonoEmpresa ?? "+503 7123-4567";
                string logoRuta = empresa?.LogoEmpresa;

                // ============================================
                //   GENERAR PDF
                // ============================================
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(40);
                        page.DefaultTextStyle(t => t.FontFamily("Lato").FontSize(10));

                        // ============ HEADER ============
                        page.Header().Column(col =>
                        {
                            // Título principal
                            col.Item()
                               .AlignCenter()
                               .Text("FACTURA")
                               .FontSize(24)
                               .Bold()
                               .FontColor(Colors.Brown.Darken4);

                            col.Item().PaddingTop(8);

                            // Logo + nombre empresa (si hay logo)
                            if (!string.IsNullOrWhiteSpace(logoRuta))
                            {
                                string rutaLogo = Path.IsPathRooted(logoRuta)
                                    ? logoRuta
                                    : Path.Combine(System.Windows.Forms.Application.StartupPath, logoRuta);

                                if (File.Exists(rutaLogo))
                                {
                                    col.Item()
                                       .AlignCenter()
                                       .Height(60)
                                       .Image(rutaLogo)
                                       .FitArea();
                                }
                            }

                            col.Item()
                               .PaddingTop(6)
                               .AlignCenter()
                               .Text(nombreEmpresa)
                               .FontSize(16)
                               .Bold()
                               .FontColor(Colors.Brown.Darken3);

                            col.Item()
                               .PaddingTop(4)
                               .AlignCenter()
                               .Text(direccionEmpresa)
                               .FontSize(10)
                               .FontColor(Colors.Black);

                            col.Item()
                               .AlignCenter()
                               .Text($"Teléfono: {telefonoEmpresa}")
                               .FontSize(10);

                            col.Item()
                               .AlignCenter()
                               .Text($"Correo: {correoEmpresa}")
                               .FontSize(10);

                            col.Item().PaddingTop(15);

                            // ============ DATOS DE LA FACTURA ============
                            col.Item().Row(row =>
                            {
                                // Columna izquierda
                                row.RelativeItem().Column(izq =>
                                {
                                    izq.Item().Text(t =>
                                    {
                                        t.Span("N° Factura: ").SemiBold();
                                        t.Span(factura.NumeroFactura);
                                    });
                                    izq.Item().Text(t =>
                                    {
                                        t.Span("N° Control: ").SemiBold();
                                        t.Span(factura.NumeroControlFactura?.ToString() ?? "N/A");
                                    });
                                    izq.Item().Text(t =>
                                    {
                                        t.Span("Sello: ").SemiBold();
                                        t.Span(factura.SelloRecepcion ?? "");
                                    });
                                    izq.Item().Text(t =>
                                    {
                                        t.Span("Fecha: ").SemiBold();
                                        t.Span(factura.FechaFactura.ToString("dd/MM/yyyy"));
                                    });
                                    izq.Item().Text(t =>
                                    {
                                        t.Span("Hora: ").SemiBold();
                                        t.Span(factura.HoraFactura?.ToString(@"hh\:mm") ?? "");
                                    });
                                });

                                // Columna derecha
                                row.RelativeItem().Column(der =>
                                {
                                    der.Item().AlignRight().Text(t =>
                                    {
                                        t.Span("Tipo Documento: ").SemiBold();
                                        t.Span(factura.TipoDocumento ?? "");
                                    });
                                    der.Item().AlignRight().Text(t =>
                                    {
                                        t.Span("Sucursal: ").SemiBold();
                                        t.Span(factura.Sucursal ?? "");
                                    });
                                    der.Item().AlignRight().Text(t =>
                                    {
                                        t.Span("Vendedor: ").SemiBold();
                                        t.Span(factura.NombreEmpleado ?? "");
                                    });
                                    der.Item().AlignRight().Text(t =>
                                    {
                                        t.Span("Tipo Pago: ").SemiBold();
                                        t.Span(factura.TipoPago ?? "");
                                    });
                                    der.Item().AlignRight().Text(t =>
                                    {
                                        t.Span("Estado: ").SemiBold();
                                        t.Span(factura.EstadoVenta ?? "");
                                    });
                                });
                            });

                            // Línea separadora
                            col.Item().PaddingTop(15)
                               .LineHorizontal(1)
                               .LineColor(Colors.Brown.Darken3);
                        });

                        // ============ CONTENIDO ============
                        page.Content().PaddingTop(15).Column(col =>
                        {
                            // ============ DATOS DEL CLIENTE ============
                            col.Item()
                               .Text("DATOS DEL CLIENTE")
                               .FontSize(12)
                               .Bold()
                               .FontColor(Colors.Brown.Darken3);

                            col.Item().PaddingTop(6);

                            col.Item().Text(t =>
                            {
                                t.Span("Cliente: ").SemiBold();
                                t.Span($"{factura.NombreCliente} {factura.ApellidoCliente}");
                            });
                            col.Item().Text(t =>
                            {
                                t.Span("DUI: ").SemiBold();
                                t.Span(string.IsNullOrWhiteSpace(factura.DuiCliente) ? "No aplica" : factura.DuiCliente);
                            });
                            col.Item().Text(t =>
                            {
                                t.Span("NIT: ").SemiBold();
                                t.Span(string.IsNullOrWhiteSpace(factura.NitCliente) ? "No aplica" : factura.NitCliente);
                            });
                            col.Item().Text(t =>
                            {
                                t.Span("Correo: ").SemiBold();
                                t.Span(factura.CorreoCliente ?? "");
                            });

                            col.Item().PaddingTop(15);

                            // Línea separadora
                            col.Item().LineHorizontal(1).LineColor(Colors.Brown.Darken3);

                            col.Item().PaddingTop(15);

                            // ============ TABLA DE PRODUCTOS ============
                            col.Item().Table(tabla =>
                            {
                                tabla.ColumnsDefinition(c =>
                                {
                                    c.RelativeColumn(4);    // Producto
                                    c.RelativeColumn(1.2f); // Cantidad
                                    c.RelativeColumn(1.8f); // Precio Unit
                                    c.RelativeColumn(1.6f); // Descuento
                                    c.RelativeColumn(1.8f); // Subtotal
                                });

                                // Encabezados
                                tabla.Header(h =>
                                {
                                    var headerStyle = TextStyle.Default
                                        .SemiBold()
                                        .FontColor(Colors.White)
                                        .FontSize(10);

                                    h.Cell().Background(Colors.Brown.Darken2).Padding(6).Text("Producto").Style(headerStyle);
                                    h.Cell().Background(Colors.Brown.Darken2).Padding(6).AlignCenter().Text("Cantidad").Style(headerStyle);
                                    h.Cell().Background(Colors.Brown.Darken2).Padding(6).AlignCenter().Text("Precio Unit.").Style(headerStyle);
                                    h.Cell().Background(Colors.Brown.Darken2).Padding(6).AlignCenter().Text("Descuento").Style(headerStyle);
                                    h.Cell().Background(Colors.Brown.Darken2).Padding(6).AlignCenter().Text("Subtotal").Style(headerStyle);
                                });

                                // Filas
                                int idx = 0;
                                foreach (var detalle in factura.Detalles)
                                {
                                    var fondo = (idx % 2 == 0) ? Colors.White : Colors.Brown.Lighten5;

                                    tabla.Cell().Background(fondo).Padding(6).Text(detalle.NombreProducto);
                                    tabla.Cell().Background(fondo).Padding(6).AlignCenter().Text(detalle.Cantidad.ToString());
                                    tabla.Cell().Background(fondo).Padding(6).AlignRight().Text($"${detalle.PrecioUnitario:F2}");
                                    tabla.Cell().Background(fondo).Padding(6).AlignRight().Text($"${detalle.Descuento:F2}");
                                    tabla.Cell().Background(fondo).Padding(6).AlignRight().Text($"${detalle.TotalDetalleVenta:F2}");

                                    idx++;
                                }
                            });

                            col.Item().PaddingTop(20);

                            // ============ TOTALES ============
                            col.Item().AlignRight().Width(250).Column(tot =>
                            {
                                tot.Item().Row(r =>
                                {
                                    r.RelativeItem().AlignRight().Text("SUBTOTAL:").SemiBold().FontSize(11);
                                    r.ConstantItem(90).AlignRight().Text($"${factura.SubtotalVenta:F2}").FontSize(11);
                                });

                                tot.Item().Row(r =>
                                {
                                    r.RelativeItem().AlignRight().Text("IVA (13%):").FontSize(11);
                                    r.ConstantItem(90).AlignRight().Text($"${factura.IVA:F2}").FontSize(11);
                                });

                                if (factura.DescuentoVenta > 0)
                                {
                                    tot.Item().Row(r =>
                                    {
                                        r.RelativeItem().AlignRight().Text("DESCUENTO:").FontSize(11);
                                        r.ConstantItem(90).AlignRight().Text($"-${factura.DescuentoVenta:F2}").FontSize(11);
                                    });
                                }

                                tot.Item().PaddingTop(6).LineHorizontal(1).LineColor(Colors.Brown.Darken3);

                                tot.Item().PaddingTop(4).Row(r =>
                                {
                                    r.RelativeItem().AlignRight().Text("TOTAL:").Bold().FontSize(14).FontColor(Colors.Brown.Darken4);
                                    r.ConstantItem(90).AlignRight().Text($"${factura.TotalVenta:F2}").Bold().FontSize(14).FontColor(Colors.Brown.Darken4);
                                });
                            });
                        });

                        // ============ FOOTER ============
                        page.Footer().Column(col =>
                        {
                            col.Item().PaddingTop(20).LineHorizontal(1).LineColor(Colors.Brown.Darken3);

                            col.Item().PaddingTop(10)
                               .AlignCenter()
                               .Text("¡Gracias por su compra!")
                               .FontSize(12)
                               .Italic()
                               .FontColor(Colors.Brown.Darken3);

                            col.Item().PaddingTop(4)
                               .AlignCenter()
                               .Text($"Factura generada: {DateTime.Now:dd/MM/yyyy HH:mm}")
                               .FontSize(8)
                               .FontColor(Colors.Grey.Darken1);
                        });
                    });
                }).GeneratePdf(rutaCompleta);

                return rutaCompleta;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar PDF: {ex.Message}", ex);
            }
        }
    }
}
