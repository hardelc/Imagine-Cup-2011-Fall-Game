namespace FlyingBananaProj
{
/// <summary>
/// The options screen is brought up over the top of the main menu
/// screen, and gives the user a chance to configure the game
/// in various hopefully useful ways.
/// </summary>
class OptionsMenuScreen : MenuScreen
{
    #region Class Variables

    MenuEntry difficultyMenuEntry;
    AudioVideoController sounds = new AudioVideoController();
#if WINDOWS
    MenuEntry fullscreenMenuEntry;
#endif


    static string[] Difficulty = { "Beginner", "Casual", "Hard" };
    static int currentDifficulty = 0;

    static bool fullscreen = true;

    #endregion

    #region Initialization


    /// <summary>
    /// Constructor.
    /// </summary>
    public OptionsMenuScreen()
    : base("Options")
    {
        // Create our menu entries.
        difficultyMenuEntry = new MenuEntry(string.Empty);
#if WINDOWS
        fullscreenMenuEntry = new MenuEntry(string.Empty);
#endif

        SetMenuEntryText();

        MenuEntry back = new MenuEntry("Back");

        // Hook up menu event handlers.
        difficultyMenuEntry.Selected += DifficultyMenuEntrySelected;
#if WINDOWS
        fullscreenMenuEntry.Selected += fullscreenMenuEntrySelected;
#endif

        back.Selected += OnCancel;

        // Add entries to the menu.
        MenuEntries.Add(difficultyMenuEntry);
#if WINDOWS
        MenuEntries.Add(fullscreenMenuEntry);
#endif

        MenuEntries.Add(back);
    }


    /// <summary>
    /// Fills in the latest values for the options screen menu text.
    /// </summary>
    void SetMenuEntryText()
    {
        difficultyMenuEntry.Text = "Difficulty: " + Difficulty[currentDifficulty];
#if WINDOWS
        fullscreenMenuEntry.Text = "Fullscreen: " + (fullscreen ? "on" : "off");
#endif

    }


    #endregion

    #region Handle Input

    /// <summary>
    /// Event handler for when the Language menu entry is selected.
    /// </summary>
    void DifficultyMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        sounds.playSoundEffect("select", 1);
        currentDifficulty = (currentDifficulty + 1) % Difficulty.Length;

        SetMenuEntryText();
    }


    /// <summary>
    /// Event handler for when the Frobnicate menu entry is selected.
    /// </summary>
    void fullscreenMenuEntrySelected(object sender, PlayerIndexEventArgs e)
    {
        sounds.playSoundEffect("select", 1);
        fullscreen = !fullscreen;
        Game1.changeFullScreen();
        SetMenuEntryText();

    }

    #endregion
    }
}

