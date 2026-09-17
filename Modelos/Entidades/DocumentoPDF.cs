using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using Chunk = iTextSharp.text.Chunk;

namespace Modelos.Entidades
{
    public class DocumentoPDF
    {
        public static string GenerarFacturaPDF(FacturaCompleta factura)
        {
            try
            {
                string carpetaFactura = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    "Facturas");

                if (!Directory.Exists(carpetaFactura))
                {
                    Directory.CreateDirectory(carpetaFactura);
                }

                string nombreArchivo = $"Factura_{factura.NumeroFactura}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                string rutaCompleta = Path.Combine(carpetaFactura, nombreArchivo);

                Document documento = new Document(PageSize.A4, 50, 50, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(documento, new FileStream(rutaCompleta, FileMode.Create));

                documento.Open();

                //Titulo
                Font tituloFont = FontFactory.GetFont("Arial", 22, Font.BOLD, BaseColor.BLACK);
                Paragraph titulo = new Paragraph("FACTURA", tituloFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                documento.Add(titulo);

                documento.Add(new Paragraph("\n"));

                //Datos de la empresa
                Font empresaFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
                Paragraph empresa = new Paragraph("Eventos y Alquileres Maquillishuat", empresaFont);
                empresa.Alignment = Element.ALIGN_CENTER;
                documento.Add(empresa);

                documento.Add(new Paragraph("C. Berlin No.247, San Salvador", FontFactory.GetFont("Arial", 10)));
                documento.Add(new Paragraph("NRC: 4618", FontFactory.GetFont("Arial", 10)));
                documento.Add(new Paragraph("Teléfono: +503 7123-4567", FontFactory.GetFont("Arial", 10)));
                documento.Add(new Paragraph("\n"));

                //Datos de la factura
                PdfPTable tablaInfo = new PdfPTable(2);
                tablaInfo.WidthPercentage = 100;
                tablaInfo.SetWidths(new float[] { 1f, 1f });

                // Columna Izquierda
                PdfPCell ladoIzquierdo = new PdfPCell();
                ladoIzquierdo.Border = Rectangle.NO_BORDER;

                ladoIzquierdo.AddElement(new Paragraph($"N° Factura: {factura.NumeroFactura}", FontFactory.GetFont("Arial", 10)));
                ladoIzquierdo.AddElement(new Paragraph($"N° Control: {factura.NumeroControlFactura}", FontFactory.GetFont("Arial", 10)));
                ladoIzquierdo.AddElement(new Paragraph($"Sello: {factura.SelloRecepcion}", FontFactory.GetFont("Arial", 10)));
                ladoIzquierdo.AddElement(new Paragraph($"Fecha: {factura.FechaFactura:dd/MM/yyyy}", FontFactory.GetFont("Arial", 10)));
                ladoIzquierdo.AddElement(new Paragraph($"Hora: {factura.HoraFactura?.ToString(@"hh\:mm")}", FontFactory.GetFont("Arial", 10)));

                // Columna Derecha
                PdfPCell ladoDerecho = new PdfPCell();
                ladoDerecho.Border = Rectangle.NO_BORDER;
                ladoDerecho.HorizontalAlignment = Element.ALIGN_RIGHT;

                ladoDerecho.AddElement(new Paragraph($"Tipo Documento: {factura.TipoDocumento}", FontFactory.GetFont("Arial", 10)));
                ladoDerecho.AddElement(new Paragraph($"Sucursal: {factura.Sucursal}", FontFactory.GetFont("Arial", 10)));
                ladoDerecho.AddElement(new Paragraph($"Vendedor: {factura.NombreEmpleado}", FontFactory.GetFont("Arial", 10)));
                ladoDerecho.AddElement(new Paragraph($"Tipo Pago: {factura.TipoPago}", FontFactory.GetFont("Arial", 10)));
                ladoDerecho.AddElement(new Paragraph($"Estado: {factura.EstadoVenta}", FontFactory.GetFont("Arial", 10)));

                tablaInfo.AddCell(ladoIzquierdo);
                tablaInfo.AddCell(ladoDerecho);
                documento.Add(tablaInfo);

                documento.Add(new Paragraph("\n"));

                // Línea separadora
                documento.Add(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 1)));

                //Datos del Cliente
                Font clienteFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
                Paragraph clienteTitulo = new Paragraph("DATOS DEL CLIENTE", clienteFont);
                clienteTitulo.Alignment = Element.ALIGN_LEFT;
                documento.Add(clienteTitulo);

                documento.Add(new Paragraph($"Cliente: {factura.NombreCliente} {factura.ApellidoCliente}", FontFactory.GetFont("Arial", 10)));
                documento.Add(new Paragraph($"DUI: {factura.DuiCliente}", FontFactory.GetFont("Arial", 10)));
                documento.Add(new Paragraph($"NIT: {factura.NitCliente}", FontFactory.GetFont("Arial", 10)));
                documento.Add(new Paragraph($"Correo: {factura.CorreoCliente}", FontFactory.GetFont("Arial", 10)));
                //documento.Add(new Paragraph($"Fecha de Uso: {factura.FechaUso:dd/MM/yyyy}", FontFactory.GetFont("Arial", 10)));

                documento.Add(new Paragraph("\n"));

                // Línea separadora
                documento.Add(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 1)));

                documento.Add(new Paragraph("\n"));

