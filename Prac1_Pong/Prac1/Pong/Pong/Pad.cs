using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Pong
{
    class Pad
    {
        public Vector2 position, startPosition, origin, collisionOrigin;
        public Texture2D playerTex, collisionTex;
        Keys upKey, downKey;

        public Rectangle padRect;

        int playerSpeed = 5;

        public int lives = 3;

        int playerIndex;

        int collisionOffset = 15; //a slight offset is added to the rect collision because otherwise the collision box is sometimes slightly too small

        int sideOffset = 70; //offset from side of screen to pad

        bool playerBoogie = true; //You can turn this off if you don't want to use boogie

        public Pad(ContentManager content, int playerID)
        {
            playerIndex = playerID;

            collisionTex = content.Load<Texture2D>("GameObjects/collisionMask");

            if (playerIndex == 0)
            {
                upKey = Keys.W;
                downKey = Keys.S;
                playerTex = content.Load<Texture2D>("GameObjects/paddleLeft");
            }
            else
            {
                upKey = Keys.Up;
                downKey = Keys.Down;
                playerTex = content.Load<Texture2D>("GameObjects/paddleRight");
            }

            collisionOrigin = new Vector2(collisionTex.Width, collisionTex.Height) / 2;
            origin = new Vector2(playerTex.Width, playerTex.Height) / 2;
        }

        public void Update(GameTime gameTime)
        {
            padRect = new Rectangle((int)(position.X - collisionOrigin.X - collisionOffset), (int)(position.Y - collisionOrigin.Y), collisionTex.Width + collisionOffset, collisionTex.Height);
            if (PongGame.InputHelper.KeyDown(upKey))
            {
                position.Y -= playerSpeed;
            }
            if (PongGame.InputHelper.KeyDown(downKey))
            {
                position.Y += playerSpeed;
            }
            if (playerBoogie)
            {
                position.X = xBoogie(position.Y);
            }
            position.Y = Math.Min(PongGame.GameDimensions.Y - (playerTex.Height - origin.Y), position.Y);
            position.Y = Math.Max(origin.Y, position.Y);   
        }

        public float xBoogie(float curY) //function that makes the player path wobble
        {
            return startPosition.X + 50 * (float)Math.Sin((double)((Math.PI * 2) / 300 * curY));
        }
        

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            //spriteBatch.Draw(collisionTex, position - collisionOrigin, Color.White);
            spriteBatch.Draw(playerTex, position - origin, Color.White);
            spriteBatch.End();

        }

        public void Reset()
        {
            if (playerIndex == 0)
            {
                position = new Vector2(sideOffset, PongGame.GameDimensions.Y / 2);
            }
            else
            {
                position = new Vector2(PongGame.GameDimensions.X - sideOffset, PongGame.GameDimensions.Y / 2);
            }

            startPosition = position;

            if (playerBoogie)
            {
                position.X = xBoogie(position.Y);
            }

        }
    }
}
