using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    class PongGame : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public static Vector2 GameDimensions;

        public static InputHelper InputHelper;
        
        public static GameWorld GameWorld;

        public static Random Random;


        static void Main()
        {
            PongGame game = new PongGame();
            game.Run();
        }

        public PongGame()
        {
            Content.RootDirectory = "Content";
            graphics = new GraphicsDeviceManager(this);
            InputHelper = new InputHelper();
            Random = new Random();

            GameDimensions = new Vector2(graphics.PreferredBackBufferWidth,graphics.PreferredBackBufferHeight);
        }
        

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GameWorld = new GameWorld(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            GameWorld.Update(gameTime);
            InputHelper.Update();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            GameWorld.Draw(gameTime, spriteBatch);
        }
    }
}