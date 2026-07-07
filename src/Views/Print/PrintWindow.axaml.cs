using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AnimeNotepad.Views.Print
{
    public partial class PrintWindow : Window
    {
        public bool PrintConfirmed { get; private set; } = false;

        public PrintWindow()
        {
            InitializeComponent();
        }

        private void Aceptar_Click(object sender, RoutedEventArgs e)
        {
            PrintConfirmed = true;
            Close(true);
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            PrintConfirmed = false;
            Close(false);
        }
    }
}
