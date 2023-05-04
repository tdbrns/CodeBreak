namespace CodeBreak.Pages;

public partial class HardGame : ContentPage
{
    private DataSaving _save = new DataSaving();
    private readonly int[] _columnArray1 = new int[6];
    private readonly int[] _columnArray2 = new int[6];
    private readonly int[] _columnArray3 = new int[6];
    private readonly int[] _columnArray4 = new int[6];
    private readonly int[] _columnArray5 = new int[6];
    private readonly int[] _columnArray6 = new int[6];
    private readonly int[] _correctIndexes = new int[8];
    private readonly int[] _chosenIndexes = new int[8];
    private readonly bool[] _disabledColumns = new bool[8];
    private int _gameScore = 0;
    private int _remainingTries = 3;
    private bool _pauseTimer = false;
    private int _timerValue = 60;
    private int _viewTimeValue = 1000;

    static class Constants
    {
        public const int VIEWTIME_DECREMENT = 50;
        public const int MIN_VIEWTIME = 50;
        public const int NUM_COLUMNS = 6;
        public const int NUM_ROWS = 8;
    }


    public HardGame()
    {
        InitializeComponent();

        finalGrid.IsVisible = false;
        lblFinalScore.IsVisible = false;
        btnMainMenu.IsEnabled = false;
        btnMainMenu.IsVisible = false;

        lblScore.Text = $"Score: {_gameScore}";
        lblTries.Text = $"Tries: {_remainingTries}";
        lblTimer.Text = "60";


        HeaderAnimation();
        CreateNewPattern();
    }
    public async void HeaderAnimation()
    {
        lblScore.TranslationY = -250;
        lblTimer.TranslationY = -250;
        lblTries.TranslationY = -250;
        headerLine.TranslationY = -250;
        lblScore.Opacity = 0;
        lblTimer.Opacity = 0;
        lblTries.Opacity = 0;
        headerLine.Opacity = 0;

        await Task.WhenAll<bool>
            (
                lblScore.TranslateTo(0, 0, 500),
                lblTimer.TranslateTo(0, 0, 500),
                lblTries.TranslateTo(0, 0, 500),
                headerLine.TranslateTo(0, 0, 500),
                lblScore.FadeTo(1, 500),
                lblTimer.FadeTo(1, 500),
                lblTries.FadeTo(1, 500),
                headerLine.FadeTo(1, 500)
            );
    }
    public async void ScoreAnimation()
    {
        lblScore.Scale = 1;
        await lblScore.ScaleTo(1.2, 500, Easing.SpringIn);
        await lblScore.ScaleTo(1, 500, Easing.SpringOut);
    }
    public async void TriesAnimation()
    {
        lblTries.Scale = 1;
        await lblTries.ScaleTo(1.2, 500, Easing.SpringIn);
        await lblTries.ScaleTo(1, 500, Easing.SpringOut);
    }

