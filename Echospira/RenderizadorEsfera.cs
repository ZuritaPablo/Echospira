using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoGrafica.Graficos;

namespace ProyectoGrafica
{
    internal class RenderizadorEsfera
    {
        private Random rand = new Random();
        private List<LineaEsfera> lineasEsfera;
        private PictureBox pictureCanvas;
        private PointF centro;
        private float radioBase;
        private float rotacionX = 0f;
        private float rotacionY = 0f;
        private float rotacionZ = 0f;

        public RenderizadorEsfera(PictureBox canvas)
        {
            pictureCanvas = canvas;
            InicializarEsferaLineas();
        }

        private void InicializarEsferaLineas()
        {
            lineasEsfera = new List<LineaEsfera>();

            // Crear líneas radiales (desde el centro hacia afuera)
            int numLineasRadiales = 120;
            for (int i = 0; i < numLineasRadiales; i++)
            {
                float phi = (float)(Math.PI * i / numLineasRadiales); // 0 a PI
                float theta = (float)(2 * Math.PI * i * 0.618033988749895); // Golden ratio

                lineasEsfera.Add(new LineaEsfera
                {
                    Tipo = TipoLinea.Radial,
                    Phi = phi,
                    Theta = theta,
                    LongitudBase = 1.0f,
                    IndiceColor = i
                });
            }

            // Crear líneas de latitud (círculos horizontales)
            int numLatitudes = 15;
            int puntosPerLatitud = 24;
            for (int lat = 0; lat < numLatitudes; lat++)
            {
                float phi = (float)(Math.PI * lat / (numLatitudes - 1)); // 0 a PI
                float radioLatitud = (float)Math.Sin(phi);

                if (radioLatitud > 0.1f) // Evitar líneas muy pequeñas en los polos
                {
                    for (int punto = 0; punto < puntosPerLatitud; punto++)
                    {
                        float theta1 = (float)(2 * Math.PI * punto / puntosPerLatitud);
                        float theta2 = (float)(2 * Math.PI * (punto + 1) / puntosPerLatitud);

                        lineasEsfera.Add(new LineaEsfera
                        {
                            Tipo = TipoLinea.Latitud,
                            Phi = phi,
                            Theta = theta1,
                            Theta2 = theta2,
                            LongitudBase = radioLatitud,
                            IndiceColor = lat * 10 + punto
                        });
                    }
                }
            }

            // Crear líneas de longitud (meridianos)
            int numLongitudes = 18;
            int puntosPerLongitud = 20;
            for (int lon = 0; lon < numLongitudes; lon++)
            {
                float theta = (float)(2 * Math.PI * lon / numLongitudes);

                for (int punto = 0; punto < puntosPerLongitud - 1; punto++)
                {
                    float phi1 = (float)(Math.PI * punto / (puntosPerLongitud - 1));
                    float phi2 = (float)(Math.PI * (punto + 1) / (puntosPerLongitud - 1));

                    lineasEsfera.Add(new LineaEsfera
                    {
                        Tipo = TipoLinea.Longitud,
                        Phi = phi1,
                        Phi2 = phi2,
                        Theta = theta,
                        LongitudBase = 1.0f,
                        IndiceColor = lon * 15 + punto
                    });
                }
            }
        }

        public void ActualizarAnimacion(AnalizadorAudio analizador)
        {
            float volumenSuavizado = analizador.VolumenSuavizado;

            // Rotación automática suave
            rotacionX += 0.01f + volumenSuavizado * 0.2f;
            rotacionY += 0.015f + volumenSuavizado * 0.01f;
            rotacionZ += 0.008f + volumenSuavizado * 0.03f;

            // Actualizar efectos musicales en las líneas
            ActualizarLineasConMusica(analizador);
        }

        private void ActualizarLineasConMusica(AnalizadorAudio analizador)
        {
            float intensidadGeneral = analizador.CalcularIntensidadGeneral();
            float pulsacionGeneral = analizador.CalcularPulsacionGeneral();
            float tiempo = analizador.Tiempo;
            float[] bandasFrecuencia = analizador.BandasFrecuencia;

            foreach (var linea in lineasEsfera)
            {
                // Efectos basados en posición
                float efectoLatitud = (float)Math.Sin(tiempo * 2f + linea.Phi * 3f) * intensidadGeneral * 0.7f;
                float efectoLongitud = (float)Math.Cos(tiempo * 1.5f + linea.Theta * 2f) * intensidadGeneral * 0.3f;

                // Efecto de banda de frecuencia basado en ángulo
                int banda = (int)((linea.Theta / (2 * Math.PI)) * bandasFrecuencia.Length) % bandasFrecuencia.Length;
                float efectoBanda = bandasFrecuencia[banda] * 0.4f;

                // Efecto de respiración global
                float respiracion = (float)Math.Sin(tiempo * 1.2f) * intensidadGeneral * 0.2f;

                // Pulsación súbita
                float pulsacion = pulsacionGeneral * (float)Math.Exp(-((tiempo * 60f) % 60f) * 0.5f);

                // Combinar efectos
                linea.FactorLongitud = linea.LongitudBase + efectoLatitud + efectoLongitud +
                                      efectoBanda + respiracion + pulsacion;

                // Limitar distorsión
                linea.FactorLongitud = Math.Max(0.1f, Math.Min(1.8f, linea.FactorLongitud));

                // Color intensity
                linea.IntensidadColor = Math.Max(0.3f, Math.Min(1.0f,
                    0.7f + intensidadGeneral * 0.8f + efectoBanda * 0.5f));
            }
        }

