using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Input;
using Avalonia.Threading;

using AnimeNotepad.Views.Main;

namespace AnimeNotepad.Views.Splash;

public partial class SplashScreen : Window
{
    private readonly DispatcherTimer _timer;
    private bool _transitioned = false;

    public SplashScreen()
    {
        InitializeComponent();
        
        _timer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(2.5)
        };
        _timer.Tick += Timer_Tick;
        _timer.Start();
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        TransitionToMain();
    }

    private void SplashScreen_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        TransitionToMain();
    }

    private void SplashScreen_KeyDown(object? sender, KeyEventArgs e)
    {
        TransitionToMain();
    }

    private void TransitionToMain()
    {
        if (_transitioned) return;
        _transitioned = true;
        _timer.Stop();

        var mainWindow = new MainWindow();
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = mainWindow;
        }

        mainWindow.Show();
        this.Close();
    }
}