    // Controls the 1 minute timer. If the timer runs out, if the timer is paused, or if the player runs out of tries, the while loop will
    // break
    public async void ProgressTimer()
    {
        while (_timerValue != 0 && _pauseTimer == false && _remainingTries != 0)
        {
            lblTimer.Text = _timerValue.ToString();
            await Task.Delay(1000);
            _timerValue--;
        }

        if (_timerValue == 0 || _remainingTries == 0)
        {
            lblTimer.Text = String.Empty;
            GameOver();
        }
    }
    public async void CreateNewPattern()
    {
        // Pause the timer and reset all columns
        _pauseTimer = true;
        ErasePattern();
        DisableAllColumns();

        // Initialize the rows of each column array to 0.
        for (int i = 0; i < Constants.NUM_ROWS; i++)
        {
            _columnArray1[i] = 0;
            _columnArray2[i] = 0;
            _columnArray3[i] = 0;
            _columnArray4[i] = 0;
            _columnArray5[i] = 0;
            _columnArray6[i] = 0;
        }

        // Generate five random numbers between 1 and 4 that will each represent one index of each column.
        Random rand = new Random();
        for (int i = 0; i < Constants.NUM_COLUMNS; i++)
        {
            _correctIndexes[i] = rand.Next(1, 5);
        }

        // The random values will dictate which elements in the column arrays will store a 1.
        // Every 1 in the column arrays represents a button that is a part of the correct pattern, and every 0 represents a button that is not
        // part of the correct pattern
        _columnArray1[_correctIndexes[0] - 1] = 1;
        _columnArray2[_correctIndexes[1] - 1] = 1;
        _columnArray3[_correctIndexes[2] - 1] = 1;
        _columnArray4[_correctIndexes[3] - 1] = 1;
        _columnArray5[_correctIndexes[4] - 1] = 1;
        _columnArray6[_correctIndexes[5] - 1] = 1;

        ShowCorrectButtonCol1();
        ShowCorrectButtonCol2();
        ShowCorrectButtonCol3();
        ShowCorrectButtonCol4();
        ShowCorrectButtonCol5();

        // The time during which the correct pattern is shown will be decremented in CheckPattern by the VIEWTIME_DECREMENT
        // constant with each new pattern.
        await Task.Delay(_viewTimeValue);

        ErasePattern();
        EnableAllColumns();
        _pauseTimer = false;
        ProgressTimer();
    }
    // After "Submit" is clicked, CheckPattern will determine whether or not the buttons that the player chose are the correct ones
    public async void CheckPattern(object sender, EventArgs e)
    {
        // This code will only execute if the player has selected one button from each column.
        if (_disabledColumns.Contains(false) == false)
        {
            bool patternIsCorrect = true;

            for (int i = 0; i < Constants.NUM_COLUMNS; i++)
            {
                if (_chosenIndexes[i] != _correctIndexes[i])
                {
                    patternIsCorrect = false;
                    break;
                }
            }

            // If the player's pattern is correct, they earn a point, but if it is incorrect, they lose a try. A new pattern will be created
            // afterwards, regardless of whether or not the player is correct.
            if (patternIsCorrect == true)
            {
                _gameScore += 1;
                lblScore.Text = $"Score: {_gameScore}";

                if (_viewTimeValue >= Constants.MIN_VIEWTIME)
                    _viewTimeValue -= Constants.VIEWTIME_DECREMENT;

                ScoreAnimation();
                CorrectGreen();
                await Task.Delay(1000);
                CreateNewPattern();
            }
            else
            {
                _remainingTries -= 1;
                lblTries.Text = $"Tries: {_remainingTries}";

                if (_viewTimeValue >= Constants.MIN_VIEWTIME)
                    _viewTimeValue -= Constants.VIEWTIME_DECREMENT;

                TriesAnimation();
                IncorrectRed();
                await Task.Delay(1000);
                CreateNewPattern();
            }
        }
    }
    public void ErasePattern()
    {
        btn1x1.BackgroundColor = Color.Parse("Transparent");
        btn2x1.BackgroundColor = Color.Parse("Transparent");
        btn3x1.BackgroundColor = Color.Parse("Transparent");
        btn4x1.BackgroundColor = Color.Parse("Transparent");
        btn5x1.BackgroundColor = Color.Parse("Transparent");
        btn6x1.BackgroundColor = Color.Parse("Transparent");
        btn7x1.BackgroundColor = Color.Parse("Transparent");
        btn8x1.BackgroundColor = Color.Parse("Transparent");
        btn1x2.BackgroundColor = Color.Parse("Transparent");
        btn2x2.BackgroundColor = Color.Parse("Transparent");
        btn3x2.BackgroundColor = Color.Parse("Transparent");
        btn4x2.BackgroundColor = Color.Parse("Transparent");
        btn5x2.BackgroundColor = Color.Parse("Transparent");
        btn6x2.BackgroundColor = Color.Parse("Transparent");
        btn7x2.BackgroundColor = Color.Parse("Transparent");
        btn8x2.BackgroundColor = Color.Parse("Transparent");
        btn1x3.BackgroundColor = Color.Parse("Transparent");
        btn2x3.BackgroundColor = Color.Parse("Transparent");
        btn3x3.BackgroundColor = Color.Parse("Transparent");
        btn4x3.BackgroundColor = Color.Parse("Transparent");
        btn5x3.BackgroundColor = Color.Parse("Transparent");
        btn6x3.BackgroundColor = Color.Parse("Transparent");
        btn7x3.BackgroundColor = Color.Parse("Transparent");
        btn8x3.BackgroundColor = Color.Parse("Transparent");
        btn1x4.BackgroundColor = Color.Parse("Transparent");
        btn2x4.BackgroundColor = Color.Parse("Transparent");
        btn3x4.BackgroundColor = Color.Parse("Transparent");
        btn4x4.BackgroundColor = Color.Parse("Transparent");
        btn5x4.BackgroundColor = Color.Parse("Transparent");
        btn6x4.BackgroundColor = Color.Parse("Transparent");
        btn7x4.BackgroundColor = Color.Parse("Transparent");
        btn8x4.BackgroundColor = Color.Parse("Transparent");
        btn1x5.BackgroundColor = Color.Parse("Transparent");
        btn2x5.BackgroundColor = Color.Parse("Transparent");
        btn3x5.BackgroundColor = Color.Parse("Transparent");
        btn4x5.BackgroundColor = Color.Parse("Transparent");
        btn5x5.BackgroundColor = Color.Parse("Transparent");
        btn6x5.BackgroundColor = Color.Parse("Transparent");
        btn7x5.BackgroundColor = Color.Parse("Transparent");
        btn8x5.BackgroundColor = Color.Parse("Transparent");
        btn1x6.BackgroundColor = Color.Parse("Transparent");
        btn2x6.BackgroundColor = Color.Parse("Transparent");
        btn3x6.BackgroundColor = Color.Parse("Transparent");
        btn4x6.BackgroundColor = Color.Parse("Transparent");
        btn5x6.BackgroundColor = Color.Parse("Transparent");
        btn6x6.BackgroundColor = Color.Parse("Transparent");
        btn7x6.BackgroundColor = Color.Parse("Transparent");
        btn8x6.BackgroundColor = Color.Parse("Transparent");
    }
    public void IncorrectRed()
    {
        _pauseTimer = true;

        btn1x1.BackgroundColor = Color.Parse("Red");
        btn2x1.BackgroundColor = Color.Parse("Red");
        btn3x1.BackgroundColor = Color.Parse("Red");
        btn4x1.BackgroundColor = Color.Parse("Red");
        btn5x1.BackgroundColor = Color.Parse("Red");
        btn6x1.BackgroundColor = Color.Parse("Red");
        btn7x1.BackgroundColor = Color.Parse("Red");
        btn8x1.BackgroundColor = Color.Parse("Red");
        btn1x2.BackgroundColor = Color.Parse("Red");
        btn2x2.BackgroundColor = Color.Parse("Red");
        btn3x2.BackgroundColor = Color.Parse("Red");
        btn4x2.BackgroundColor = Color.Parse("Red");
        btn5x2.BackgroundColor = Color.Parse("Red");
        btn6x2.BackgroundColor = Color.Parse("Red");
        btn7x2.BackgroundColor = Color.Parse("Red");
        btn8x2.BackgroundColor = Color.Parse("Red");
        btn1x3.BackgroundColor = Color.Parse("Red");
        btn2x3.BackgroundColor = Color.Parse("Red");
        btn3x3.BackgroundColor = Color.Parse("Red");
        btn4x3.BackgroundColor = Color.Parse("Red");
        btn5x3.BackgroundColor = Color.Parse("Red");
        btn6x3.BackgroundColor = Color.Parse("Red");
        btn7x3.BackgroundColor = Color.Parse("Red");
        btn8x3.BackgroundColor = Color.Parse("Red");
        btn1x4.BackgroundColor = Color.Parse("Red");
        btn2x4.BackgroundColor = Color.Parse("Red");
        btn3x4.BackgroundColor = Color.Parse("Red");
        btn4x4.BackgroundColor = Color.Parse("Red");
        btn5x4.BackgroundColor = Color.Parse("Red");
        btn6x4.BackgroundColor = Color.Parse("Red");
        btn7x4.BackgroundColor = Color.Parse("Red");
        btn8x4.BackgroundColor = Color.Parse("Red");
        btn1x5.BackgroundColor = Color.Parse("Red");
        btn2x5.BackgroundColor = Color.Parse("Red");
        btn3x5.BackgroundColor = Color.Parse("Red");
        btn4x5.BackgroundColor = Color.Parse("Red");
        btn5x5.BackgroundColor = Color.Parse("Red");
        btn6x5.BackgroundColor = Color.Parse("Red");
        btn7x5.BackgroundColor = Color.Parse("Red");
        btn8x5.BackgroundColor = Color.Parse("Red");
        btn1x6.BackgroundColor = Color.Parse("Red");
        btn2x6.BackgroundColor = Color.Parse("Red");
        btn3x6.BackgroundColor = Color.Parse("Red");
        btn4x6.BackgroundColor = Color.Parse("Red");
        btn5x6.BackgroundColor = Color.Parse("Red");
        btn6x6.BackgroundColor = Color.Parse("Red");
        btn7x6.BackgroundColor = Color.Parse("Red");
        btn8x6.BackgroundColor = Color.Parse("Red");
    }
    public void CorrectGreen()
    {
        _pauseTimer = true;

        btn1x1.BackgroundColor = Color.Parse("Green");
        btn2x1.BackgroundColor = Color.Parse("Green");
        btn3x1.BackgroundColor = Color.Parse("Green");
        btn4x1.BackgroundColor = Color.Parse("Green");
        btn5x1.BackgroundColor = Color.Parse("Green");
        btn6x1.BackgroundColor = Color.Parse("Green");
        btn7x1.BackgroundColor = Color.Parse("Green");
        btn8x1.BackgroundColor = Color.Parse("Green");
        btn1x2.BackgroundColor = Color.Parse("Green");
        btn2x2.BackgroundColor = Color.Parse("Green");
        btn3x2.BackgroundColor = Color.Parse("Green");
        btn4x2.BackgroundColor = Color.Parse("Green");
        btn5x2.BackgroundColor = Color.Parse("Green");
        btn6x2.BackgroundColor = Color.Parse("Green");
        btn7x2.BackgroundColor = Color.Parse("Green");
        btn8x2.BackgroundColor = Color.Parse("Green");
        btn1x3.BackgroundColor = Color.Parse("Green");
        btn2x3.BackgroundColor = Color.Parse("Green");
        btn3x3.BackgroundColor = Color.Parse("Green");
        btn4x3.BackgroundColor = Color.Parse("Green");
        btn5x3.BackgroundColor = Color.Parse("Green");
        btn6x3.BackgroundColor = Color.Parse("Green");
        btn7x3.BackgroundColor = Color.Parse("Green");
        btn8x3.BackgroundColor = Color.Parse("Green");
        btn1x4.BackgroundColor = Color.Parse("Green");
        btn2x4.BackgroundColor = Color.Parse("Green");
        btn3x4.BackgroundColor = Color.Parse("Green");
        btn4x4.BackgroundColor = Color.Parse("Green");
        btn5x4.BackgroundColor = Color.Parse("Green");
        btn6x4.BackgroundColor = Color.Parse("Green");
        btn7x4.BackgroundColor = Color.Parse("Green");
        btn8x4.BackgroundColor = Color.Parse("Green");
        btn1x5.BackgroundColor = Color.Parse("Green");
        btn2x5.BackgroundColor = Color.Parse("Green");
        btn3x5.BackgroundColor = Color.Parse("Green");
        btn4x5.BackgroundColor = Color.Parse("Green");
        btn5x5.BackgroundColor = Color.Parse("Green");
        btn6x5.BackgroundColor = Color.Parse("Green");
        btn7x5.BackgroundColor = Color.Parse("Green");
        btn8x5.BackgroundColor = Color.Parse("Green");
        btn1x6.BackgroundColor = Color.Parse("Green");
        btn2x6.BackgroundColor = Color.Parse("Green");
        btn3x6.BackgroundColor = Color.Parse("Green");
        btn4x6.BackgroundColor = Color.Parse("Green");
        btn5x6.BackgroundColor = Color.Parse("Green");
        btn6x6.BackgroundColor = Color.Parse("Green");
        btn7x6.BackgroundColor = Color.Parse("Green");
        btn8x6.BackgroundColor = Color.Parse("Green");
    }
    public void GameOver()
    {
        table.IsVisible = false;
        btn1x1.IsVisible = false;
        btn2x1.IsVisible = false;
        btn3x1.IsVisible = false;
        btn4x1.IsVisible = false;
        btn5x1.IsVisible = false;
        btn6x1.IsVisible = false;
        btn7x1.IsVisible = false;
        btn8x1.IsVisible = false;
        btn2x5.IsVisible = false;
        btn1x2.IsVisible = false;
        btn2x2.IsVisible = false;
        btn3x2.IsVisible = false;
        btn4x2.IsVisible = false;
        btn5x2.IsVisible = false;
        btn6x2.IsVisible = false;
        btn7x2.IsVisible = false;
        btn8x2.IsVisible = false;
        btn1x3.IsVisible = false;
        btn2x3.IsVisible = false;
        btn3x3.IsVisible = false;
        btn4x3.IsVisible = false;
        btn5x3.IsVisible = false;
        btn6x3.IsVisible = false;
        btn7x3.IsVisible = false;
        btn8x3.IsVisible = false;
        btn1x4.IsVisible = false;
        btn2x4.IsVisible = false;
        btn3x4.IsVisible = false;
        btn4x4.IsVisible = false;
        btn5x4.IsVisible = false;
        btn6x4.IsVisible = false;
        btn7x4.IsVisible = false;
        btn8x4.IsVisible = false;
        btn1x5.IsVisible = false;
        btn2x5.IsVisible = false;
        btn3x5.IsVisible = false;
        btn4x5.IsVisible = false;
        btn5x5.IsVisible = false;
        btn6x5.IsVisible = false;
        btn7x5.IsVisible = false;
        btn8x5.IsVisible = false;
        btn1x6.IsVisible = false;
        btn2x6.IsVisible = false;
        btn3x6.IsVisible = false;
        btn4x6.IsVisible = false;
        btn5x6.IsVisible = false;
        btn6x6.IsVisible = false;
        btn7x6.IsVisible = false;
        btn8x6.IsVisible = false;
        btnSubmit.IsVisible = false;

        finalGrid.IsVisible = true;
        lblFinalScore.IsVisible = true;
        btnMainMenu.IsEnabled = true;
        btnMainMenu.IsVisible = true;

        lblFinalScore.Text = $"Final Score: {_gameScore}";
    }

