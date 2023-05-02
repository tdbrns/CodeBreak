using System.Collections.ObjectModel;

namespace CodeBreak.Pages;

public partial class HardLeaderboard : ContentPage
{
    public HardLeaderboard()
    {
        InitializeComponent();
        ShowPlayersByScore();
    }
    public async void ShowPlayersByScore()
    {
        var players = await PlayerDatabase.GetHardPlayersByScore();
        ObservableCollection<HardPlayers> easylist = new ObservableCollection<HardPlayers>(players);

        int rankCount = 1;

        foreach (var player in easylist)
        {
            player.Rank = rankCount;
            rankCount++;
        }

        collectionPlayers.ItemsSource = easylist;
    }

    public async void ToEasyBoard(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new EasyLeaderboard());
    }

    public async void ToNormalBoard(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new NormalLeaderboard());
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