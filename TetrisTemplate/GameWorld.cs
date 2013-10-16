using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;

/*
 * A class for representing the game world.
 */
namespace TetrisPrac
{
    class GameWorld
    {
        /*
         * enum for different game states (playing or game over)
         */
        enum GameState
        {
            Playing, GameOver
        }

        /*
         * screen width and height
         */
        int screenWidth, screenHeight;

        /*
         * random number generator
         */
        static Random random;

        Texture2D backgroundTex;

        /*
         * main game font
         */
        SpriteFont font;


        /*
         * the current game state
         */
        GameState gameState;

        /*
         * the main playing grid
         */
        TetrisGrid grid;

        public GameWorld(int width, int height, ContentManager Content)
        {
            screenWidth = width;
            screenHeight = height;
            random = new Random();
            gameState = GameState.Playing;

            font = Content.Load<SpriteFont>("SpelFont");

            backgroundTex = Content.Load<Texture2D>("GameHUD");

            grid = new TetrisGrid(Content);
        }

        public void Reset()
        {
            grid.Reset();
        }

        public void HandleInput(GameTime gameTime, InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.R, false))
            {
                Reset();
                gameState = GameState.Playing;
            }
            grid.HandleInput(gameTime, inputHelper);
        }

        public void Update(GameTime gameTime)
        {
            if (gameState == GameState.Playing)
            {
                grid.Update(gameTime);
                if (grid.gameOver)
                {
                    gameState = GameState.GameOver; 
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            grid.Draw(gameTime, spriteBatch);
            if (gameState == GameState.GameOver)
            {
                DrawText("Game Over Loser.\nMuhahaha\npress r to restart.", new Vector2(screenWidth, screenHeight) / 2, spriteBatch);
            }
            spriteBatch.End();    
        }

        /*
         * utility method for drawing text on the screen
         */
        public void DrawText(string text, Vector2 positie, SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(font, text, positie, Color.Blue);
        }

        public static Random Random
        {
            get { return random; }
        }
    }
}