    // The column array is used to determine which button in each column will be a part of the correct pattern
    public void ShowCorrectButtonCol1()
    {
        int correctIndex = 0;

        for (int i = 0; i < Constants.NUM_ROWS; i++)
        {
            if (_columnArray1[i] == 1)
            {
                correctIndex = i;
                break;
            }
        }

        switch (correctIndex)
        {
            case 0:
                btn1x1.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 1:
                btn2x1.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 2:
                btn3x1.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 3:
                btn4x1.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 4:
                btn5x1.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 5:
                btn6x1.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 6:
                btn7x1.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 7:
                btn8x1.BackgroundColor = Color.Parse("#FFD700");
                break;
            default:
                break;
        }
    }
    public void ShowCorrectButtonCol2()
    {
        int correctIndex = 0;

        for (int i = 0; i < Constants.NUM_ROWS; i++)
        {
            if (_columnArray2[i] == 1)
            {
                correctIndex = i;
                break;
            }
        }

        switch (correctIndex)
        {
            case 0:
                btn1x2.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 1:
                btn2x2.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 2:
                btn3x2.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 3:
                btn4x2.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 4:
                btn5x2.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 5:
                btn6x2.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 6:
                btn7x2.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 7:
                btn8x2.BackgroundColor = Color.Parse("#FFD700");
                break;
            default:
                break;
        }
    }
    public void ShowCorrectButtonCol3()
    {
        int correctIndex = 0;

        for (int i = 0; i < Constants.NUM_ROWS; i++)
        {
            if (_columnArray3[i] == 1)
            {
                correctIndex = i;
                break;
            }
        }

        switch (correctIndex)
        {
            case 0:
                btn1x3.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 1:
                btn2x3.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 2:
                btn3x3.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 3:
                btn4x3.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 4:
                btn5x3.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 5:
                btn6x3.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 6:
                btn7x3.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 7:
                btn8x3.BackgroundColor = Color.Parse("#FFD700");
                break;
            default:
                break;
        }
    }
    public void ShowCorrectButtonCol4()
    {
        int correctIndex = 0;

        for (int i = 0; i < Constants.NUM_ROWS; i++)
        {
            if (_columnArray4[i] == 1)
            {
                correctIndex = i;
                break;
            }
        }

        switch (correctIndex)
        {
            case 0:
                btn1x4.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 1:
                btn2x4.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 2:
                btn3x4.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 3:
                btn4x4.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 4:
                btn5x4.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 5:
                btn6x4.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 6:
                btn7x4.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 7:
                btn8x4.BackgroundColor = Color.Parse("#FFD700");
                break;

            default:
                break;
        }
    }
    public void ShowCorrectButtonCol5()
    {
        int correctIndex = 0;

        for (int i = 0; i < Constants.NUM_ROWS; i++)
        {
            if (_columnArray5[i] == 1)
            {
                correctIndex = i;
                break;
            }
        }

        switch (correctIndex)
        {
            case 0:
                btn1x5.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 1:
                btn2x5.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 2:
                btn3x5.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 3:
                btn4x5.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 4:
                btn5x5.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 5:
                btn6x5.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 6:
                btn7x5.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 7:
                btn8x5.BackgroundColor = Color.Parse("#FFD700");
                break;
            default:
                break;
        }
    }
    public void ShowCorrectButtonCol6()
    {
        int correctIndex = 0;

        for (int i = 0; i < Constants.NUM_ROWS; i++)
        {
            if (_columnArray6[i] == 1)
            {
                correctIndex = i;
                break;
            }
        }

        switch (correctIndex)
        {
            case 0:
                btn1x6.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 1:
                btn2x6.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 2:
                btn3x6.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 3:
                btn4x6.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 4:
                btn5x6.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 5:
                btn6x6.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 6:
                btn7x6.BackgroundColor = Color.Parse("#FFD700");
                break;
            case 7:
                btn8x6.BackgroundColor = Color.Parse("#FFD700");
                break;
            default:
                break;
        }
    }

