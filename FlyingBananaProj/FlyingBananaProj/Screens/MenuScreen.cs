using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    /// <summary>
    /// Base class for screens that contain a menu of options. The user can
    /// move up and down to select an entry, or cancel to back out of the screen.
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Class Variables

        AudioVideoController sounds = new AudioVideoController();
        List<MenuEntry> menuEntries = new List<MenuEntry>();
        int selectedEntry = 0;
        string menuTitle;
        protected ContentManager content;
        Texture2D gradientTexture;
        #endregion

        #region Properties


        /// <summary>
        /// Gets the list of menu entries, so derived classes can add
        /// or change the menu contents.
        /// </summary>
        public MenuScreen()
        {
            
        }

        public ContentManager Content
        {
            get { return content; }
        }

        public IList<MenuEntry> MenuEntries
        {
            get { return menuEntries; }
        }


        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen(string menuTitle)
        {
            this.menuTitle = menuTitle;

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            gradientTexture = content.Load<Texture2D>("ui/gradient");
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            // Move to the previous menu entry?
            if (input.IsMenuUp(ControllingPlayer))
            {
                sounds.playSoundEffect("move", 1);
                selectedEntry = (selectedEntry - 1 + menuEntries.Count) % menuEntries.Count; //Modulus FTW.
            }

            // Move to the next menu entry?
            if (input.IsMenuDown(ControllingPlayer))
            {
                sounds.playSoundEffect("move", 1);
                selectedEntry = (selectedEntry + 1) % menuEntries.Count;
            }

            // Accept or cancel the menu? We pass in our ControllingPlayer, which may
            // either be null (to accept input from any player) or a specific index.
            // If we pass a null controlling player, the InputState helper returns to
            // us which player actually provided the input. We pass that through to
            // OnSelectEntry and OnCancel, so they can tell which player triggered them.
            PlayerIndex playerIndex;

            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                OnSelectEntry(selectedEntry, playerIndex);
            }
            else if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                sounds.playSoundEffect("cancel", 1);
                OnCancel(playerIndex);
            }
        }


        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {     
            menuEntries[entryIndex].OnSelectEntry(playerIndex);
        }


        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel(PlayerIndex playerIndex)
        {
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, PlayerIndexEventArgs e)
        {
            sounds.playSoundEffect("cancel", 1);
            OnCancel(e.PlayerIndex);
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected virtual void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, 250f);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                //while(position != )
                //{
                    // each entry is to be centered horizontally
                    position.X = 70;

                    if (ScreenState == ScreenState.TransitionOn)
                        position.X -= transitionOffset * 256;
                    else
                        position.X += transitionOffset * 512;

                    // set the entry's position
                    menuEntry.Position = position;

                    // move down for the next entry the size of this entry
                    position.Y += menuEntry.GetHeight(this) + 25;
                //}
            }
        }
        protected virtual void UpdateMessageBoxMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, 350f);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                // each entry is to be centered horizontally
                position.X = ScreenManager.GraphicsDevice.Viewport.Width / 2 - menuEntry.GetWidth(this) / 2;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this) + 10;
            }
        }


        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == selectedEntry);

                menuEntries[i].Update(this, isSelected, gameTime);
            }
        }


        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            if (this is MessageBoxScreen)
            {
                // make sure our entries are in the right place before we draw them
                UpdateMessageBoxMenuEntryLocations();

                // Darken down any other screens that were drawn beneath the popup.
                ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 7 / 10);

                // Center the message text in the viewport.
                Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
                Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
                Vector2 textSize = new Vector2 (gradientTexture.Width, gradientTexture.Height);
                Vector2 textPosition = textSize/6;

                // The background includes a border somewhat larger than the text itself.
                const int hPad = 320;
                const int vPad = 140;

                Rectangle backgroundRectangle = new Rectangle((int)textPosition.X - hPad,
                                                              (int)textPosition.Y - vPad,
                                                              (int)(textSize.X*1.1),
                                                              (int)(textSize.Y*1.1));

                // Fade the popup alpha during transitions.
                Color color = Color.White * TransitionAlpha;

                GraphicsDevice graphics = ScreenManager.GraphicsDevice;

                spriteBatch.Begin();

                // Draw the background rectangle.
                spriteBatch.Draw(gradientTexture, backgroundRectangle, color);

                // Draw each menu entry in turn.
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = menuEntries[i];

                    bool isSelected = IsActive && (i == selectedEntry);

                    menuEntry.Draw(this, isSelected, gameTime);
                }
                // Make the menu slide into place during transitions, using a
                // power curve to make things look more interesting (this makes
                // the movement slow down as it nears the end).
                float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

                // Draw the menu title centered on the screen
                Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 250f);
                Vector2 titlePosition2 = new Vector2((graphics.Viewport.Width / 2)-1, 249f);
                Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
                float titleScale = 1.25f;

               // titlePosition.Y -= transitionOffset * 100;
                spriteBatch.DrawString(font, menuTitle, titlePosition, Color.Black, 0,
                                       titleOrigin, titleScale, SpriteEffects.None, 0);
                spriteBatch.DrawString(font, menuTitle, titlePosition2, Color.Violet, 0,
                                       titleOrigin, titleScale, SpriteEffects.None, 0);
                spriteBatch.End();
            }
            else
            {
                // make sure our entries are in the right place before we draw them
                UpdateMenuEntryLocations();

                if (!(this is MainMenuScreen))
                {
                    // Darken down any other screens that were drawn beneath the popup.
                    ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 4 / 5);
                }

                GraphicsDevice graphics = ScreenManager.GraphicsDevice;

                spriteBatch.Begin();
                // Draw each menu entry in turn.
                for (int i = 0; i < menuEntries.Count; i++)
                {
                    MenuEntry menuEntry = menuEntries[i];

                    bool isSelected = IsActive && (i == selectedEntry);

                    menuEntry.Draw(this, isSelected, gameTime);
                }
                // Make the menu slide into place during transitions, using a
                // power curve to make things look more interesting (this makes
                // the movement slow down as it nears the end).
                float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

                // Draw the menu title centered on the screen
                Vector2 titlePosition = new Vector2(graphics.Viewport.Width / 2, 80);
                Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
                Color titleColor = new Color(192, 192, 192) * TransitionAlpha;
                float titleScale = 1.25f;

                titlePosition.Y -= transitionOffset * 100;

                spriteBatch.DrawString(font, menuTitle, titlePosition, titleColor, 0,
                                       titleOrigin, titleScale, SpriteEffects.None, 0);

                spriteBatch.End();

                ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
                ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                ScreenManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            }
        }


        #endregion
    }
}

