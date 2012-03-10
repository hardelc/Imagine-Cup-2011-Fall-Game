using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace FlyingBananaProj
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {
        #region Initialization
        string[] messages = { "Why do you make Rick cry?",
        "But Team Omega needs You!", "...But we have cake!",
        "Millions of cells count on you!"};
        Random rand = new Random();
        AudioVideoController sounds = new AudioVideoController();
        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("")
        {

            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry controlsMenuEntry = new MenuEntry("Controls");
            MenuEntry helpMenuEntry = new MenuEntry("How to Play");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            creditsMenuEntry.Selected += CreditsMenuEntrySelected;
            controlsMenuEntry.Selected += ControlsMenuEntrySelected;
            helpMenuEntry.Selected += helpMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(optionsMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(controlsMenuEntry);
            MenuEntries.Add(helpMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        public override void LoadContent()
        {
            base.LoadContent();
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            MediaPlayer.Stop();
            sounds.playSoundEffect("playgame", 1);
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,new GameplayScreen());
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            sounds.playSoundEffect("select", 1);
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            sounds.playSoundEffect("select", 1);
            ScreenManager.AddScreen(new CreditsMenuScreen(), e.PlayerIndex);
        }
        void ControlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            sounds.playSoundEffect("select", 1);
            ScreenManager.AddScreen(new ControlsMenuScreen(), e.PlayerIndex);
        }
        void helpMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            sounds.playSoundEffect("select", 1);
            ScreenManager.AddScreen(new HelpMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            string message = "Do you really wish to quit? \n" + messages[rand.Next(messages.Length)];

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
        #endregion
    }
}
