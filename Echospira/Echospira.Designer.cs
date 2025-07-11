namespace ProyectoGrafica
{
    partial class Echospira
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pictureCanvas = new PictureBox();
            btnPlay = new Button();
            btnPause = new Button();
            btnStop = new Button();
            btnAdelantar = new Button();
            btnRetroceder = new Button();
            barraProgreso = new ProgressBar();
            btnUpload = new Button();
            cbxListMusic = new ComboBox();
            gbxMusic = new GroupBox();
            gbxControls = new GroupBox();
            gbxAnother = new GroupBox();
            ((System.ComponentModel.ISupportInitialize)pictureCanvas).BeginInit();
            gbxMusic.SuspendLayout();
            gbxControls.SuspendLayout();
            SuspendLayout();
            // 
            // pictureCanvas
            // 
            pictureCanvas.BackColor = Color.Black;
            pictureCanvas.Dock = DockStyle.Top;
            pictureCanvas.Location = new Point(0, 0);
            pictureCanvas.Margin = new Padding(3, 2, 3, 2);
            pictureCanvas.Name = "pictureCanvas";
            pictureCanvas.Size = new Size(891, 450);
            pictureCanvas.TabIndex = 0;
            pictureCanvas.TabStop = false;
            pictureCanvas.Click += pictureCanvas_Click;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = SystemColors.ActiveCaptionText;
            btnPlay.Cursor = Cursors.Hand;
            btnPlay.Font = new Font("Calibri", 15.75F, FontStyle.Bold);
            btnPlay.ForeColor = SystemColors.AppWorkspace;
            btnPlay.Location = new Point(11, 21);
            btnPlay.Margin = new Padding(3, 2, 3, 2);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(50, 50);
            btnPlay.TabIndex = 7;
            btnPlay.Text = "▶";
            btnPlay.UseVisualStyleBackColor = false;
            btnPlay.Click += btnPlay_Click;
            // 
            // btnPause
            // 
            btnPause.BackColor = SystemColors.ActiveCaptionText;
            btnPause.Font = new Font("Calibri", 15.75F, FontStyle.Bold);
            btnPause.ForeColor = SystemColors.ButtonFace;
            btnPause.Location = new Point(123, 21);
            btnPause.Margin = new Padding(3, 2, 3, 2);
            btnPause.Name = "btnPause";
            btnPause.Size = new Size(50, 50);
            btnPause.TabIndex = 8;
            btnPause.Text = "⏸";
            btnPause.UseVisualStyleBackColor = false;
            // 
            // btnStop
            // 
            btnStop.BackColor = SystemColors.ActiveCaptionText;
            btnStop.Font = new Font("Calibri", 15.75F, FontStyle.Bold);
            btnStop.ForeColor = SystemColors.ButtonFace;
            btnStop.Location = new Point(67, 21);
            btnStop.Margin = new Padding(3, 2, 3, 2);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(50, 50);
            btnStop.TabIndex = 9;
            btnStop.Text = "⏯";
            btnStop.UseVisualStyleBackColor = false;
            // 
            // btnAdelantar
            // 
            btnAdelantar.BackColor = SystemColors.ActiveCaptionText;
            btnAdelantar.Font = new Font("Calibri", 15.75F, FontStyle.Bold);
            btnAdelantar.ForeColor = SystemColors.ButtonFace;
            btnAdelantar.Location = new Point(235, 21);
            btnAdelantar.Margin = new Padding(3, 2, 3, 2);
            btnAdelantar.Name = "btnAdelantar";
            btnAdelantar.Size = new Size(50, 50);
            btnAdelantar.TabIndex = 10;
            btnAdelantar.Text = "⏩";
            btnAdelantar.UseVisualStyleBackColor = false;
            // 
            // btnRetroceder
            // 
            btnRetroceder.BackColor = SystemColors.ActiveCaptionText;
            btnRetroceder.Font = new Font("Calibri", 15.75F, FontStyle.Bold);
            btnRetroceder.ForeColor = SystemColors.ButtonFace;
            btnRetroceder.Location = new Point(179, 21);
            btnRetroceder.Margin = new Padding(3, 2, 3, 2);
            btnRetroceder.Name = "btnRetroceder";
            btnRetroceder.Size = new Size(50, 50);
            btnRetroceder.TabIndex = 11;
            btnRetroceder.Text = "⏪";
            btnRetroceder.UseVisualStyleBackColor = false;
            // 
            // barraProgreso
            // 
            barraProgreso.BackColor = SystemColors.ActiveCaptionText;
            barraProgreso.ForeColor = SystemColors.Desktop;
            barraProgreso.Location = new Point(11, 81);
            barraProgreso.Margin = new Padding(3, 2, 3, 2);
            barraProgreso.Name = "barraProgreso";
            barraProgreso.Size = new Size(274, 22);
            barraProgreso.TabIndex = 12;
            // 
            // btnUpload
            // 
            btnUpload.BackColor = SystemColors.ActiveCaptionText;
            btnUpload.Font = new Font("Yu Gothic UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpload.ForeColor = SystemColors.AppWorkspace;
            btnUpload.Location = new Point(146, 62);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(97, 32);
            btnUpload.TabIndex = 13;
            btnUpload.Text = "⬆ Upload";
            btnUpload.UseVisualStyleBackColor = false;
            btnUpload.Click += btnUpload_Click;
            // 
            // cbxListMusic
            // 
            cbxListMusic.BackColor = SystemColors.InactiveCaptionText;
            cbxListMusic.ForeColor = SystemColors.ScrollBar;
            cbxListMusic.FormattingEnabled = true;
            cbxListMusic.Items.AddRange(new object[] { "Rapsodi.wav" });
            cbxListMusic.Location = new Point(18, 22);
            cbxListMusic.Name = "cbxListMusic";
            cbxListMusic.Size = new Size(225, 23);
            cbxListMusic.TabIndex = 14;
            cbxListMusic.Text = "Select the track";
            cbxListMusic.SelectedIndexChanged += cbxListMusic_SelectedIndexChanged;
            // 
            // gbxMusic
            // 
            gbxMusic.Controls.Add(cbxListMusic);
            gbxMusic.Controls.Add(btnUpload);
            gbxMusic.ForeColor = SystemColors.AppWorkspace;
            gbxMusic.Location = new Point(617, 455);
            gbxMusic.Name = "gbxMusic";
            gbxMusic.Size = new Size(262, 116);
            gbxMusic.TabIndex = 15;
            gbxMusic.TabStop = false;
            gbxMusic.Text = "Music";
            // 
            // gbxControls
            // 
            gbxControls.Controls.Add(barraProgreso);
            gbxControls.Controls.Add(btnRetroceder);
            gbxControls.Controls.Add(btnAdelantar);
            gbxControls.Controls.Add(btnStop);
            gbxControls.Controls.Add(btnPause);
            gbxControls.Controls.Add(btnPlay);
            gbxControls.ForeColor = SystemColors.ButtonFace;
            gbxControls.Location = new Point(11, 455);
            gbxControls.Name = "gbxControls";
            gbxControls.Size = new Size(299, 116);
            gbxControls.TabIndex = 16;
            gbxControls.TabStop = false;
            gbxControls.Text = "Controls";
            // 
            // gbxAnother
            // 
            gbxAnother.Location = new Point(316, 455);
            gbxAnother.Name = "gbxAnother";
            gbxAnother.Size = new Size(295, 116);
            gbxAnother.TabIndex = 17;
            gbxAnother.TabStop = false;
            // 
            // FormProyecto
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            ClientSize = new Size(891, 583);
            Controls.Add(gbxAnother);
            Controls.Add(gbxControls);
            Controls.Add(gbxMusic);
            Controls.Add(pictureCanvas);
            Margin = new Padding(3, 2, 3, 2);
            Name = "FormProyecto";
            Text = "FormProyecto";
            Load += FormProyecto_Load;
            ((System.ComponentModel.ISupportInitialize)pictureCanvas).EndInit();
            gbxMusic.ResumeLayout(false);
            gbxControls.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private PictureBox pictureCanvas;
        private Button btnPlay;
        private Button btnPause;
        private Button btnStop;
        private Button btnAdelantar;
        private Button btnRetroceder;
        private ProgressBar barraProgreso;
        private Button btnUpload;
        private ComboBox cbxListMusic;
        private GroupBox gbxMusic;
        private GroupBox gbxControls;
        private GroupBox gbxAnother;
    }
}