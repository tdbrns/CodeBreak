namespace CodeBreak.Pages;

public partial class HowToPlay : ContentPage
{
	public HowToPlay()
	{
		InitializeComponent();

        lblInfo.Text = "- Code Break is a memory game where you must quickly memorize and replicate a series of random patterns to earn points.\n\n" +
			"- Once the game starts, you will be presented with a table of buttons. One button in each column will turn yellow to show a pattern before disappearing.\n\n" +
			"- When the pattern of yellow buttons disappears, you must correctly replicate the pattern by selecting the correct buttons on the table.\n\n" +
			"- Once you select a button in one of the columns, all of the buttons in that column will be disabled, so be careful!\n\n" +
			"- When you think you have created the correct pattern, click the \"Submit\" button.\n\n" +
			"- If your pattern is correct, you will earn a point!\n\n" +
			"- If your pattern is incorrect, you will lose the game.\n\n" +
			"- You will have 1 minute to successfully replicate as many patterns as you can!\n\n" +
			"- As you correctly replicate more patterns, the time during which the correct pattern is shown will decrease. Remember to stay focused!\n\n";
	}
}