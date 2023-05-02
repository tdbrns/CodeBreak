using Microsoft.Maui.Controls;
using CodeBreak.Pages;
namespace CodeBreak;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        PlayAnimation1();
    }

    // Set up the animation of the Main Menu text
    public void InitializeAnimation1()
    {
        //btnNewGame.TranslationX = 100;
        //btnLeaderboards.TranslationX = 100;
        //btnQuit.TranslationX = 100;
        lblCode.Opacity = 0;
        btnNewGame.Opacity = 0;
        btnLeaderboards.Opacity = 0;
        btnQuit.Opacity = 0;
        btnNewGame.IsEnabled = false;
        btnLeaderboards.IsEnabled = false;
        btnQuit.IsEnabled = false;
    }

    // Animates the Main Menu text
    public async void PlayAnimation1()
    {
        InitializeAnimation1();

        await Task.Delay(1000);

        await lblCode.FadeTo(1, 1000);
        await Task.Delay(150);
        await btnNewGame.FadeTo(0.5, 150);
        await Task.WhenAll<bool>
            (
                btnLeaderboards.FadeTo(0.5, 150),
                btnNewGame.FadeTo(1, 150)
            );
        await Task.WhenAll<bool>
            (
                btnQuit.FadeTo(0.5, 150),
                btnLeaderboards.FadeTo(1, 150)
            );
        await btnQuit.FadeTo(1, 150);

        btnNewGame.IsEnabled = true;
        btnLeaderboards.IsEnabled = true;
        btnQuit.IsEnabled = true;
    }

    public async void NewGame(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NameEntry());
    }

    public async void ShowLeaderboards(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EasyLeaderboard());
    }

    public void QuitGame(object sender, EventArgs e)
    {
        System.Environment.Exit(0);
    }
}