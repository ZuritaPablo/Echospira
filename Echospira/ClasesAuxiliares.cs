using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProyectoGrafica
{
    internal class Punto3D
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    internal class PuntoProyectado
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Alpha { get; set; }
        public bool Visible { get; set; }
    }
}