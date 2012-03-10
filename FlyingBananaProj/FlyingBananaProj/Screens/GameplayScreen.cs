using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace FlyingBananaProj
{
    class GameplayScreen : GameScreen
    {
        #region Class Variables

        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont gameFont;
        Player player;
        Rectangle UIposition, TokenPosition, ShieldPosition, PowerPosition, shieldSource, powerSource;
        float pauseAlpha;
        Level currentLevel;
        //GameInterface UI;
        Texture2D UI, Missile, Shield, PowerupBar, ShieldBar, Needle;
        Texture2D border;
        bool first = true;
        float wide16x9ratio = 16 / 9;
        float wide16x10ratio = 1.6f;
        float full4x3ratio = 4 / 3;
        bool fadeIn;
        bool fadeOut;
        byte iAlpha;
        bool oldPlayerLocked;
        #endregion

        #region Initialization

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            fadeIn = false;
            fadeOut = false;
            iAlpha = 0;
            oldPlayerLocked = true;
        }

        /// Load graphics content for the game.
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            currentLevel = new Level1(content, ScreenManager.SpriteBatch);
            gameFont = content.Load<SpriteFont>("fonts/gamefont");
            //Used for drawing UI
            spriteBatch = ScreenManager.SpriteBatch;

            player = new Player(content);
            player.resetScore();
            Level.Add(player); //add the player

            UI = content.Load<Texture2D>("ui/UI");
            ShieldBar = content.Load<Texture2D>("ui/shieldbar");
            PowerupBar = content.Load<Texture2D>("ui/powerupbar");
            Needle = content.Load<Texture2D>("ui/Needles");
            Missile = content.Load<Texture2D>("ui/Missile");
            Shield = content.Load<Texture2D>("ui/Shield");
            border = content.Load<Texture2D>("textures/borderTexture");
            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        /// Unload graphics content used by the game.
        public override void UnloadContent()
        {
            content.Unload();
            currentLevel.Destroy();
            Camera.Instance.resetPosition();
        }
        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                currentLevel.Update(gameTime);
            }

            if (player.getShields() == -1 && first)
            {
                ScreenManager.AddScreen(new GameOverScreen(), ControllingPlayer);
                first = false;
            }
            if (currentLevel.isFinished() && first)
            {
                ScreenManager.AddScreen(new ToBeContinuedScreen(), ControllingPlayer);
                first = false;
            }

        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a red background. Why? Because BLOOD LOL
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Red, 0, 0);
            currentLevel.Draw();
            Game1.getGraphicsDevice().SetRenderTarget(null);

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);
                ScreenManager.FadeBackBufferToBlack(alpha);
            }

            if (player.controls.Locked && !oldPlayerLocked) //fade out
            {
                //Debug.WriteLine("fade out!");
                fadeOut = true;
                fadeIn = false;
                iAlpha = 255;
            }
            else if (!player.controls.Locked && oldPlayerLocked) //fade in
            {
                //Debug.WriteLine("fade in!");
                fadeIn = true;
                fadeOut = false;
                iAlpha = 0;
            }
            else
            {
                if (fadeIn)
                {
                    iAlpha += 1;
                    //Debug.WriteLine("alpha="+iAlpha);
                    if (iAlpha == 255)
                        fadeIn = false;
                    //Debug.WriteLine("fadein=" + fadeIn);
                }
                if (fadeOut)
                {
                    iAlpha -= 1;
                    //Debug.WriteLine("alpha=" + iAlpha);
                    if (iAlpha == 0)
                        fadeOut = false;
                    //Debug.WriteLine("fadeout=" + fadeOut);
                }
            }

            DrawUI();

            oldPlayerLocked = player.controls.Locked;

            ScreenManager.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
            ScreenManager.GraphicsDevice.BlendState = BlendState.Opaque;
            ScreenManager.GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            ScreenManager.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
        }

        //Draws UI elements
        void DrawUI()
        {
            Color final = Color.White;
            final.A = iAlpha;
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            UIposition = new Rectangle(28, 40, UI.Width / 6, UI.Height / 6);
            ShieldPosition = new Rectangle(37, 152, ShieldBar.Width / 6, ShieldBar.Height / 6);
            PowerPosition = new Rectangle(98, 182, PowerupBar.Width / 6, PowerupBar.Height / 6);
            TokenPosition = new Rectangle(55, 62, Needle.Width / 6, Needle.Height / 6);

            // Source rectangles
            if (player.getShields() == 3)
                shieldSource = new Rectangle(0, 0, ShieldBar.Width, ShieldBar.Height);
            else if (player.getShields() == 2)
                shieldSource = new Rectangle(0, 407, ShieldBar.Width, ShieldBar.Height);
            else if (player.getShields() == 1)
                shieldSource = new Rectangle(0, 740, ShieldBar.Width, ShieldBar.Height);
            else if (player.getShields() == 0)
                shieldSource = new Rectangle(0, 1407, ShieldBar.Width, ShieldBar.Height);

            powerSource = new Rectangle(0, 1060 - player.getPowerup(), PowerupBar.Width, PowerupBar.Height);

            spriteBatch.Begin();
            //handle borders
            float aspect = Game1.getGraphicsDevice().Viewport.Bounds.Width / Game1.getGraphicsDevice().Viewport.Bounds.Height;
            if (aspect == wide16x9ratio || aspect == wide16x10ratio)
            {
                int newHeight = Game1.getGraphicsDevice().Viewport.Bounds.Height * 4;
                newHeight /= 3;
                int offset = (Game1.getGraphicsDevice().Viewport.Bounds.Width - newHeight) / 2;
                spriteBatch.Draw(border, new Rectangle(0, 0, offset, Game1.getGraphicsDevice().Viewport.Bounds.Height), final);
                spriteBatch.Draw(border, new Rectangle(Game1.getGraphicsDevice().Viewport.Bounds.Width - offset, 0, offset, Game1.getGraphicsDevice().Viewport.Bounds.Height), final);
            }
            else //borders for more arbitrary resolutions
            {
                if (Game1.getGraphicsDevice().Viewport.Bounds.Width == 1366 && Game1.getGraphicsDevice().Viewport.Bounds.Height == 768)
                {
                    spriteBatch.Draw(border, new Rectangle(0, 0, 171, 768), final);
                    spriteBatch.Draw(border, new Rectangle(1195, 0, 171, 768), final);
                }
                if (Game1.getGraphicsDevice().Viewport.Bounds.Width == 1360 && Game1.getGraphicsDevice().Viewport.Bounds.Height == 768)
                {
                    spriteBatch.Draw(border, new Rectangle(0, 0, 168, 768), final);
                    spriteBatch.Draw(border, new Rectangle(1192, 0, 168, 768), final);
                }
            }
            if (iAlpha > 0)
            {
                spriteBatch.Draw(ShieldBar, ShieldPosition, shieldSource, final);
                spriteBatch.Draw(PowerupBar, PowerPosition, powerSource, final);
                //Renders PlayerWeapon Token
                switch (player.Weapon.Name)
                {
                    case PlayerWeaponName.Missile:
                        ScreenManager.SpriteBatch.Draw(Missile, TokenPosition, final);
                        break;
                    case PlayerWeaponName.Needle:
                        ScreenManager.SpriteBatch.Draw(Needle, TokenPosition, final);
                        break;
                    case PlayerWeaponName.Shield:
                        ScreenManager.SpriteBatch.Draw(Shield, TokenPosition, final);
                        break;
                }
                spriteBatch.Draw(UI, UIposition, final);
            }

            spriteBatch.DrawString(gameFont, "" + player.getScore(), new Vector2(256, 34), final);
            if (Game1.debug)
            {
                spriteBatch.DrawString(gameFont, "" + player.Position, new Vector2(256, 340), Color.White);
                spriteBatch.DrawString(gameFont, "" + Camera.Instance.getPosition(), new Vector2(256, 440), Color.White);
                spriteBatch.DrawString(gameFont, "" + player.getPowerup(), new Vector2(256, 240), Color.White);
                spriteBatch.DrawString(gameFont, "" + currentLevel.getZ(), new Vector2(156, 540), Color.White);
            }

            spriteBatch.End();
        }
        #endregion
    }
}
