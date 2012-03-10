using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace FlyingBananaProj
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static bool debug = false;
        static GraphicsDeviceManager graphics;
        ScreenManager screenManager;
        Camera camera;
        static readonly string[] preloadAssets = 
        {
            "ui/gradient",
        };

        public Game1()
        {
            Content.RootDirectory = "Content";

            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.IsFullScreen = false;
    
            screenManager = new ScreenManager(this);
            Components.Add(screenManager);
            
            screenManager.AddScreen(new BackgroundScreen(), null);
        }

        public static void changeFullScreen()
        {
            graphics.ToggleFullScreen();
        }

        //Load graphics content
        protected override void LoadContent()
        {
            string asset;
            for (int i = 0; i < preloadAssets.Length; i++)
            {
                asset = preloadAssets[i];
                Content.Load<object>(asset);
            }
            camera = new Camera(this);
        }

        public static GraphicsDevice getGraphicsDevice()
        {
            return graphics.GraphicsDevice;
        }
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
    static class Program
    {
        static void Main()
        {
            using (Game1 game = new Game1())
            {
                game.Run();
            }
        }
    }
}