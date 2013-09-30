using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace Pong
{
    class Disco
    {
        Texture2D discoTex;

        Vector2 position, destination, origin;

        float discoScale = 2f;
        Color discoColor;

        public Disco(ContentManager content)
        {
            discoTex = content.Load<Texture2D>("GameObjects/disco");
            origin = new Vector2(discoTex.Width, discoTex.Height) / 2;
            
            position = randomPosition();
            destination = randomPosition();

            discoScale = randomSize();
            discoColor = randomColor();
        }

        public void Update(GameTime gameTime)
        {
            position += (destination - position) * 0.02f;

            if ((destination - position).Length() < 20) //since we will never actually reach our destination we will get a new one when we are close enough
            {
                destination = randomPosition();
                discoColor = randomColor();
                //discoScale = randomSize();
            }
        }

        

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(discoTex, position, null, discoColor, 0f, origin, discoScale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public Color randomColor()
        {
            return new Color(PongGame.Random.Next(70), PongGame.Random.Next(70), PongGame.Random.Next(70), 1);
        }

        public float randomSize()
        {
            return (float)(PongGame.Random.NextDouble() * 0.75 + 0.5d);
        }

        public Vector2 randomPosition()
        {
            return new Vector2(PongGame.Random.Next((int)PongGame.GameDimensions.X), PongGame.Random.Next((int)PongGame.GameDimensions.Y));
        }
    }
}
