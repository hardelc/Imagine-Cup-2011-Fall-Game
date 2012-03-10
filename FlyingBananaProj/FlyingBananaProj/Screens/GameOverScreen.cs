using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class GameOverScreen : MenuScreen
    {
        #region Class Variables
        Texture2D gameOver;
        SpriteFont gameFont;
        double fadeDelay = .035;
        double fadeDelay2 = .035;

        float alphaValuePulse = 0;
        double fadeAmountPulse = .1;

        float alphaValueGO = 0;
        double fadeAmountGO = .15;
        #endregion

        public GameOverScreen()
        { }

        public override void LoadContent()
        {

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameOver = content.Load<Texture2D>("ui/GameOver");
            gameFont = content.Load<SpriteFont>("fonts/gameFont");
        }
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            if (input.IsMenuSelect(ControllingPlayer, out playerIndex))
            {
                LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen());
            }
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            fadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;

            if (fadeDelay <= 0 && alphaValueGO <= 1)
            {
                //Reset the Fade delay
                fadeDelay = .035;

                //Increment the fade value for the image
                alphaValueGO += (float)(fadeAmountGO);
            }
            fadeDelay2 -= gameTime.ElapsedGameTime.TotalSeconds;
            if (fadeDelay2 <= 0 && alphaValueGO >= 1)
            {
                //Reset the Fade delay
                fadeDelay2 = .1;


                //Increment/Decrement the fade value for the image
                alphaValuePulse += (float)(fadeAmountPulse);

                if(alphaValuePulse <= 0f || alphaValuePulse >= 1f)
                    fadeAmountPulse *= -1;

            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin();
            spriteBatch.Draw(gameOver, fullscreen,new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueGO);
#if XBOX
            spriteBatch.DrawString(gameFont, "Press Start", new Vector2(viewport.Width, viewport.Height), Color.Red * alphaValuePulse);
#endif

#if WINDOWS
            spriteBatch.DrawString(gameFont, "Press Enter", new Vector2((viewport.Width / 2) - 80, (viewport.Width - viewport.Height) - 100), Color.Violet * alphaValuePulse);
#endif
            spriteBatch.End();
            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

    }
}

