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
            rdoEasy.TextColor = Color.Parse("White");
        else if (rdoNormal.IsChecked == true)
            App.CurrentDifficulty = "Normal";
        else if (rdoHard.IsChecked == true)
            App.CurrentDifficulty = "Hard";
        else if (rdoImpossible.IsChecked == true)
            App.CurrentDifficulty = "Impossible";

        await Navigation.PushAsync(new EasyDotTable());
    }
}