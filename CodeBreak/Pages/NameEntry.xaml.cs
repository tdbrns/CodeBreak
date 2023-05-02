namespace CodeBreak.Pages;

public partial class NameEntry : ContentPage
{
    public List<string> playerList = new List<string>();
    public NameEntry()
    {
        InitializeComponent();
        lblAcknowledgement.IsVisible = false;
        lblPlayerName.IsVisible = false;
        btnNext.IsVisible = false;
        btnNext.IsEnabled = false;
        lblErrorMsg.IsVisible = false;
    }

    public void ConfirmName(object sender, EventArgs e)
    {
        if (txtPlayerName.Text == "" || string.IsNullOrWhiteSpace(txtPlayerName.Text))
        {
            lblErrorMsg.Text = "Invalid name";
            lblErrorMsg.IsVisible = true;
        }
        else if (txtPlayerName.Text.Contains(" "))
        {
            lblErrorMsg.Text = "Name cannot contain spaces";
            lblErrorMsg.IsVisible = true;
        }
        else
        {
            lblErrorMsg.IsVisible = false;

            App.CurrentName = txtPlayerName.Text;

            StreamReader reader = new StreamReader(App.PlayersTextFilePath);
            string line = reader.ReadLine();

            while (line != null)
            {
                playerList.Add(line);
                line = reader.ReadLine();
            }

            reader.Close();

            if (playerList.Count < 0 || playerList.Contains(App.CurrentName) == false)
            {
                StreamWriter writer = new StreamWriter(App.PlayersTextFilePath, true);
                writer.WriteLine(App.CurrentName);
                writer.Close();
                playerList = null;

                lblAcknowledgement.Text = "New Player";
                lblPlayerName.Text = App.CurrentName;
                PlayAnimation2();
            }
            else if (playerList.Contains(App.CurrentName) == true)
            {
                playerList = null;

                lblAcknowledgement.Text = "Welcome Back";
                lblPlayerName.Text = App.CurrentName;
                PlayAnimation2();
            }
        }
    }

    public async void PlayAnimation2()
    {
        txtPlayerName.IsEnabled = false;
        btnConfirm.IsEnabled = false;

        lblAcknowledgement.IsVisible = true;
        lblPlayerName.IsVisible = true;
        btnNext.IsVisible = true;
        lblAcknowledgement.TranslationY = 100;
        lblPlayerName.TranslationY = 100;
        btnNext.TranslationY = 100;
        lblAcknowledgement.Opacity = 0;
        lblPlayerName.Opacity = 0;
        btnNext.Opacity = 0;

        await Task.WhenAll<bool>
            (
                lblTitle.TranslateTo(0, -100, 250),
                lblTitle.FadeTo(0, 250),
                txtPlayerName.TranslateTo(0, -100, 250),
                txtPlayerName.FadeTo(0, 250),
                btnConfirm.TranslateTo(0, -100, 250),
                btnConfirm.FadeTo(0, 250)
            );

        lblTitle.IsVisible = false;
        txtPlayerName.IsVisible = false;
        btnConfirm.IsVisible = false;

        await Task.WhenAll<bool>
            (
                lblAcknowledgement.TranslateTo(0, 0, 250),
                lblAcknowledgement.FadeTo(1, 250),
                lblPlayerName.TranslateTo(0, 0, 250),
                lblPlayerName.FadeTo(1, 250),
                btnNext.TranslateTo(0, 0, 250),
                btnNext.FadeTo(1, 250)
            );

        btnNext.IsEnabled = true;
    }

    public async void NextPage(object sender, EventArgs e)
    {
        txtPlayerName.Text = null;
        await Navigation.PushAsync(new DifficultySelection());
    }
}