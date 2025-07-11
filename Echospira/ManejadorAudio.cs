using System;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace ProyectoGrafica
{
    internal class ManejadorAudio : IDisposable
    {
        private IWavePlayer waveOut;
        private AudioFileReader audioFile;
        private MeteringSampleProvider meteringProvider;
        private float currentVolume = 0f;

        public void InicializarAudio(string rutaArchivo)
        {
            audioFile = new AudioFileReader(rutaArchivo);
            meteringProvider = new MeteringSampleProvider(audioFile);
            meteringProvider.StreamVolume += MeteringProvider_StreamVolume;

            waveOut = new WaveOutEvent();
            waveOut.Init(meteringProvider);
        }

        private void MeteringProvider_StreamVolume(object sender, StreamVolumeEventArgs e)
        {
            float maxVol = 0f;
            foreach (var vol in e.MaxSampleValues)
                if (vol > maxVol)
                    maxVol = vol;
            currentVolume = maxVol;
        }

        public float ObtenerVolumenActual()
        {
            return currentVolume;
        }

        public void Reproducir()
        {
            if (waveOut != null && waveOut.PlaybackState != PlaybackState.Playing)
                waveOut.Play();
        }

        public void Pausar()
        {
            if (waveOut != null && waveOut.PlaybackState == PlaybackState.Playing)
                waveOut.Pause();
        }

        public void Detener()
        {
            if (waveOut != null)
                waveOut.Stop();
            if (audioFile != null)
                audioFile.Position = 0;
        }

        public void Adelantar(int segundos)
        {
            if (audioFile != null)
            {
                long nuevo = audioFile.Position + audioFile.WaveFormat.AverageBytesPerSecond * segundos;
                audioFile.Position = Math.Min(audioFile.Length, nuevo);
            }
        }

        public void Retroceder(int segundos)
        {
            if (audioFile != null)
            {
                long nuevo = audioFile.Position - audioFile.WaveFormat.AverageBytesPerSecond * segundos;
                audioFile.Position = Math.Max(0, nuevo);
            }
        }

        public void IrAProgreso(float progreso)
        {
            if (audioFile != null)
            {
                long posicion = (long)(audioFile.Length * progreso);
                audioFile.Position = posicion;
            }
        }

        public float ObtenerProgreso()
        {
            if (audioFile == null || audioFile.Length == 0) return 0f;
            return (float)audioFile.Position / audioFile.Length;
        }

        public void Dispose()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (audioFile != null)
            {
                audioFile.Dispose();
                audioFile = null;
            }
        }
    }
}
