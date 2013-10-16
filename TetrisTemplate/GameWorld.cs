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

        Texture2D backgroundTex, gradientTex, HUDTex, suspicionBarTex;

        Vector2 suspicionBarOffset;

        int suspicionBarMaxHeight, suspicionBarHeight, suspicionBarMaxWidth;
        float suspicionValue;

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

            backgroundTex = Content.Load<Texture2D>("GameBackground");
            gradientTex = Content.Load<Texture2D>("GameGradient");
            HUDTex = Content.Load<Texture2D>("GameHUD");
            suspicionBarTex = Content.Load<Texture2D>("SuspicionBar");

            suspicionBarOffset = new Vector2(574, 565);
            suspicionBarMaxHeight = (int)suspicionBarOffset.Y - 174;
            suspicionBarMaxWidth = 677 - (int)suspicionBarOffset.X;

            suspicionValue = 0.8f;

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

            if (gameState == GameState.Playing)
            {
                grid.HandleInput(gameTime, inputHelper);
            }
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

            spriteBatch.Draw(backgroundTex, Vector2.Zero, Color.White);
            spriteBatch.Draw(gradientTex, Vector2.Zero, Color.White);
            grid.Draw(gameTime, spriteBatch);
            suspicionBarHeight = (int)(suspicionBarMaxHeight*suspicionValue);
            spriteBatch.Draw(suspicionBarTex, new Rectangle((int)suspicionBarOffset.X, (int)suspicionBarOffset.Y - suspicionBarHeight, suspicionBarMaxWidth, suspicionBarHeight), Color.White);
            
            spriteBatch.Draw(HUDTex, Vector2.Zero, Color.White);
            
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