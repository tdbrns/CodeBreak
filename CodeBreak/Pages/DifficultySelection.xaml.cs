namespace CodeBreak.Pages;

public partial class DifficultySelection : ContentPage
{
    public DifficultySelection()
    {
        InitializeComponent();
    }

    public async void StartGame(object sender, EventArgs e)
    {
        if (rdoEasy.IsChecked == true)
        {
            App.CurrentDifficulty = "Easy";
            await Navigation.PushAsync(new EasyGame());
        }
        else if (rdoNormal.IsChecked == true)
        {
            App.CurrentDifficulty = "Normal";
            await Navigation.PushAsync(new NormalGame());
        }
        else if (rdoHard.IsChecked == true)
        {
            App.CurrentDifficulty = "Hard";
            await Navigation.PushAsync(new HardGame());
        }
        else if (rdoImpossible.IsChecked == true)
        {
            App.CurrentDifficulty = "Impossible";
            await Navigation.PushAsync(new ImpossibleGame());
        }

    }
}