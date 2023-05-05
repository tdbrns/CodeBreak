namespace CodeBreak.Pages;

public partial class DifficultySelection : ContentPage
{
    public DifficultySelection()
    {
        InitializeComponent();
        DifficultyAnimation();
    }

    public async void DifficultyAnimation()
    {
        lblDifficulty.Opacity = 0;
        rdoEasy.Opacity = 0;
        rdoNormal.Opacity = 0;
        rdoHard.Opacity = 0;
        rdoImpossible.Opacity = 0;
        btnStart.Opacity = 0;
        rdoEasy.IsEnabled = false;
        rdoNormal.IsEnabled = false;
        rdoHard.IsEnabled = false;
        rdoImpossible.IsEnabled = false;
        btnStart.IsEnabled = false;

        await lblDifficulty.FadeTo(0.5, 150);
        await Task.WhenAll<bool>
            (
                lblDifficulty.FadeTo(1, 150),
                rdoEasy.FadeTo(0.5, 150)
            );
        await Task.WhenAll<bool>
            (
                rdoEasy.FadeTo(1, 150),
                rdoNormal.FadeTo(0.5, 150)
            );
        await Task.WhenAll<bool>
            (
                rdoNormal.FadeTo(1, 150),
                rdoHard.FadeTo(0.5, 150)
            );
        await Task.WhenAll<bool>
            (
                rdoHard.FadeTo(1, 150),
                rdoImpossible.FadeTo(0.5, 150)
            );
        await Task.WhenAll<bool>
            (
                rdoImpossible.FadeTo(1, 150),
                btnStart.FadeTo(0.5, 150)
            );
        await btnStart.FadeTo(1, 150);

        rdoEasy.IsEnabled = true;
        rdoNormal.IsEnabled = true;
        rdoHard.IsEnabled = true;
        rdoImpossible.IsEnabled = true;
        btnStart.IsEnabled = true;
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