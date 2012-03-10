#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace FlyingBananaProj
{
    class CreditsMenuScreen : MenuScreen
    {
        #region Class Variables
        Texture2D credits;
        SpriteFont bigFont;
        String back = "Go back";
        int width = Game1.getGraphicsDevice().Viewport.Bounds.Width;
        int height = Game1.getGraphicsDevice().Viewport.Bounds.Height;
        double fadeDelay = .035;
        float alphaValueCredits = 0;
        double fadeAmountCredits = .15;
        #endregion

        public CreditsMenuScreen()
        { }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            credits = content.Load<Texture2D>("ui/Credits");
            bigFont = content.Load<SpriteFont>("fonts/gameFont");
        }
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            if (input.IsMenuCancel(ControllingPlayer, out playerIndex))
            {
                ExitScreen();
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            fadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;

            if (fadeDelay <= 0 || alphaValueCredits == 1)
            {
                //Reset the Fade delay
                fadeDelay = .035;

                //Increment/Decrement the fade value for the image
                alphaValueCredits += (float)(fadeAmountCredits);
            }
        }

        public override void  Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            spriteBatch.Begin();
            spriteBatch.Draw(credits, fullscreen,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha)*alphaValueCredits);
            spriteBatch.DrawString(bigFont, back, new Vector2((int)width - back.Length * 20, (int)height - 48), Color.White);
            spriteBatch.End();
        }
    }
}