    public void DisableColumn1()
    {
        btn1x1.IsEnabled = false;
        btn2x1.IsEnabled = false;
        btn3x1.IsEnabled = false;
        btn4x1.IsEnabled = false;
        btn5x1.IsEnabled = false;
        btn6x1.IsEnabled = false;
        btn7x1.IsEnabled = false;
        btn8x1.IsEnabled = false;
        _disabledColumns[0] = true;
    }
    public void DisableColumn2()
    {
        btn1x2.IsEnabled = false;
        btn2x2.IsEnabled = false;
        btn3x2.IsEnabled = false;
        btn4x2.IsEnabled = false;
        btn5x2.IsEnabled = false;
        btn6x2.IsEnabled = false;
        btn7x2.IsEnabled = false;
        btn8x2.IsEnabled = false;
        _disabledColumns[1] = true;
    }
    public void DisableColumn3()
    {
        btn1x3.IsEnabled = false;
        btn2x3.IsEnabled = false;
        btn3x3.IsEnabled = false;
        btn4x3.IsEnabled = false;
        btn5x3.IsEnabled = false;
        btn6x3.IsEnabled = false;
        btn7x3.IsEnabled = false;
        btn8x3.IsEnabled = false;
        _disabledColumns[2] = true;
    }
    public void DisableColumn4()
    {
        btn1x4.IsEnabled = false;
        btn2x4.IsEnabled = false;
        btn3x4.IsEnabled = false;
        btn4x4.IsEnabled = false;
        btn5x4.IsEnabled = false;
        btn6x4.IsEnabled = false;
        btn7x4.IsEnabled = false;
        btn8x4.IsEnabled = false;
        _disabledColumns[3] = true;
    }
    public void DisableColumn5()
    {
        btn1x5.IsEnabled = false;
        btn2x5.IsEnabled = false;
        btn3x5.IsEnabled = false;
        btn4x5.IsEnabled = false;
        btn5x5.IsEnabled = false;
        btn6x5.IsEnabled = false;
        btn7x5.IsEnabled = false;
        btn8x5.IsEnabled = false;
        _disabledColumns[4] = true;
    }
    public void DisableColumn6()
    {
        btn1x6.IsEnabled = false;
        btn2x6.IsEnabled = false;
        btn3x6.IsEnabled = false;
        btn4x6.IsEnabled = false;
        btn5x6.IsEnabled = false;
        btn6x6.IsEnabled = false;
        btn7x6.IsEnabled = false;
        btn8x6.IsEnabled = false;
        _disabledColumns[5] = true;
    }
    public void EnableAllColumns()
    {
        for (int i = 0; i < Constants.NUM_COLUMNS; i++)
            _disabledColumns[i] = false;

        btn1x1.IsEnabled = true;
        btn2x1.IsEnabled = true;
        btn3x1.IsEnabled = true;
        btn4x1.IsEnabled = true;
        btn5x1.IsEnabled = true;
        btn6x1.IsEnabled = true;
        btn7x1.IsEnabled = true;
        btn8x1.IsEnabled = true;
        btn1x2.IsEnabled = true;
        btn2x2.IsEnabled = true;
        btn3x2.IsEnabled = true;
        btn4x2.IsEnabled = true;
        btn5x2.IsEnabled = true;
        btn6x2.IsEnabled = true;
        btn7x2.IsEnabled = true;
        btn8x2.IsEnabled = true;
        btn1x3.IsEnabled = true;
        btn2x3.IsEnabled = true;
        btn3x3.IsEnabled = true;
        btn4x3.IsEnabled = true;
        btn1x4.IsEnabled = true;
        btn2x4.IsEnabled = true;
        btn3x4.IsEnabled = true;
        btn4x4.IsEnabled = true;
        btn5x4.IsEnabled = true;
        btn6x4.IsEnabled = true;
        btn7x4.IsEnabled = true;
        btn8x4.IsEnabled = true;
        btn1x5.IsEnabled = true;
        btn2x5.IsEnabled = true;
        btn3x5.IsEnabled = true;
        btn4x5.IsEnabled = true;
        btn5x5.IsEnabled = true;
        btn6x5.IsEnabled = true;
        btn7x5.IsEnabled = true;
        btn8x5.IsEnabled = true;
        btn1x6.IsEnabled = true;
        btn2x6.IsEnabled = true;
        btn3x6.IsEnabled = true;
        btn4x6.IsEnabled = true;
        btn5x6.IsEnabled = true;
        btn6x6.IsEnabled = true;
        btn7x6.IsEnabled = true;
        btn8x6.IsEnabled = true;
    }

