namespace CodeBreak.Pages;

public partial class EasyDotTable : ContentPage
{
    public DataSaving save = new DataSaving();
    public EasyDotTable()
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
        switch (App.CurrentDifficulty)
        {
            case "Easy":
                save.SaveEasyPlayerData();
                break;

            case "Normal":
                save.SaveNormalPlayerData();
                break;

            case "Hard":
                save.SaveHardPlayerData();
                break;

            case "Impossible":
                save.SaveImpossiblePlayerData();
                break;

            default:
                break;
        }

        save = null;
        await Navigation.PopToRootAsync();
    }
}