        public void Renderizar(Graphics g, AnalizadorAudio analizador)
        {
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.Clear(Color.Black);

            centro = new PointF(pictureCanvas.Width / 2f, pictureCanvas.Height / 2f);
            radioBase = Math.Min(pictureCanvas.Width, pictureCanvas.Height) * 0.3f;

            // Separar líneas por profundidad para renderizado correcto
            var lineasOrdenadas = new List<(LineaEsfera linea, float profundidad)>();

            foreach (var linea in lineasEsfera)
            {
                var puntos = CalcularPuntosLinea(linea);
                if (puntos != null)
                {
                    float profundidadPromedio = (puntos.Value.punto1.Z + puntos.Value.punto2.Z) / 2f;
                    lineasOrdenadas.Add((linea, profundidadPromedio));
                }
            }

            // Ordenar por profundidad (más lejano primero)
            lineasOrdenadas.Sort((a, b) => a.profundidad.CompareTo(b.profundidad));

            // Dibujar líneas
            foreach (var (linea, _) in lineasOrdenadas)
            {
                DibujarLinea(g, linea, analizador);
            }

            // Agregar líneas de conexión dinámicas cuando hay mucha música
            if (analizador.VolumenSuavizado > 0.3f)
            {
                DibujarConexionesDinamicas(g, analizador);
            }

            // Dibujar anillo central sutil
            using (var penAnillo = new Pen(Color.FromArgb(60, Color.Cyan), 1f))
            {
                float radioAnillo = radioBase * 0.3f;
                g.DrawEllipse(penAnillo, centro.X - radioAnillo, centro.Y - radioAnillo,
                             radioAnillo * 2, radioAnillo * 2);
            }
        }

        private (Punto3D punto1, Punto3D punto2)? CalcularPuntosLinea(LineaEsfera linea)
        {
            Punto3D punto1, punto2;

            switch (linea.Tipo)
            {
                case TipoLinea.Radial:
                    punto1 = new Punto3D { X = 0, Y = 0, Z = 0 };
                    punto2 = EsferaACartesiano(linea.Phi, linea.Theta, linea.FactorLongitud);
                    break;

                case TipoLinea.Latitud:
                    punto1 = EsferaACartesiano(linea.Phi, linea.Theta, linea.FactorLongitud);
                    punto2 = EsferaACartesiano(linea.Phi, linea.Theta2, linea.FactorLongitud);
                    break;

                case TipoLinea.Longitud:
                    punto1 = EsferaACartesiano(linea.Phi, linea.Theta, linea.FactorLongitud);
                    punto2 = EsferaACartesiano(linea.Phi2, linea.Theta, linea.FactorLongitud);
                    break;

                default:
                    return null;
            }

            return (punto1, punto2);
        }

        private Punto3D EsferaACartesiano(float phi, float theta, float radio)
        {
            return new Punto3D
            {
                X = radio * (float)(Math.Sin(phi) * Math.Cos(theta)),
                Y = radio * (float)(Math.Cos(phi)),
                Z = radio * (float)(Math.Sin(phi) * Math.Sin(theta))
            };
        }

        private void DibujarLinea(Graphics g, LineaEsfera linea, AnalizadorAudio analizador)
        {
            var puntos = CalcularPuntosLinea(linea);
            if (puntos == null) return;

            var p1_2d = ProyectarPunto3D(puntos.Value.punto1);
            var p2_2d = ProyectarPunto3D(puntos.Value.punto2);

            // Solo dibujar si ambos puntos están visibles
            if (p1_2d.Visible && p2_2d.Visible)
            {
                // Color basado en tipo de línea y efectos musicales
                float hue = (linea.IndiceColor * 7f + analizador.Tiempo * 40f) % 360f;
                Color color = ColorFromHSV(hue, 0.8f, linea.IntensidadColor);

                // Alpha basado en profundidad promedio
                float alphaPromedio = (p1_2d.Alpha + p2_2d.Alpha) / 2f;
                color = Color.FromArgb((int)(alphaPromedio * 255), color);

                // Grosor basado en tipo e intensidad
                float grosor = linea.Tipo switch
                {
                    TipoLinea.Radial => 0.8f + analizador.VolumenSuavizado * 1.2f,
                    TipoLinea.Latitud => 0.6f + analizador.VolumenSuavizado * 0.8f,
                    TipoLinea.Longitud => 0.5f + analizador.VolumenSuavizado * 0.7f,
                    _ => 0.5f
                };

                using (var pen = new Pen(color, Math.Max(0.3f, grosor)))
                {
                    g.DrawLine(pen, p1_2d.X, p1_2d.Y, p2_2d.X, p2_2d.Y);
                }
            }
        }

