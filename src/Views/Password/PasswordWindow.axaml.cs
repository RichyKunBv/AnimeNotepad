using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace AnimeNotepad.Views.Password;

public partial class PasswordWindow : Window
{
    public string? Password { get; private set; }

    public PasswordWindow() : this("Escribe la contraseña:")
    {
    }

    public PasswordWindow(string prompt)
    {
        InitializeComponent();
        PromptText.Text = prompt;
        Opened += (_, _) => PasswordTextBox.Focus();
    }

    private void AcceptButton_Click(object? sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(PasswordTextBox.Text)) return;
        Password = PasswordTextBox.Text;
        Close(true);
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e) => Close(false);

    private void PasswordTextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter) AcceptButton_Click(sender, e);
        if (e.Key == Key.Escape) CancelButton_Click(sender, e);
    }
}