namespace CodeBreak.Pages;

public partial class NameEntry : ContentPage
{
    public NameEntry()
    {
        InitializeComponent();
        lblErrorMsg.IsVisible = false;
        NameEntryAnimation();
    }

    public async void NameEntryAnimation()
    {
        lblTitle.Opacity = 0;
        txtPlayerName.Opacity = 0;
        btnConfirm.Opacity = 0;
        txtPlayerName.IsEnabled = false;
        btnConfirm.IsEnabled = false;

        await Task.Delay(150);

        await lblTitle.FadeTo(0.5, 150);
        await Task.WhenAll<bool>
            (
                lblTitle.FadeTo(1, 150),
                txtPlayerName.FadeTo(0.5, 150)
            );
        await Task.WhenAll<bool>
            (
                txtPlayerName.FadeTo(1, 150),
                btnConfirm.FadeTo(0.5, 150)
            );
        await btnConfirm.FadeTo(1, 250);

        txtPlayerName.IsEnabled = true;
        btnConfirm.IsEnabled = true;
    }

    public async void ConfirmNameOnEnter(object sender, EventArgs e)
    {
        string playerName = ((Entry)sender).Text;

        if (playerName == "" || string.IsNullOrWhiteSpace(playerName))
        {
            lblErrorMsg.Text = "Invalid name";
            lblErrorMsg.IsVisible = true;
        }
        else if (playerName.Contains(" "))
        {
            lblErrorMsg.Text = "Name cannot contain spaces";
            lblErrorMsg.IsVisible = true;
        }
        else
        {
            lblErrorMsg.IsVisible = false;
            App.CurrentName = playerName;

            await Navigation.PushAsync(new NameAcknowledgement());
        }
    }
    public async void ConfirmNameOnClick(object sender, EventArgs e)
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