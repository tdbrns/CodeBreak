namespace CodeBreak.Pages;

public partial class NameAcknowledgement : ContentPage
{
    public List<string> playerList = new List<string>();
    public string playerName;
    public NameAcknowledgement()
    {
        InitializeComponent();

        AcknowledgeName();
    }

    public void AcknowledgeName()
    {
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
        }
        else if (playerList.Contains(App.CurrentName) == true)
        {
            playerList = null;

            lblAcknowledgement.Text = "Welcome Back";
            lblPlayerName.Text = App.CurrentName;
        }
    }

    async void NextPage(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new DifficultySelection());
    }
}