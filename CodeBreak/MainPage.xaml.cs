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

    // Animates the Main Menu text
    public async void PlayAnimation1()
    {
        lblCodeBreak.Opacity = 0;
        btnNewGame.Opacity = 0;
        btnLeaderboards.Opacity = 0;
        btnHowToPlay.Opacity = 0;
        btnQuit.Opacity = 0;
        btnNewGame.IsEnabled = false;
        btnLeaderboards.IsEnabled = false;
        btnHowToPlay.IsEnabled = false;
        btnQuit.IsEnabled = false;

        await Task.Delay(1000);

        await lblCodeBreak.FadeTo(1, 1000);
        await Task.Delay(500);
        await btnNewGame.FadeTo(0.5, 150);
        await Task.WhenAll<bool>
            (
                btnNewGame.FadeTo(1, 150),
                btnLeaderboards.FadeTo(0.5, 150)
            );
        await Task.WhenAll<bool>
            (
                btnLeaderboards.FadeTo(1, 150),
                btnHowToPlay.FadeTo(0.5, 150)
            );
        await Task.WhenAll<bool>
            (
                btnHowToPlay.FadeTo(1, 150),
                btnQuit.FadeTo(0.5, 150)
            );
        await btnQuit.FadeTo(1, 150);
        

        btnNewGame.IsEnabled = true;
        btnLeaderboards.IsEnabled = true;
        btnHowToPlay.IsEnabled = true;
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

    public async void ShowHowToPlay(object sender, EventArgs e)
    {
        await Navigation.PushAsync (new HowToPlay());
    }

    public void QuitGame(object sender, EventArgs e)
    {
        System.Environment.Exit(0);
    }
}