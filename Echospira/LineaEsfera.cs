using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProyectoGrafica
{
    public enum TipoLinea
    {
        Radial,    // Desde el centro hacia afuera
        Latitud,   // Círculos horizontales
        Longitud   // Meridianos verticales
    }

    public class LineaEsfera
    {
        public TipoLinea Tipo { get; set; }
        public float Phi { get; set; }      // Ángulo polar (0 a PI)
        public float Phi2 { get; set; }     // Segundo ángulo polar (para longitudes)
        public float Theta { get; set; }    // Ángulo azimutal (0 a 2PI)
        public float Theta2 { get; set; }   // Segundo ángulo azimutal (para latitudes)
        public float LongitudBase { get; set; } = 1.0f;
        public float FactorLongitud { get; set; } = 1.0f;
        public float IntensidadColor { get; set; } = 1.0f;
        public int IndiceColor { get; set; }

        public PointF PuntoInicio { get; private set; }
        public PointF PuntoFin { get; private set; }
        public LineaEsfera() { }

        // ← Este constructor nuevo soluciona el error 2
        public LineaEsfera(PointF inicio, PointF fin)
        {
            PuntoInicio = inicio;
            PuntoFin = fin;
        }

        public void Actualizar(float energia)
        {
            FactorLongitud = 1.0f + energia * 0.5f;
            IntensidadColor = Math.Min(1.0f, energia * 2f);

            float dx = PuntoFin.X - PuntoInicio.X;
            float dy = PuntoFin.Y - PuntoInicio.Y;

            PuntoFin = new PointF(
                PuntoInicio.X + dx * FactorLongitud,
                PuntoInicio.Y + dy * FactorLongitud
            );
        }
    }
}
