using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.Utilidades
{
    public class ImagenVista
    {
        public static Image CargarImagen(string rutaBD)
        {

            //Caragr una imagen desde la db o tuta, sino null
            try
            {
                if (string.IsNullOrWhiteSpace(rutaBD))
                    return null;

                string rutaCompleta = Path.IsPathRooted(rutaBD) ? rutaBD : Path.Combine(Application.StartupPath, rutaBD);

                if (!File.Exists(rutaCompleta))
                    return null;


                byte[] bytes = File.ReadAllBytes(rutaCompleta);
                using (var ms = new MemoryStream(bytes))
                {
                    return Image.FromStream(ms);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error al cargar imagen: " + ex.Message);
                return null;
            }
        }

        public static void AsignarImagen(PictureBox pb, string rutaBD, bool stretch = true)
        {
            if (pb == null) return;

            Image img = CargarImagen(rutaBD);

            if (img != null)
            {
                pb.Image = img;
                pb.SizeMode = stretch
                    ? PictureBoxSizeMode.StretchImage
                    : PictureBoxSizeMode.Zoom;
                pb.BackColor = Color.White;
            }
            else
            {
                pb.Image = null;
                pb.BackColor = Color.White; 
            }
        }
    }
}
