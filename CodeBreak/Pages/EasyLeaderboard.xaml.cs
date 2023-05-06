using System.Collections.ObjectModel;

namespace CodeBreak.Pages;

public partial class EasyLeaderboard : ContentPage
{
    public EasyLeaderboard()
    {
        InitializeComponent();
        ShowPlayersByScore();
    }

    public async void ShowPlayersByScore()
    {
        var players = await PlayerDatabase.GetEasyPlayersByScore();
        ObservableCollection<EasyPlayers> easylist = new ObservableCollection<EasyPlayers>(players);

        // Each player is assigned a rank according to their score.
        int rankCount = 1;

        foreach (var player in easylist)
        {
            player.Rank = rankCount;
            rankCount++;
        }

        collectionPlayers.ItemsSource = easylist;
    }

    public async void ToNormalBoard(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NormalLeaderboard());
    }

    public async void ToHardBoard(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HardLeaderboard());
    }

    public async void ToImpossibleBoard(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ImpossibleLeaderboard());
    }

    private async void ReturnToMainMenu(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}