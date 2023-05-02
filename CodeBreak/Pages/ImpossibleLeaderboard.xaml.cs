using System.Collections.ObjectModel;

namespace CodeBreak.Pages;

public partial class ImpossibleLeaderboard : ContentPage
{
    public ImpossibleLeaderboard()
    {
        InitializeComponent();
        ShowPlayersByScore();
    }

    public async void ShowPlayersByScore()
    {
        var players = await PlayerDatabase.GetImpossiblePlayersByScore();

        ObservableCollection<ImpossiblePlayers> easylist = new ObservableCollection<ImpossiblePlayers>(players);

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

    public async void ToHardBoard(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new HardLeaderboard());
    }
    private async void ReturnToMainMenu(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}