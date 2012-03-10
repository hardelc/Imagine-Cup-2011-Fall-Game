using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlyingBananaProj
{
    class ControlsMenuScreen : MenuScreen
    {
        #region Class Variables
        bool gamepadConnected;
        Texture2D keyboard;
        Texture2D gamepad;
        int width = Game1.getGraphicsDevice().Viewport.Bounds.Width;
        int height = Game1.getGraphicsDevice().Viewport.Bounds.Height;
        double fadeDelay = .035;
        float alphaValueCredits = 0;
        double fadeAmountCredits = .15;
        #endregion

        public ControlsMenuScreen()
        { }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            keyboard = content.Load<Texture2D>("ui/keyboardControls");
            gamepad = content.Load<Texture2D>("ui/gamepadControls");
        }
        public override void HandleInput(InputState input)
        {
            PlayerIndex playerIndex;
            int pIndex = (int)ControllingPlayer.Value;
            GamePadState gamePadState = input.CurrentGamePadStates[pIndex];
            if (gamePadState.IsConnected)
            {
                gamepadConnected = true;
            }
            else
                gamepadConnected = false;
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

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle gamepadSpace = new Rectangle(width / 2 - gamepad.Width / 2, height / 2 - gamepad.Height/2, gamepad.Width, gamepad.Height);
            Rectangle keyboardSpace = new Rectangle(width/2 - keyboard.Width/2, height/2 - keyboard.Height/2, keyboard.Width, keyboard.Height);
            spriteBatch.Begin();

            if (gamepadConnected)
            {
                spriteBatch.Draw(gamepad, gamepadSpace,
                                 new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueCredits);
            }
            else
            {
#if XBOX
                spriteBatch.Draw(gamepad, gamepadSpace,
                                 new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueCredits);
#else
                spriteBatch.Draw(keyboard, keyboardSpace,
                                new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueCredits);
#endif
            }
            spriteBatch.End();
        }

    }
}
