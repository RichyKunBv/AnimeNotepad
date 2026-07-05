using Avalonia.Controls;
using Avalonia.Interactivity;

namespace AnimeNotepad
{
    public partial class ManualWindow : Window
    {
        public ManualWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
