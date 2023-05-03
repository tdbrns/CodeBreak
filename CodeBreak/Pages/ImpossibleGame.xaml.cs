namespace CodeBreak.Pages;

public partial class ImpossibleGame : ContentPage
{
    public DataSaving save = new DataSaving();
    public ImpossibleGame()
    {
        InitializeComponent();

        lblPlayer.Text = $"Name: {App.CurrentName}";
        lblDifficulty.Text = $"Difficulty: {App.CurrentDifficulty}";
        stprScoreCount.Value = 0;
        lblScore.Text = Convert.ToString(stprScoreCount.Value);
    }

    public void IncrementScore(object sender, ValueChangedEventArgs e)
    {
        int score = Convert.ToInt32(e.NewValue);

        App.CurrentScore = score;
        lblScore.Text = score.ToString();
    }

    public async void ReturnToMainMenu(object sender, EventArgs e)
    {
        save.SaveImpossiblePlayerData();

        save = null;
        await Navigation.PopToRootAsync();
    }
}