using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProyectoGrafica.Graficos
{
    public class GeometriaEsfera
    {
        public List<LineaEsfera> Lineas { get; private set; }
        private int radio;
        private Point centro;
        private Random rnd = new Random();

        public GeometriaEsfera(int radio, Point centro)
        {
            this.radio = radio;
            this.centro = centro;
            GenerarLineas();
        }

        private void GenerarLineas()
        {
            Lineas = new List<LineaEsfera>();
            for (int i = 0; i < 100; i++)
            {
                double angulo1 = rnd.NextDouble() * 2 * Math.PI;
                double angulo2 = rnd.NextDouble() * 2 * Math.PI;

                PointF p1 = new PointF(
                    centro.X + (float)(radio * Math.Sin(angulo1)),
                    centro.Y + (float)(radio * Math.Cos(angulo1))
                );
                PointF p2 = new PointF(
                    centro.X + (float)(radio * Math.Sin(angulo2)),
                    centro.Y + (float)(radio * Math.Cos(angulo2))
                );

                Lineas.Add(new LineaEsfera(p1, p2));
            }
        }

        public void Actualizar(float energia)
        {
            foreach (var linea in Lineas)
            {
                linea.Actualizar(energia);
            }
        }
    }
}
