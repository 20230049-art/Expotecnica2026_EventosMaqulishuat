using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vista.Utilidades
{
    public static class Responsive
    {
        #region Clases internas

        private class ControlLayoutInfo
        {
            public Control Control { get; set; }
            public Rectangle BoundsOriginales { get; set; }
            public float TamanoFuenteOriginal { get; set; }
            public FontStyle EstiloFuenteOriginal { get; set; }
            public string NombreFamiliaFuente { get; set; }
            public GraphicsUnit UnidadFuente { get; set; }
            public bool EsControlLayout { get; set; } // TableLayout o FlowLayout
            public Size TamanoParentOriginal { get; set; }
        }

        private class FormResponsiveInfo
        {
            public Size TamanoOriginal { get; set; }
            public List<ControlLayoutInfo> Controles { get; set; }
            public Timer TimerResize { get; set; }
            public bool Actualizando { get; set; }
            public int ContadorResizes { get; set; }
        }

        #endregion

        #region Variables estáticas

        private static readonly Dictionary<Form, FormResponsiveInfo> _formularios =
            new Dictionary<Form, FormResponsiveInfo>();

        // Configuración global
        private static ResponsiveConfig _config = new ResponsiveConfig();

        #endregion

        #region Configuración

        /// <summary>
        /// Configuración global del responsive
        /// </summary>
        public class ResponsiveConfig
        {
            public bool MantenerProporciones { get; set; } = true;
            public bool ExcluirLayouts { get; set; } = true;
            public int TimerInterval { get; set; } = 150;
            public float FuenteMinima { get; set; } = 7f;
            public float FuenteMaxima { get; set; } = 40f;
            public float EscalaMinima { get; set; } = 0.5f;
            public float EscalaMaxima { get; set; } = 2.0f;
            public bool ForzarAnchor { get; set; } = true;
            public bool AplicarAutoScroll { get; set; } = true;
            public bool AplicarSizeMinimo { get; set; } = true;
            public Size SizeMinimo { get; set; } = new Size(800, 500);
            public bool RedimensionarControlesAnidados { get; set; } = true;
        }

        /// <summary>
        /// Configura el comportamiento global del responsive
        /// </summary>
        public static void SetConfig(ResponsiveConfig config)
        {
            _config = config ?? new ResponsiveConfig();
        }

        #endregion

        #region Métodos públicos

        /// <summary>
        /// Aplica responsive a un formulario y todos sus controles
        /// </summary>
        public static void Apply(Form formulario)
        {
            if (formulario == null) return;

            // Evitar aplicar dos veces
            if (_formularios.ContainsKey(formulario))
                return;

            // Configuración inicial del formulario
            ConfigurarFormulario(formulario);

            // Guardar estado inicial
            var infoFormulario = new FormResponsiveInfo
            {
                TamanoOriginal = formulario.ClientSize,
                Controles = new List<ControlLayoutInfo>(),
                TimerResize = new Timer { Interval = _config.TimerInterval }
            };

            // Capturar todos los controles
            CapturarControles(formulario, infoFormulario.Controles, formulario.ClientSize);

            // Configurar Timer
            infoFormulario.TimerResize.Tick += (sender, e) =>
            {
                infoFormulario.TimerResize.Stop();
                if (!formulario.IsDisposed)
                {
                    AjustarFormulario(formulario, infoFormulario);
                }
            };

            _formularios[formulario] = infoFormulario;

            // Eventos del formulario
            formulario.Resize += (sender, e) =>
            {
                if (_formularios.TryGetValue(formulario, out var info))
                {
                    info.TimerResize.Stop();
                    info.TimerResize.Start();
                }
            };

            formulario.FormClosed += (sender, e) =>
            {
                if (_formularios.TryGetValue(formulario, out var info))
                {
                    info.TimerResize?.Stop();
                    info.TimerResize?.Dispose();
                    _formularios.Remove(formulario);
                }
            };

            // Aplicar ajuste inicial (por si el formulario ya está redimensionado)
            AjustarFormulario(formulario, infoFormulario);
        }

        /// <summary>
        /// Aplica responsive a todos los formularios abiertos
        /// </summary>
        public static void ApplyToAllOpenForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                if (!_formularios.ContainsKey(form))
                {
                    Apply(form);
                }
            }
        }

        /// <summary>
        /// Remueve el responsive de un formulario
        /// </summary>
        public static void Remove(Form formulario)
        {
            if (_formularios.TryGetValue(formulario, out var info))
            {
                info.TimerResize?.Stop();
                info.TimerResize?.Dispose();
                _formularios.Remove(formulario);
            }
        }

        /// <summary>
        /// Fuerza un reajuste inmediato de un formulario
        /// </summary>
        public static void ForceResize(Form formulario)
        {
            if (_formularios.TryGetValue(formulario, out var info))
            {
                AjustarFormulario(formulario, info);
            }
        }

        #endregion

        #region Métodos privados

        private static void ConfigurarFormulario(Form form)
        {
            if (_config.AplicarAutoScroll)
            {
                form.AutoScroll = true;
            }

            if (_config.AplicarSizeMinimo)
            {
                form.MinimumSize = new Size(
                    Math.Max(form.MinimumSize.Width, _config.SizeMinimo.Width),
                    Math.Max(form.MinimumSize.Height, _config.SizeMinimo.Height)
                );
            }
        }

        private static void CapturarControles(
            Control padre,
            List<ControlLayoutInfo> lista,
            Size tamanoParentOriginal)
        {
            foreach (Control control in padre.Controls)
            {
                // Determinar si es un control de layout
                bool esLayout = control is TableLayoutPanel ||
                               control is FlowLayoutPanel ||
                               control is SplitContainer;

                var info = new ControlLayoutInfo
                {
                    Control = control,
                    BoundsOriginales = control.Bounds,
                    TamanoFuenteOriginal = control.Font.Size,
                    EstiloFuenteOriginal = control.Font.Style,
                    NombreFamiliaFuente = control.Font.FontFamily.Name,
                    UnidadFuente = control.Font.Unit,
                    EsControlLayout = esLayout,
                    TamanoParentOriginal = tamanoParentOriginal
                };

                lista.Add(info);

                // Recursivamente capturar controles hijos, si está configurado
                if (_config.RedimensionarControlesAnidados && control.HasChildren)
                {
                    CapturarControles(control, lista, control.ClientSize);
                }
            }
        }

        private static void AjustarFormulario(Form formulario, FormResponsiveInfo info)
        {
            if (info.Actualizando || formulario.IsDisposed)
                return;

            info.Actualizando = true;

            try
            {
                int anchoOriginal = info.TamanoOriginal.Width;
                int altoOriginal = info.TamanoOriginal.Height;

                if (anchoOriginal <= 0 || altoOriginal <= 0)
                    return;

                // Calcular escalas
                float escalaX = (float)formulario.ClientSize.Width / anchoOriginal;
                float escalaY = (float)formulario.ClientSize.Height / altoOriginal;

                // Aplicar límites a las escalas
                escalaX = Math.Max(_config.EscalaMinima, Math.Min(_config.EscalaMaxima, escalaX));
                escalaY = Math.Max(_config.EscalaMinima, Math.Min(_config.EscalaMaxima, escalaY));

                // Escala final
                float escala = _config.MantenerProporciones
                    ? Math.Min(escalaX, escalaY)
                    : Math.Min(escalaX, escalaY); // Usamos la menor para evitar overflow

                // Ajustar cada control
                foreach (var controlInfo in info.Controles)
                {
                    try
                    {
                        AjustarControl(controlInfo, escala, escalaX, escalaY);
                    }
                    catch
                    {
                        // Si un control falla, continuamos con el siguiente
                        continue;
                    }
                }
            }
            finally
            {
                info.Actualizando = false;
            }
        }

        private static void AjustarControl(ControlLayoutInfo info, float escala, float escalaX, float escalaY)
        {
            Control control = info.Control;

            if (control == null || control.IsDisposed || control.Parent == null)
                return;

            // Excluir controles de layout si está configurado
            if (_config.ExcluirLayouts && info.EsControlLayout)
                return;

            // Excluir controles que ya tienen Dock
            if (control.Dock != DockStyle.None)
                return;

            Rectangle original = info.BoundsOriginales;

            // Calcular nuevas posiciones y tamaños
            float escalaUsar = _config.MantenerProporciones ? escala : Math.Min(escalaX, escalaY);

            int x = (int)(original.X * escalaUsar);
            int y = (int)(original.Y * escalaUsar);
            int ancho = Math.Max(1, (int)(original.Width * escalaUsar));
            int alto = Math.Max(1, (int)(original.Height * escalaUsar));

            // Forzar Anchor si está configurado
            if (_config.ForzarAnchor && control.Anchor == AnchorStyles.None)
            {
                control.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }

            // Aplicar bounds
            control.SetBounds(x, y, ancho, alto);

            // Ajustar fuente
            AjustarFuente(control, info, escalaUsar);

            // Si el control es un panel con hijos y está configurado,
            // ajustar sus hijos también
            if (control.HasChildren && !info.EsControlLayout)
            {
                // Los hijos ya se ajustan en el loop principal
                // porque fueron capturados recursivamente
            }
        }

        private static void AjustarFuente(Control control, ControlLayoutInfo info, float escala)
        {
            float nuevoTamano = info.TamanoFuenteOriginal * escala;

            // Aplicar límites
            nuevoTamano = Math.Max(_config.FuenteMinima, Math.Min(_config.FuenteMaxima, nuevoTamano));

            // Solo cambiar si hay diferencia significativa (evita parpadeo)
            if (Math.Abs(control.Font.Size - nuevoTamano) > 0.1f)
            {
                try
                {
                    control.Font = new Font(
                        info.NombreFamiliaFuente,
                        nuevoTamano,
                        info.EstiloFuenteOriginal,
                        info.UnidadFuente
                    );
                }
                catch
                {
                    // Si falla, intentar con la fuente actual
                    control.Font = new Font(
                        control.Font.FontFamily,
                        nuevoTamano,
                        control.Font.Style
                    );
                }
            }
        }

        #endregion

        #region Métodos de extensión (opcionales)

        /// <summary>
        /// Extensión para aplicar responsive directamente desde el formulario
        /// </summary>
        public static void ApplyResponsive(this Form form)
        {
            Apply(form);
        }

        /// <summary>
        /// Extensión para forzar reajuste
        /// </summary>
        public static void ForceResponsiveResize(this Form form)
        {
            ForceResize(form);
        }

        #endregion

        #region Métodos de utilidad

        /// <summary>
        /// Obtiene el estado de responsive de un formulario
        /// </summary>
        public static bool IsResponsive(Form form)
        {
            return _formularios.ContainsKey(form);
        }

        /// <summary>
        /// Reinicia el responsive de un formulario (recaptura controles)
        /// </summary>
        public static void Refresh(Form form)
        {
            if (_formularios.TryGetValue(form, out var info))
            {
                // Recapturar controles
                info.Controles.Clear();
                CapturarControles(form, info.Controles, form.ClientSize);
                info.TamanoOriginal = form.ClientSize;

                // Reajustar
                AjustarFormulario(form, info);
            }
        }

        /// <summary>
        /// Limpia todos los estados de responsive
        /// </summary>
        public static void ClearAll()
        {
            foreach (var kvp in _formularios)
            {
                kvp.Value.TimerResize?.Stop();
                kvp.Value.TimerResize?.Dispose();
            }
            _formularios.Clear();
        }

        #endregion
    }
}
