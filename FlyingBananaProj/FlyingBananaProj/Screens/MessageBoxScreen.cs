using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class MessageBoxScreen : MenuScreen
    {
        #region Class Variables

        string message;
        PlayerIndex playerIndex;

        #endregion

        #region Events

        public event EventHandler<PlayerIndexEventArgs> Accepted;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message)
            : this(message, true)
        { }


        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public MessageBoxScreen(string message, bool includeUsageText):base(message)
        {
            this.message = message;

            // Create our menu entries.
            MenuEntry yesMenuEntry = new MenuEntry("Yes");
            MenuEntry noMenuEntry = new MenuEntry("No");

            // Hook up menu event handlers.
            yesMenuEntry.Selected += YesMenuEntrySelected;
            noMenuEntry.Selected += OnCancel;

            // Add entries to the menu.
            MenuEntries.Add(yesMenuEntry);
            MenuEntries.Add(noMenuEntry);


            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }

        #endregion

        #region Handle Input

        public void YesMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            // Raise the accepted event, then exit the message box.
            Accepted(this, new PlayerIndexEventArgs(playerIndex));
            ExitScreen();
        }

        #endregion

        #region Draw


        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }


        #endregion
    }
}