    public void DisableAllColumns()
    {
        for (int i = 0; i < Constants.NUM_COLUMNS; i++)
            _disabledColumns[i] = true;

        btn1x1.IsEnabled = false;
        btn2x1.IsEnabled = false;
        btn3x1.IsEnabled = false;
        btn4x1.IsEnabled = false;
        btn5x1.IsEnabled = false;
        btn6x1.IsEnabled = false;
        btn7x1.IsEnabled = false;
        btn8x1.IsEnabled = false;
        btn1x2.IsEnabled = false;
        btn2x2.IsEnabled = false;
        btn3x2.IsEnabled = false;
        btn4x2.IsEnabled = false;
        btn5x2.IsEnabled = false;
        btn6x2.IsEnabled = false;
        btn7x2.IsEnabled = false;
        btn8x2.IsEnabled = false;
        btn1x3.IsEnabled = false;
        btn2x3.IsEnabled = false;
        btn3x3.IsEnabled = false;
        btn4x3.IsEnabled = false;
        btn5x3.IsEnabled = false;
        btn6x3.IsEnabled = false;
        btn7x3.IsEnabled = false;
        btn8x3.IsEnabled = false;
        btn1x4.IsEnabled = false;
        btn2x4.IsEnabled = false;
        btn3x4.IsEnabled = false;
        btn4x4.IsEnabled = false;
        btn5x4.IsEnabled = false;
        btn6x4.IsEnabled = false;
        btn7x4.IsEnabled = false;
        btn8x4.IsEnabled = false;
        btn1x5.IsEnabled = false;
        btn2x5.IsEnabled = false;
        btn3x5.IsEnabled = false;
        btn4x5.IsEnabled = false;
        btn5x5.IsEnabled = false;
        btn6x5.IsEnabled = false;
        btn7x5.IsEnabled = false;
        btn8x5.IsEnabled = false;
        btn1x6.IsEnabled = false;
        btn2x6.IsEnabled = false;
        btn3x6.IsEnabled = false;
        btn4x6.IsEnabled = false;
        btn5x6.IsEnabled = false;
        btn6x6.IsEnabled = false;
        btn7x6.IsEnabled = false;
        btn8x6.IsEnabled = false;
    }

