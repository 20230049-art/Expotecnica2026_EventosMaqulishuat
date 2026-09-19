using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.Utilidades
{
    public class ControlesBloqueo
    {
        //Bloquear Controles para que no se pueda copiar, pegar o cortar texto en los controles de tipo TextBox
        public void BloquearControlesTXT(System.Windows.Forms.TextBox txt)
        {
            txt.ContextMenuStrip = new ContextMenuStrip();
            txt.KeyDown += (s, e) =>
            {
                if (e.Control &&
                 (e.KeyCode == Keys.C ||
                  e.KeyCode == Keys.V ||
                  e.KeyCode == Keys.X))
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;

                    MessageBox.Show("Esta acción no está permitida.", "ERROR-CCP-006",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
        }

        //Bloquear Controles para que no se pueda copiar, pegar o cortar texto en los controles de tipo MaskedTextBox
        public void BloquearControlesMTXT(MaskedTextBox mtxt)
        {
            mtxt.ContextMenuStrip = new ContextMenuStrip();
            mtxt.KeyDown += (s, e) =>
            {
                if (e.Control &&
                 (e.KeyCode == Keys.C ||
                  e.KeyCode == Keys.V ||
                  e.KeyCode == Keys.X))
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;

                    MessageBox.Show("Esta acción no está permitida.", "ERROR-CCP-006",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
        }

        //Bloquear Controles para que no se pueda copiar, pegar o cortar texto en los controles de tipo NumericUpDown
        public void BloquearControlesNUD(NumericUpDown nud)
        {
            nud.ContextMenuStrip = new ContextMenuStrip();
            nud.KeyDown += (s, e) =>
            {
                if (e.Control &&
                 (e.KeyCode == Keys.C ||
                  e.KeyCode == Keys.V ||
                  e.KeyCode == Keys.X))
                {
                    e.SuppressKeyPress = true;
                    e.Handled = true;

                    MessageBox.Show("Esta acción no está permitida.", "ERROR-CCP-006",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
        }

        //Validar que los controles que resiven texto solo resivan letras y espacios
        public void ValidarSoloLetras(KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) &&
                !char.IsWhiteSpace(e.KeyChar) &&
                e.KeyChar != '.' &&
                e.KeyChar != ',' &&
                e.KeyChar != ';' &&
                e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;

                MessageBox.Show("Solo se permiten letras, espacios, puntos, comas y punto y coma.", "ERROR-CARACTER-004",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        //Validar que los controles de numeros solo resivan numeros y el punto decimal
        public void ValidarSoloNumeros(System.Windows.Forms.TextBox txt, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != '.' &&
                e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;

                MessageBox.Show("Solo se permiten números y un punto decimal.", "ERROR-CARACTERNO-005",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (char.IsDigit(e.KeyChar) && txt.Text.Contains("."))
            {
                int posicionPunto = txt.Text.IndexOf('.');
                int cantidadDecimales = txt.Text.Length - posicionPunto - 1;

                if (cantidadDecimales >= 2)
                {
                    e.Handled = true;

                    MessageBox.Show("El número solo puede tener un máximo de 2 decimales.", "Cantidad de decimales",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return;
                }
            }

            if (e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;

                MessageBox.Show("El número solo puede contener un punto decimal.", "Punto decimal",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
        }

        //Bloqueo de longitud de controles
        public static void LimitarTextBox(TextBox textBox, int maxCaracteres)
        {
            textBox.MaxLength = maxCaracteres;

            textBox.KeyPress += (s, e) =>
            {
                if (textBox.Text.Length >= maxCaracteres &&
                    !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;

                    MessageBox.Show(
                        $"Este campo permite un máximo de {maxCaracteres} caracteres.", "ERROR-LONGITUD-003",
                        MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
            };
        }

        public static void AplicarMascara(MaskedTextBox maskedTextBox, string mascara)
        {
            maskedTextBox.Mask = mascara;
        }

        public static void LimitarComboBox(ComboBox comboBox, int maxCaracteres)
        {
            comboBox.DropDownStyle = ComboBoxStyle.DropDown;

            comboBox.KeyPress += (s, e) =>
            {
                if (comboBox.Text.Length >= maxCaracteres &&
                    !char.IsControl(e.KeyChar))
                {
                    e.Handled = true;

                    MessageBox.Show(
                        $"Este campo permite un máximo de {maxCaracteres} caracteres.", "ERROR-LONGITUD-003",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };
        }
    }
}