                //Productos Vendidos
                PdfPTable tablaProductos = new PdfPTable(5);
                tablaProductos.WidthPercentage = 100;
                tablaProductos.SetWidths(new float[] { 4f, 1.5f, 2f, 2f, 2.5f });

                // Encabezados
                string[] headers = { "Producto", "Cantidad", "Precio Unit.", "Descuento", "Subtotal" };
                foreach (string header in headers)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(header, FontFactory.GetFont("Arial", 10, Font.BOLD)));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tablaProductos.AddCell(cell);
                }

                // Datos de los productos
                foreach (var detalle in factura.Detalles)
                {
                    tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.NombreProducto, FontFactory.GetFont("Arial", 10))));
                    tablaProductos.AddCell(new PdfPCell(new Phrase(detalle.Cantidad.ToString(), FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_CENTER });
                    tablaProductos.AddCell(new PdfPCell(new Phrase($"${detalle.PrecioUnitario:F2}", FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    tablaProductos.AddCell(new PdfPCell(new Phrase($"${detalle.Descuento:F2}", FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });
                    tablaProductos.AddCell(new PdfPCell(new Phrase($"${detalle.TotalDetalleVenta:F2}", FontFactory.GetFont("Arial", 10))) { HorizontalAlignment = Element.ALIGN_RIGHT });
                }

                documento.Add(tablaProductos);

                documento.Add(new Paragraph("\n"));

                //Totales
                PdfPTable tablaTotales = new PdfPTable(2);
                tablaTotales.WidthPercentage = 40;
                tablaTotales.HorizontalAlignment = Element.ALIGN_RIGHT;
                tablaTotales.SetWidths(new float[] { 1f, 1.5f });

                // Subtotal
                PdfPCell cellSubtotalLabel = new PdfPCell(new Phrase("SUBTOTAL:", FontFactory.GetFont("Arial", 11, Font.BOLD)));
                cellSubtotalLabel.Border = Rectangle.NO_BORDER;
                cellSubtotalLabel.HorizontalAlignment = Element.ALIGN_RIGHT;
                tablaTotales.AddCell(cellSubtotalLabel);

                PdfPCell cellSubtotalValue = new PdfPCell(new Phrase($"${factura.SubtotalVenta:F2}", FontFactory.GetFont("Arial", 11)));
                cellSubtotalValue.Border = Rectangle.NO_BORDER;
                cellSubtotalValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                tablaTotales.AddCell(cellSubtotalValue);

                // IVA
                PdfPCell cellIvaLabel = new PdfPCell(new Phrase("IVA (13%):", FontFactory.GetFont("Arial", 11)));
                cellIvaLabel.Border = Rectangle.NO_BORDER;
                cellIvaLabel.HorizontalAlignment = Element.ALIGN_RIGHT;
                tablaTotales.AddCell(cellIvaLabel);

                PdfPCell cellIvaValue = new PdfPCell(new Phrase($"${factura.IVA:F2}", FontFactory.GetFont("Arial", 11)));
                cellIvaValue.Border = Rectangle.NO_BORDER;
                cellIvaValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                tablaTotales.AddCell(cellIvaValue);

                // Descuento (si hay)
                if (factura.DescuentoVenta > 0)
                {
                    PdfPCell cellDescLabel = new PdfPCell(new Phrase("DESCUENTO:", FontFactory.GetFont("Arial", 11)));
                    cellDescLabel.Border = Rectangle.NO_BORDER;
                    cellDescLabel.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tablaTotales.AddCell(cellDescLabel);

                    PdfPCell cellDescValue = new PdfPCell(new Phrase($"-${factura.DescuentoVenta:F2}", FontFactory.GetFont("Arial", 11)));
                    cellDescValue.Border = Rectangle.NO_BORDER;
                    cellDescValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                    tablaTotales.AddCell(cellDescValue);
                }

                // TOTAL
                PdfPCell cellTotalLabel = new PdfPCell(new Phrase("TOTAL:", FontFactory.GetFont("Arial", 14, Font.BOLD)));
                cellTotalLabel.Border = Rectangle.NO_BORDER;
                cellTotalLabel.HorizontalAlignment = Element.ALIGN_RIGHT;
                tablaTotales.AddCell(cellTotalLabel);

                PdfPCell cellTotalValue = new PdfPCell(new Phrase($"${factura.TotalVenta:F2}", FontFactory.GetFont("Arial", 14, Font.BOLD)));
                cellTotalValue.Border = Rectangle.NO_BORDER;
                cellTotalValue.HorizontalAlignment = Element.ALIGN_RIGHT;
                tablaTotales.AddCell(cellTotalValue);

                documento.Add(tablaTotales);

                documento.Add(new Paragraph("\n"));

                // Línea separadora
                documento.Add(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0f, 100f, BaseColor.BLACK, Element.ALIGN_CENTER, 1)));

                //Pie de página
                documento.Add(new Paragraph("\n"));
                documento.Add(new Paragraph("¡Gracias por su compra!", FontFactory.GetFont("Arial", 12, Font.ITALIC)) { Alignment = Element.ALIGN_CENTER });
                documento.Add(new Paragraph($"Factura generada: {DateTime.Now:dd/MM/yyyy HH:mm}", FontFactory.GetFont("Arial", 8)) { Alignment = Element.ALIGN_CENTER });

                documento.Close();
                writer.Close();

                return rutaCompleta;

            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar PDF: {ex.Message}");
            }
        }
    }
}
