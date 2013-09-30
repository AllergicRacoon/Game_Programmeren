using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;


namespace Pong
{
    class GameWorld
    {
        public enum GameState { Running, GameOver, RoundBegin, GameStart };
        GameState curGameState = GameState.GameStart;

        public Pad rightPlayer, leftPlayer;
        Ball ball;

        int maxDisco = 3;
        Disco[] disco;

        Texture2D ballTex, backgroundTex, startTex;
        SpriteFont gameFont;
        String startString = "press space to start", endString;

        public GameWorld(ContentManager Content)
        {
            ballTex = Content.Load<Texture2D>("UI/lives");
            backgroundTex = Content.Load<Texture2D>("UI/background");
            startTex = Content.Load<Texture2D>("UI/startScreen");
            gameFont = Content.Load<SpriteFont>("UI/gameFont");

            MediaPlayer.Play(Content.Load<Song>("Sound/bg_music"));

            rightPlayer = new Pad(Content, 1);
            leftPlayer = new Pad(Content, 0);
            ball = new Ball(Content);
            disco = new Disco[maxDisco];

            for (int i = 0; i < maxDisco; i++)
            {                           
                disco[i] = new Disco(Content);
            }

            Reset(GameState.GameStart); //Sets the game to initial positions and sets correct gamestate
        }

        public void Update(GameTime gameTime)
        {
            if (curGameState == GameState.Running)
            {
                ball.Update(gameTime);
                rightPlayer.Update(gameTime);
                leftPlayer.Update(gameTime);

                foreach (Disco discoLight in disco)
                {
                    discoLight.Update(gameTime);
                }

                //win conditions
                if (ball.position.X + ball.origin.X > PongGame.GameDimensions.X)
                {
                    rightPlayer.lives -= 1;
                    Reset(GameState.RoundBegin);
                }

                if (ball.position.X - ball.origin.X < 0)
                {
                    leftPlayer.lives -= 1;
                    Reset(GameState.RoundBegin);
                }

                if (rightPlayer.lives <= 0)
                {
                    endString = "white player wins\nspace to restart";
                    curGameState = GameState.GameOver;
                }
                if (leftPlayer.lives <= 0)
                {
                    endString = "black player wins\nspace to restart";
                    curGameState = GameState.GameOver;
                }


            }
            else if (curGameState == GameState.RoundBegin || curGameState == GameState.GameStart)
            {
                if (PongGame.InputHelper.KeyPressed(Keys.Space))
                {
                    curGameState = GameState.Running;
                }
            }
            else if (curGameState == GameState.GameOver)
            {
                if (PongGame.InputHelper.KeyPressed(Keys.Space))
                {
                    RestartGame();
                }
            } 
        }



        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (curGameState != GameState.GameStart)
            {
                //Draw in game HUD
                spriteBatch.Begin();
                spriteBatch.Draw(backgroundTex, Vector2.Zero, Color.White);
                for (int i = 0; i < leftPlayer.lives; i += 1)
                {
                    spriteBatch.Draw(ballTex, new Vector2(i * ballTex.Width, 0), Color.White);
                }
                for (int j = 0; j < rightPlayer.lives; j += 1)
                {
                    spriteBatch.Draw(ballTex, new Vector2(PongGame.GameDimensions.X - (j + 1) * ballTex.Width, 0), Color.White);
                }
                spriteBatch.End();

                //Call draw methods on game objects
                ball.Draw(gameTime, spriteBatch);
                rightPlayer.Draw(gameTime, spriteBatch);
                leftPlayer.Draw(gameTime, spriteBatch);
                foreach (Disco discoLight in disco)
                {
                    discoLight.Draw(gameTime, spriteBatch);
                }
            }

            spriteBatch.Begin();
            if (curGameState == GameState.RoundBegin)
            {
                Vector2 stringOffset = gameFont.MeasureString(startString);
                spriteBatch.DrawString(gameFont, startString, new Vector2(PongGame.GameDimensions.X / 2, PongGame.GameDimensions.Y / 2 - 80) - stringOffset / 2, Color.White);
            }
            
            if (curGameState == GameState.GameOver)
            {
                Vector2 stringOffset = gameFont.MeasureString(endString);
                spriteBatch.DrawString(gameFont, endString, new Vector2(PongGame.GameDimensions.X / 2, PongGame.GameDimensions.Y / 2 - 80) - stringOffset / 2, Color.White);
            }

            if (curGameState == GameState.GameStart)
            {
                spriteBatch.Draw(startTex, Vector2.Zero, Color.White);   
            }
            spriteBatch.End();
        }

        public void Reset(GameState gameState)
        {
            ball.Reset(PongGame.Random);
            rightPlayer.Reset();
            leftPlayer.Reset();
            curGameState = gameState;
        }

        public void RestartGame()
        {
            rightPlayer.lives = 3;
            leftPlayer.lives = 3;
            Reset(GameState.GameStart);
        }
    }
}
