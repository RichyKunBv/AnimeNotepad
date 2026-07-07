using System;
using Avalonia.Controls;
using Avalonia.Threading;

using AnimeNotepad.Views.Main;

namespace AnimeNotepad.Views.Splash;

public partial class SplashScreen : Window
{
    private DispatcherTimer _timer;

    public SplashScreen()
    {
        InitializeComponent();
        
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(3)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _timer.Stop();
        
        var mainWindow = new MainWindow();
        mainWindow.Show();
        
        this.Close();
    }
}