        private void DibujarConexionesDinamicas(Graphics g, AnalizadorAudio analizador)
        {
            int numConexiones = (int)(analizador.VolumenSuavizado * 50);

            for (int i = 0; i < numConexiones; i++)
            {
                // Puntos aleatorios en la superficie de la esfera
                float phi1 = (float)(rand.NextDouble() * Math.PI);
                float theta1 = (float)(rand.NextDouble() * 2 * Math.PI);
                float phi2 = (float)(rand.NextDouble() * Math.PI);
                float theta2 = (float)(rand.NextDouble() * 2 * Math.PI);

                var p1_3d = EsferaACartesiano(phi1, theta1, 1.0f + analizador.VolumenSuavizado * 0.3f);
                var p2_3d = EsferaACartesiano(phi2, theta2, 1.0f + analizador.VolumenSuavizado * 0.3f);

                var p1_2d = ProyectarPunto3D(p1_3d);
                var p2_2d = ProyectarPunto3D(p2_3d);

                if (p1_2d.Visible && p2_2d.Visible)
                {
                    float distancia = (float)Math.Sqrt(Math.Pow(p1_2d.X - p2_2d.X, 2) + Math.Pow(p1_2d.Y - p2_2d.Y, 2));

                    if (distancia < 100f) // Solo líneas cortas
                    {
                        float alpha = (1 - distancia / 100f) * analizador.VolumenSuavizado * 0.4f;
                        Color colorConexion = Color.FromArgb((int)(alpha * 120), Color.White);

                        using (var pen = new Pen(colorConexion, 0.5f))
                        {
                            g.DrawLine(pen, p1_2d.X, p1_2d.Y, p2_2d.X, p2_2d.Y);
                        }
                    }
                }
            }
        }

        private PuntoProyectado ProyectarPunto3D(Punto3D punto)
        {
            float x = punto.X;
            float y = punto.Y;
            float z = punto.Z;

            // Aplicar rotaciones
            // Rotación X
            float tempY = y;
            y = y * (float)Math.Cos(rotacionX) - z * (float)Math.Sin(rotacionX);
            z = tempY * (float)Math.Sin(rotacionX) + z * (float)Math.Cos(rotacionX);

            // Rotación Y
            float tempX = x;
            x = x * (float)Math.Cos(rotacionY) + z * (float)Math.Sin(rotacionY);
            z = -tempX * (float)Math.Sin(rotacionY) + z * (float)Math.Cos(rotacionY);

            // Rotación Z
            tempX = x;
            x = x * (float)Math.Cos(rotacionZ) - y * (float)Math.Sin(rotacionZ);
            y = tempX * (float)Math.Sin(rotacionZ) + y * (float)Math.Cos(rotacionZ);

            // Proyección perspectiva
            float distanciaCamera = 4f;
            float factor = distanciaCamera / (distanciaCamera + z);

            return new PuntoProyectado
            {
                X = centro.X + x * radioBase * factor,
                Y = centro.Y + y * radioBase * factor,
                Z = z,
                Alpha = Math.Max(0.2f, Math.Min(1.0f, factor * 0.9f)),
                Visible = z > -3f && factor > 0.1f
            };
        }

        private Color ColorFromHSV(double hue, double saturation, double value)
        {
            hue = hue % 360;
            if (hue < 0) hue += 360;
            saturation = Math.Max(0, Math.Min(1, saturation));
            value = Math.Max(0, Math.Min(1, value));

            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Math.Max(0, Math.Min(255, Convert.ToInt32(value)));
            int p = Math.Max(0, Math.Min(255, Convert.ToInt32(value * (1 - saturation))));
            int q = Math.Max(0, Math.Min(255, Convert.ToInt32(value * (1 - f * saturation))));
            int t = Math.Max(0, Math.Min(255, Convert.ToInt32(value * (1 - (1 - f) * saturation))));

            return hi switch
            {
                0 => Color.FromArgb(255, v, t, p),
                1 => Color.FromArgb(255, q, v, p),
                2 => Color.FromArgb(255, p, v, t),
                3 => Color.FromArgb(255, p, q, v),
                4 => Color.FromArgb(255, t, p, v),
                _ => Color.FromArgb(255, v, p, q),
            };
        }
    }
}