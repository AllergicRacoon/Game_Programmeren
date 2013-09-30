using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Pong
{
    class Ball
    {
        Texture2D ballTex;
        public Vector2 position, origin, velocity;

        float bounceMod = 0.004f;

        int ballSpeed = 5;

        Rectangle ballRect;

        int lastIntersectID = -1; //integer to prevent collision with same pad twice

        public Ball(ContentManager content)
        {
            ballTex = content.Load<Texture2D>("GameObjects/ball");
            origin = new Vector2(ballTex.Width, ballTex.Height) / 2;
        }

        public void Update(GameTime gameTime)
        {
            Pad rightPlayer = PongGame.GameWorld.rightPlayer; //copy by reference
            Pad leftPlayer = PongGame.GameWorld.leftPlayer; 

            
            //making collision mask
            ballRect = new Rectangle((int)(position.X-origin.X), (int)(position.Y-origin.Y), ballTex.Width, ballTex.Height);

            //position update
            position.X += velocity.X;
            position.Y += velocity.Y;
            
            //collision check
            if (position.Y - origin.Y < 0 || position.Y + origin.Y > PongGame.GameDimensions.Y)
            {
                velocity.Y *= -1;
            }

            if (ballRect.Intersects(rightPlayer.padRect) && (lastIntersectID == 0 || lastIntersectID == -1))
            {
                velocity.X *= (-1 + (position.Y - (rightPlayer.position.Y + rightPlayer.origin.Y)) * bounceMod);
                lastIntersectID = 1;
            }

            if (ballRect.Intersects(leftPlayer.padRect) && (lastIntersectID == 1 || lastIntersectID == -1))
            {
                velocity.X *= (-1 + (position.Y - (leftPlayer.position.Y + leftPlayer.origin.Y)) * bounceMod);
                lastIntersectID = 0;
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(ballTex, position - origin, Color.White);
            spriteBatch.End();
        }

        public void Reset(Random random)
        {
            //create random velocity
            int xMod = 1, yMod = 1;
            if (random.Next(2) == 0)
            {
                xMod = -1;
            }

            if (random.Next(2) == 0)
            {
                yMod = -1;
            }
            velocity = new Vector2(xMod * ballSpeed, yMod * ballSpeed);
            position = new Vector2(PongGame.GameDimensions.X, PongGame.GameDimensions.Y) / 2;

            //resets intersect id to default value where we can collide with both pads
            lastIntersectID = -1;
        }
    }
}
