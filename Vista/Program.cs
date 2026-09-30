using Modelos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vista.IniciarSesion;
using Vista.PrimerUso;

namespace Vista
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                var (existeUsuarios, existeConfiguracion) = DatosEmpresa.ExisteConfiguracion();

                if (!existeUsuarios)
                {
                    using (var bienvenida = new frmInicioBienvenida())
                    {
                        if (bienvenida.ShowDialog() != DialogResult.OK)
                        {
                            Application.Exit();
                            return;
                        }
                    }

                    using (var config = new frmConfiguracionInicial())
                    {
                        if (config.ShowDialog() != DialogResult.OK)
                        {
                            Application.Exit();
                            return;
                        }
                    }

                    using (var completo = new frmInicioCompleto())
                    {
                        if (completo.ShowDialog() != DialogResult.OK)
                        {
                            Application.Exit();
                            return;
                        }
                    }
                }

                Application.Run(new frmInicio());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al iniciar la aplicación:\n" + ex.Message,
                        "ERROR-INICIO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
