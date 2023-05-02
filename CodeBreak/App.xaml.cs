using CodeBreak;
using System.IO;

namespace CodeBreak;

public partial class App : Application
{
    public static string PlayersTextFilePath { get; private set; }
    public static string CurrentName { get; set; }
    public static string CurrentDifficulty { get; set; }
    public static int CurrentScore { get; set; }

    public App()
    {
        InitializeComponent();

        // playersTextFilePath receives the file path of the AllPlayers.txt file
        PlayersTextFilePath = MakePlayersTextFile();

        // Use ClearPlayerRecords method if you wish to delete all saved player data before running a new iteration of Pong Madness.
        //ClearPlayerData();

        MainPage = new NavigationPage(new MainPage());
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);

        const int newWidth = 1050;
        const int newHeight = 650;

        window.Width = newWidth;
        window.Height = newHeight;

        window.MinimumWidth = newWidth;
        window.MinimumHeight = newHeight;

        return window;
    }

    private static string MakePlayersTextFile()
    {
        // Stores the .txt file at AppData\Local\Packages\489e3d28-c377-4149-98b9-fea101023ba9_9zz4h110yvjzm\LocalState\AllPlayers.txt
        var path = FileSystem.Current.AppDataDirectory;
        var fullPath = Path.Combine(path, "AllPlayers.txt");

        StreamWriter writer = new StreamWriter(fullPath, true);
        writer.Write(String.Empty);
        writer.Close();

        return fullPath;
    }

    private static void ClearPlayerData()
    {
        File.Delete(PlayersTextFilePath);
        PlayerDatabase.DeleteAllPlayers();

        PlayersTextFilePath = MakePlayersTextFile();
    }
}
