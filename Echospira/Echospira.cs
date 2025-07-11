using System;
using System.Drawing;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using ProyectoGrafica.Graficos;

namespace ProyectoGrafica
{
    public partial class Echospira : Form
    {
        private System.Windows.Forms.Timer timerAnimacion;
        private System.Windows.Forms.Timer timerProgreso;
        private ManejadorAudio manejadorAudio;
        private AnalizadorAudio analizadorAudio;
        private RenderizadorEsfera renderizadorEsfera;
        private bool reproduciendo = false;
        private bool animando = true;



        public Echospira()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            manejadorAudio = new ManejadorAudio();
            analizadorAudio = new AnalizadorAudio();
            renderizadorEsfera = new RenderizadorEsfera(pictureCanvas);


            pictureCanvas.Paint += PictureCanvas_Paint;

            timerAnimacion = new System.Windows.Forms.Timer();
            timerAnimacion.Interval = 16; // ~60 FPS
            timerAnimacion.Tick += TimerAnimacion_Tick;
            timerAnimacion.Start();
            timerProgreso = new System.Windows.Forms.Timer();
            timerProgreso.Interval = 100;
            timerProgreso.Tick += TimerProgreso_Tick;

            UploadFilesToComboBox();
           

            

            ReadAudioTrack();


            btnPlay.Click += (s, e) =>
            {

                
                manejadorAudio.Reproducir();
                reproduciendo = true;
                animando = false;
                timerAnimacion.Start();
                timerProgreso.Start();
            };

            btnPause.Click += (s, e) =>
            {
                manejadorAudio.Pausar();
                reproduciendo = false;
                animando = true;
                timerProgreso.Stop();
            };

            btnStop.Click += (s, e) =>
            {
                stopMusic();
            };

            btnAdelantar.Click += (s, e) =>
            {
                manejadorAudio.Adelantar(5); // Adelanta 5 segundos
            };

            btnRetroceder.Click += (s, e) =>
            {
                manejadorAudio.Retroceder(5); // Retrocede 5 segundos
            };

            barraProgreso.MouseDown += (s, e) => timerProgreso.Stop();
            barraProgreso.MouseUp += (s, e) =>
            {
                float progreso = barraProgreso.Value / 100f;
                manejadorAudio.IrAProgreso(progreso);
                if (reproduciendo) timerProgreso.Start();
            };
        }
        public void stopMusic()
        {
            manejadorAudio.Detener();
            reproduciendo = false;
            animando = true;
            timerProgreso.Stop();
            barraProgreso.Value = 0;
        }
        public void ReadAudioTrack()
        {
            try
            {
                string? nombreArchivo = cbxListMusic.SelectedItem?.ToString();

                
                // Ruta completa hacia la carpeta Audios
                string rutaArchivo = Path.Combine(Application.StartupPath, "Audios", nombreArchivo);

                // Verificar si el archivo existe
                if (!File.Exists(rutaArchivo))
                {
                    MessageBox.Show("The selected file does not exist in the Audios folder");
                    return;
                }
                manejadorAudio.InicializarAudio(rutaArchivo);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading audio file: " + ex.Message);
            }
            
        }
        private void TimerAnimacion_Tick(object sender, EventArgs e)
        {
            if (reproduciendo)
            {
                float volumenActual = manejadorAudio.ObtenerVolumenActual();
                analizadorAudio.ActualizarAnalisis(volumenActual);
                renderizadorEsfera.ActualizarAnimacion(analizadorAudio);
                pictureCanvas.Invalidate();
            }
            else if (animando)
            {
                analizadorAudio.ActualizarAnalisis(0); // Mantener animación con volumen 0
                renderizadorEsfera.ActualizarAnimacion(analizadorAudio);
                pictureCanvas.Invalidate();
            }

        }

        private void TimerProgreso_Tick(object sender, EventArgs e)
        {
            float progreso = manejadorAudio.ObtenerProgreso(); // Valor entre 0 y 1
            barraProgreso.Value = Math.Min(barraProgreso.Maximum, (int)(progreso * barraProgreso.Maximum));
        }

        private void PictureCanvas_Paint(object sender, PaintEventArgs e)
        {
            renderizadorEsfera.Renderizar(e.Graphics, analizadorAudio);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            manejadorAudio?.Dispose();
        }

        private void FormProyecto_Load(object sender, EventArgs e)
        {

        }

        private void pictureCanvas_Click(object sender, EventArgs e)
        {

        }

        private void btnAdelantar_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnPlay_Click(object sender, EventArgs e)
        {

        }
        private void UploadFilesToComboBox()
        {
            string carpetaAudios = Path.Combine(Application.StartupPath, "Audios");

            if (Directory.Exists(carpetaAudios))
            {
                string[] archivos = Directory.GetFiles(carpetaAudios);

                cbxListMusic.Items.Clear(); // Limpia antes de volver a cargar

                foreach (string ruta in archivos)
                {
                    cbxListMusic.Items.Add(Path.GetFileName(ruta)); // Solo el nombre, sin ruta completa
                }
            }
            if (cbxListMusic.Items.Count > 0)
            {
                cbxListMusic.SelectedIndex = 0; // 👉 Selecciona el primer ítem automáticamente
            }
        }


        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Archivos de audio tipo wav (*.wav)|*.wav|Todos los archivos (*.*)|*.*";
            ofd.Title = "Seleccionar archivos de audio";
            ofd.Multiselect = true;  // ✅ Permitir seleccionar varios archivos

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Carpeta destino dentro de la carpeta del ejecutable
                    string carpetaDestino = Path.Combine(Application.StartupPath, "Audios");

                    // Crear la carpeta si no existe
                    if (!Directory.Exists(carpetaDestino))
                        Directory.CreateDirectory(carpetaDestino);

                    foreach (string archivoOrigen in ofd.FileNames)
                    {
                        string nombreArchivo = Path.GetFileName(archivoOrigen);
                        string archivoDestino = Path.Combine(carpetaDestino, nombreArchivo);

                        // Copiar el archivo (sobrescribe si ya existe)
                        File.Copy(archivoOrigen, archivoDestino, true);
                    }

                    MessageBox.Show("Archivos subidos correctamente.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error al subir los archivos: " + ex.Message);
                }
            }
            UploadFilesToComboBox();
        }

        private void cbxListMusic_SelectedIndexChanged(object sender, EventArgs e)
        {
            stopMusic();
            ReadAudioTrack();
        }
    }
}
