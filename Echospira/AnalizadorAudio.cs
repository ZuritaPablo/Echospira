using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoGrafica
{
    internal class AnalizadorAudio
    {
        private Random rand = new Random();
        private float tiempo = 0f;
        private float volumenSuavizado = 0f;
        private float volumenAnterior = 0f;
        private float[] bandasFrecuencia = new float[12];

        public float VolumenSuavizado => volumenSuavizado;
        public float VolumenAnterior => volumenAnterior;
        public float Tiempo => tiempo;
        public float[] BandasFrecuencia => bandasFrecuencia;

        public void ActualizarAnalisis(float volumenActual)
        {
            tiempo += 0.016f;

            // Suavizado del volumen
            float factorSuavizado = volumenSuavizado < volumenActual ? 0.2f : 0.08f;
            volumenSuavizado += (volumenActual - volumenSuavizado) * factorSuavizado;

            // Actualizar bandas de frecuencia simuladas
            ActualizarBandasFrecuencia();

            volumenAnterior = volumenSuavizado;
        }

        private void ActualizarBandasFrecuencia()
        {
            float intensidad = Math.Min(1.0f, volumenSuavizado * 2.5f);

            for (int i = 0; i < bandasFrecuencia.Length; i++)
            {
                float ruido = (float)(rand.NextDouble() * 0.2 - 0.1);
                float onda = (float)Math.Sin(tiempo * (1.5f + i * 0.3f)) * intensidad;
                float decay = (float)Math.Exp(-tiempo % 1f * 3f) * intensidad;
                bandasFrecuencia[i] = Math.Max(0, Math.Min(1.0f, onda + ruido + decay));
            }
        }

        public float CalcularIntensidadGeneral()
        {
            return Math.Min(1.0f, volumenSuavizado * 2f);
        }

        public float CalcularCambioVolumen()
        {
            return Math.Abs(volumenSuavizado - volumenAnterior);
        }

        public float CalcularPulsacionGeneral()
        {
            return CalcularCambioVolumen() * 8f;
        }
    }
}