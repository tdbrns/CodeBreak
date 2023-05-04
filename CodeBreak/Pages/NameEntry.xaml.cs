namespace CodeBreak.Pages;

public partial class NameEntry : ContentPage
{
    public NameEntry()
    {
        InitializeComponent();
        lblErrorMsg.IsVisible = false;
    }

    public async void ConfirmName(object sender, EventArgs e)
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

            await Navigation.PushAsync(new NameAcknowledgement());
        }
    }
}