    // These ButtonClicked events will assign a value between 1 and 5 to an array (_chosenIndexes) that stores the indexes of the
    // buttons that they chose. _chosenIndexes will be compared to _correctIndexes in the CheckPattern method.
    public void Btn1x1Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[0] = 1;
        btn1x1.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn1();
    }
    public void Btn2x1Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[0] = 2;
        btn2x1.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn1();
    }
    public void Btn3x1Clicked(Object sender, EventArgs e)
    {
        _chosenIndexes[0] = 3;
        btn3x1.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn1();
    }
    public void Btn4x1Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[0] = 4;
        btn4x1.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn1();
    }

    public void Btn5x1Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[0] = 5;
        btn4x1.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn1();
    }
    public void Btn6x1Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[0] = 6;
        btn4x1.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn1();
    }
    public void Btn7x1Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[0] = 7;
        btn4x1.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn1();
    }
    public void Btn8x1Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[0] = 8;
        btn4x1.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn1();
    }
    public void Btn1x2Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[1] = 1;
        btn1x2.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn2();
    }
    public void Btn2x2Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[1] = 2;
        btn2x2.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn2();
    }
    public void Btn3x2Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[1] = 3;
        btn3x2.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn2();
    }
    public void Btn4x2Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[1] = 4;
        btn4x2.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn2();
    }
    public void Btn5x2Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[1] = 5;
        btn4x2.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn2();
    }
    public void Btn6x2Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[1] = 6;
        btn4x2.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn2();
    }
    public void Btn7x2Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[1] = 7;
        btn4x2.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn2();
    }
    public void Btn8x2Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[1] = 8;
        btn4x2.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn2();
    }
    public void Btn1x3Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[2] = 1;
        btn1x3.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn3();
    }
    public void Btn2x3Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[2] = 2;
        btn2x3.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn3();
    }
    public void Btn3x3Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[2] = 3;
        btn3x3.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn3();
    }
    public void Btn4x3Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[2] = 4;
        btn4x3.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn3();
    }
    public void Btn5x3Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[2] = 5;
        btn4x3.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn3();
    }
    public void Btn6x3Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[2] = 6;
        btn4x3.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn3();
    }
    public void Btn7x3Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[2] = 7;
        btn4x3.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn3();
    }
    public void Btn8x3Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[2] = 8;
        btn4x3.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn3();
    }
    public void Btn1x4Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[3] = 1;
        btn1x4.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn4();
    }
    public void Btn2x4Clicked(Object sender, EventArgs e)
    {
        _chosenIndexes[3] = 2;
        btn2x4.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn4();
    }
    public void Btn3x4Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[3] = 3;
        btn3x4.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn4();
    }
    public void Btn4x4Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[3] = 4;
        btn4x4.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn4();
    }
    public void Btn5x4Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[3] = 5;
        btn4x4.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn4();
    }
    public void Btn6x4Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[3] = 6;
        btn4x4.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn4();
    }
    public void Btn7x4Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[3] = 7;
        btn4x4.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn4();
    }
    public void Btn8x4Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[3] = 8;
        btn4x4.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn4();
    }
    public void Btn1x5Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[4] = 1;
        btn1x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn5();
    }
    public void Btn2x5Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[4] = 2;
        btn2x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn5();
    }
    public void Btn3x5Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[4] = 3;
        btn3x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn5();
    }
    public void Btn4x5Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[4] = 4;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn5();
    }
    public void Btn5x5Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[4] = 5;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn5();
    }
    public void Btn6x5Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[4] = 6;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn5();
    }
    public void Btn7x5Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[4] = 7;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn5();
    }
    public void Btn8x5Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[4] = 8;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn5();
    }
    public void Btn1x6Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[5] = 1;
        btn1x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn6();
    }
    public void Btn2x6Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[5] = 2;
        btn2x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn6();
    }
    public void Btn3x6Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[5] = 3;
        btn3x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn6();
    }
    public void Btn4x6Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[5] = 4;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn6();
    }
    public void Btn5x6Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[5] = 5;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn6();
    }
    public void Btn6x6Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[5] = 6;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn6();
    }
    public void Btn7x6Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[5] = 7;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn6();
    }
    public void Btn8x6Clicked(object sender, EventArgs e)
    {
        _chosenIndexes[5] = 8;
        btn4x5.BackgroundColor = Color.Parse("#FFD700");
        DisableColumn6();
    }

    // The player's final score is saved into PlayerDatabase.
    public async void ReturnToMainMenu(object sender, EventArgs e)
    {
        App.CurrentScore = _gameScore;
        _save.SaveEasyPlayerData();

        _save = null;
        await Navigation.PopToRootAsync();
    }
}