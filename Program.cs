using System;
using System.Windows.Forms;

namespace webiwabo
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            // Mostrar el SplashScreen
            using (SpashScreen splash = new SpashScreen())
            {
                splash.Show();
                Application.DoEvents(); // Permite actualizar la UI mientras se carga
                System.Threading.Thread.Sleep(3000); // Espera 3 segundos (ajustable)
                splash.Close();
            }

            // Luego abrir Form1
            Application.Run(new Form1());
        }
    }
}
