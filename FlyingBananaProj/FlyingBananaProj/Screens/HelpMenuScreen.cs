using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    class HelpMenuScreen : MenuScreen
    {
        #region Class Variables
        int width = Game1.getGraphicsDevice().Viewport.Bounds.Width;
        int height = Game1.getGraphicsDevice().Viewport.Bounds.Height;
        Texture2D straightCell, chargingCell, latchingCell, missileBot, meleeBot,
            bars;
        double fadeDelay = .035;
        float alphaValueCredits = 0;
        double fadeAmountCredits = .15;
        SpriteFont gameFont;
        #endregion

        public HelpMenuScreen()
        { }

        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            straightCell = content.Load<Texture2D>("ui/straightcell_image");
            chargingCell = content.Load<Texture2D>("ui/chargingcell_image");
            missileBot = content.Load<Texture2D>("ui/missilebot_image");
            latchingCell = content.Load<Texture2D>("ui/latchingcell_image");
            meleeBot = content.Load<Texture2D>("ui/meleebot_image");
            bars = content.Load<Texture2D>("ui/bars_image");
            gameFont = content.Load<SpriteFont>("fonts/helpfont");
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

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            //Game1.getGraphicsDevice().Viewport.Bounds.Width
            Rectangle straightCellSpace = new Rectangle(width / 16, height / 8, straightCell.Width, straightCell.Height);
            Rectangle chargingCellSpace = new Rectangle(width / 16, height / 4, chargingCell.Width, chargingCell.Height);
            Rectangle latchingCellSpace = new Rectangle(width / 16, height / 8 * 3, latchingCell.Width, latchingCell.Height);
            Rectangle missileBotSpace = new Rectangle(width / 16, height / 2, missileBot.Width, missileBot.Height);
            Rectangle meleeBotSpace = new Rectangle(width / 16, height / 8 * 5, meleeBot.Width, meleeBot.Height);
            Rectangle barsSpace = new Rectangle(width / 2, height / 16, bars.Width, bars.Height);
            spriteBatch.Begin();

            spriteBatch.Draw(straightCell, straightCellSpace,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueCredits);
            spriteBatch.DrawString(gameFont, "Don't kill these!", new Vector2((int)width / 16 + straightCell.Width, (int)height / 8 + straightCell.Height / 2), Color.White);

            spriteBatch.Draw(chargingCell, chargingCellSpace,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueCredits);
            spriteBatch.DrawString(gameFont, "Infected white blood cells are fair game!", new Vector2((int)width / 16 + chargingCell.Width, (int)height / 4 + chargingCell.Height / 2), Color.White);

            spriteBatch.Draw(latchingCell, latchingCellSpace,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueCredits);
            spriteBatch.DrawString(gameFont, "These cells can slow you down for other baddies!", new Vector2((int)width / 16 + latchingCell.Width, (int)height / 8 * 3 + latchingCell.Height / 2), Color.White);

            spriteBatch.Draw(missileBot, missileBotSpace,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueCredits);
            spriteBatch.DrawString(gameFont, "Watch out for these missile firing bots!", new Vector2((int)width / 16 + missileBot.Width, (int)height / 2 + missileBot.Height / 2), Color.White);

            spriteBatch.Draw(meleeBot, meleeBotSpace,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueCredits);
            spriteBatch.DrawString(gameFont, "Don't get let these bots get close enough for a punch!", new Vector2((int)width / 16 + meleeBot.Width, (int)height / 8 * 5 + meleeBot.Height / 2), Color.White);
            spriteBatch.Draw(bars, barsSpace,
                             new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha) * alphaValueCredits);
            spriteBatch.DrawString(gameFont, "The left blue bar is your shield - you can get hit four", new Vector2((int)width / 2 + bars.Width, (int)height / 16 + bars.Height / 4), Color.White);
            spriteBatch.DrawString(gameFont, "times before kickin' it!", new Vector2((int)width / 2 + bars.Width, (int)height / 16 + bars.Height / 4 + 24), Color.White);

            spriteBatch.DrawString(gameFont, "The right orange bar your powerup - press the", new Vector2((int)width / 2 + bars.Width, (int)height / 16 + bars.Height / 2), Color.White);
            spriteBatch.DrawString(gameFont, "power up button at levels 1, 2, or 3 to boost", new Vector2((int)width / 2 + bars.Width, (int)height / 16 + bars.Height / 2 + 24), Color.White);
            spriteBatch.DrawString(gameFont, "your weapons for five seconds!", new Vector2((int)width / 2 + bars.Width, (int)height / 16 + bars.Height / 2 + 48), Color.White);
            spriteBatch.End();
        }

    }
}
