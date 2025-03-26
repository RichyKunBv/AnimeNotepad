using System;
using System.Windows.Forms;

namespace webiwabo
{
    public partial class SpashScreen : Form
    {
        public int TimeLeft { get; set; } // Temporizador interno

        public SpashScreen()
        {
            InitializeComponent();
            TimeLeft = 3; // Duración del splash en segundos
        }

        private void SpashScreen_Load(object sender, EventArgs e)
        {
            timer1.Interval = 10000; // 1 segundo
            timer1.Tick += timer1_Tick; // Asignar evento
            timer1.Start(); // Iniciar el temporizador
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TimeLeft > 0)
            {
                TimeLeft--; // Reducir el contador cada segundo
            }
            else
            {
                timer1.Stop(); // Detener el temporizador
                this.Hide(); // Ocultar el SplashScreen

                Form1 mainForm = new Form1();
                mainForm.Show(); // Mostrar Form1

                this.Close(); // Cerrar completamente el SplashScreen
            }
        }
    }
}
