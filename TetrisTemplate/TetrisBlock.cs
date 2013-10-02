using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class TetrisBlock
    {
        public Vector2 gridPosition;
        protected bool[,] blockArray;
        protected Texture2D blockTex;
        protected Color blockColor;

        public TetrisBlock(Texture2D tex)
        {
            blockTex = tex;
            blockArray = new bool[4, 4];
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (blockArray[i,j])
                    {
                        spriteBatch.Draw(blockTex, new Vector2((gridPosition.X + j) * blockTex.Width, (gridPosition.Y + i) * blockTex.Height), blockColor);
                    }
                }
            }
        }

        public void RotateCW()
        {
            bool[,] temp = new bool[4, 4];
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    temp[i, j] = blockArray[j, 4 - i - 1];
                }
            }

            blockArray = temp;
        }
    }
}
