using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region Class Variables
        ContentManager content;
        Texture2D backgroundTexture;
        Texture2D Text;
        double fadeDelay = .035;
        bool firstTime;

        float alphaValueBackground = 0;
        double fadeAmountBackground = .01;

        float alphaValueText = 0;
        double fadeAmountText = .02;
        double pauseTime = 4.0;
        #endregion

        #region Initialization


        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Loads graphics content for this screen.
        /// </summary>
        public override void LoadContent()
        {
            firstTime = true;

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>("ui/Background-No text");
            Text = content.Load<Texture2D>("ui/Background-Text");

            AudioVideoController song = new AudioVideoController();
            //song.playMusic("01 Ghosts I");
        }

        public override void HandleInput(InputState input)
        {

            if(input.IsPauseGame(ControllingPlayer))
            {
                alphaValueBackground = 1;
                alphaValueText = 1;
            }
        }

        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }
        #endregion

        #region Update and Draw

        /// <summary>
        /// Handles the fading in of the background and title. If title has been fully 
        /// faded in, BackgroundScreen will create a MainMenuScreen.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            if (pauseTime>0)
                pauseTime -= gameTime.ElapsedGameTime.TotalSeconds;

            fadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
            if (fadeDelay <= 0 || alphaValueBackground == 1)
            {
                //Reset the Fade delay
                fadeDelay = .035;

                //Increment/Decrement the fade value for the image
                alphaValueBackground += (float)(fadeAmountBackground);
            }

            if (pauseTime <= 0)
            {
                fadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                if (fadeDelay <= 0 || alphaValueText==1)
                {
                    //Reset the Fade delay
                    fadeDelay = .035;

                    //Increment/Decrement the fade value for the image
                    alphaValueText += (float)(fadeAmountText);
                }
            }
            if(firstTime)
            {
                if (alphaValueText >= 1)
                {
                    firstTime = false;
                    ScreenManager.AddScreen(new MainMenuScreen(), ControllingPlayer);
                    ScreenManager.Game.ResetElapsedTime();
                }
            }
            base.Update(gameTime, otherScreenHasFocus, false);
        }


        /// <summary>
        /// Draws the BackgroundScreen components.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTexture, fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha)*alphaValueBackground);
            spriteBatch.Draw(Text, fullscreen, new Color(255, 255, 255) * alphaValueText);

            spriteBatch.End();

        }


        #endregion
    }
}
