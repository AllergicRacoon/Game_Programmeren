using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

/*
 * A class for representing the game world.
 */
namespace TetrisPrac
{
    enum GameState
    {
        Playing, GameOver, StartScreen
    }

    class GameWorld
    {
   
        int screenWidth, screenHeight; //game dimensions

        static Random random;

        Texture2D backgroundTex, gradientTex, HUDTex, suspicionBarTex; //the HUD textures

        Vector2 suspicionBarOffset; //offset for where to draw on the HUD

        int suspicionBarMaxHeight, suspicionBarHeight, suspicionBarMaxWidth; //variables for the suspicion bar

        SpriteFont font; // game font

        public string endString;

        GameState gameState; // the current game state

        TetrisGrid grid; // the playing grid

        public GameWorld(int width, int height, ContentManager Content)
        {
            screenWidth = width;
            screenHeight = height;
            random = new Random();
            gameState = GameState.StartScreen;

            MediaPlayer.Play(Content.Load<Song>("Song"));

            //load all the UI textures
            font = Content.Load<SpriteFont>("SpelFont");
            backgroundTex = Content.Load<Texture2D>("GameBackground");
            gradientTex = Content.Load<Texture2D>("GameGradient");
            HUDTex = Content.Load<Texture2D>("GameHUD");
            suspicionBarTex = Content.Load<Texture2D>("SuspicionBar");

            //these are the values for where the suspicion bar is located and how wide and high it is
            suspicionBarOffset = new Vector2(578, 565);
            suspicionBarMaxHeight = (int)suspicionBarOffset.Y - 174;
            suspicionBarMaxWidth = 680 - (int)suspicionBarOffset.X;

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
                gameState = GameState.StartScreen;
            }

            if (gameState == GameState.Playing)
            {
                grid.HandleInput(gameTime, inputHelper);
            }

            if (gameState == GameState.StartScreen)
            {
                if (inputHelper.KeyPressed(Keys.Space, false))
                {
                    gameState = GameState.Playing;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (gameState == GameState.Playing)
            {
                grid.Update(gameTime);

                //win conditions

                if (grid.HasReachedTop)
                {
                    gameState = GameState.GameOver;
                    endString = "Player 2 wins!";
                }
                if (grid.SuspicionValue > 1)
                {
                    gameState = GameState.GameOver;
                    endString = "Player 1 wins!";
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(backgroundTex, Vector2.Zero, Color.White); //draw the paper background first
            

            if (gameState == GameState.Playing)
            {
                grid.Draw(gameTime, spriteBatch);
                suspicionBarHeight = (int)(suspicionBarMaxHeight * grid.SuspicionValue);
                spriteBatch.Draw(suspicionBarTex, new Rectangle((int)suspicionBarOffset.X, (int)suspicionBarOffset.Y - suspicionBarHeight, suspicionBarMaxWidth, suspicionBarHeight), Color.White);
                spriteBatch.Draw(HUDTex, Vector2.Zero, Color.White);
            }

            if (gameState == GameState.StartScreen)
            {
                DrawText("ILLUMINATRIS", new Vector2(screenWidth / 2, screenHeight / 2 - 240), spriteBatch);

                DrawText("Player 1: A & D to move, W to rotate and S for fast fall", new Vector2(screenWidth / 2, screenHeight / 2 - 160), spriteBatch);
                DrawText("Player 2: Num1 through Num7 will change", new Vector2(screenWidth / 2, screenHeight / 2 - 100), spriteBatch);
                DrawText("the next block that will fall down", new Vector2(screenWidth / 2, screenHeight / 2 - 60), spriteBatch);

                DrawText("Repeating the same blocks and clearing rows will raise suspicion", new Vector2(screenWidth / 2, screenHeight / 2 + 20), spriteBatch);
                DrawText("If the suspicion bar is full, player 1 wins", new Vector2(screenWidth / 2, screenHeight / 2 + 80), spriteBatch);
                DrawText("If the blocks reach the top, player 2 wins", new Vector2(screenWidth / 2, screenHeight / 2 + 140), spriteBatch);

                DrawText("Press Space to start", new Vector2(screenWidth / 2, screenHeight / 2 + 240), spriteBatch);
            }

            if (gameState == GameState.GameOver)
            {
                DrawText(endString, new Vector2(screenWidth / 2, screenHeight / 2 - 150), spriteBatch);
                DrawText("Press R to restart", new Vector2(screenWidth / 2, screenHeight / 2 + 100), spriteBatch);
            }
            
            spriteBatch.Draw(gradientTex, Vector2.Zero, Color.White); //draw the gradient overlay last
            spriteBatch.End();    
        }

        /*
         * utility method for drawing text on the screen
         */
        public void DrawText(string text, Vector2 positie, SpriteBatch spriteBatch)
        {
            Vector2 offset = font.MeasureString(text);
            spriteBatch.DrawString(font, text, positie - offset/2, new Color(0,0,0,0.76f));
        }

        public static Random Random
        {
            get { return random; }
        }
    }
}