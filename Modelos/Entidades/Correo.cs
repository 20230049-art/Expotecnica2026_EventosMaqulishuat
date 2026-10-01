using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Correo
    {
        private const string Remitente = "sophia26crn@gmail.com";
        private const string NombreRemitente = "Alquileres Maquilishuat";
        private const string PasswordApp = "rmijxagjwotltsml";

        public static bool EnviarCodigoRecuperacion(string correoDestino, string codigo)
        {
            try
            {
                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(NombreRemitente, Remitente));
                mensaje.To.Add(MailboxAddress.Parse(correoDestino));
                mensaje.Subject = "Recuperación de contraseña";

                mensaje.Body = new TextPart("plain")
                {
                    Text = $"Hola,\n\n" +
                           $"Recibimos una solicitud para restablecer tu contraseña.\n\n" +
                           $"Tu código de recuperación es: {codigo}\n\n" +
                           $"Este código expira en 15 minutos.\n" +
                           $"Si no solicitaste esto, ignora este mensaje."
                };

                using (var client = new MailKit.Net.Smtp.SmtpClient())
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate(Remitente, PasswordApp);
                    client.Send(mensaje);
                    client.Disconnect(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Error al enviar correo:\n" +
                    "Tipo: " + ex.GetType().Name + "\n" +
                    "Mensaje: " + ex.Message + "\n" +
                    "Inner: " + (ex.InnerException?.Message ?? "N/A"),
                    "ERROR-CORREO",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return false;
            }
        }
    }
}
