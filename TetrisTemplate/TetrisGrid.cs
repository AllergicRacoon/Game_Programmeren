using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

/*
 * a class for representing the Tetris playing grid
 */
namespace TetrisPrac
{
    class TetrisGrid
    {
        /*
         * sprite for representing a single grid block
         */
        Texture2D gridblock;

        Color[,] landedGrid, fullGrid;

        /*
         * the position of the tetris grid
         */
        Vector2 position;

        Vector2 blockPosition;

        TetrisBlock activeBlock;

        protected double moveTimer, timeToMove;

        /*
         * width in terms of grid elements
         */
        public int Width
        {
            get { return 12; }
        }

        /*
         * height in terms of grid elements
         */
        public int Height
        {
            get { return 20; }
        }

        public TetrisGrid(Texture2D b)
        {
            fullGrid = new Color[Width, Height];
            landedGrid = new Color[Width, Height];
            gridblock = b;
            position = Vector2.Zero;
            blockPosition = new Vector2(2, 2);
            this.Clear();
        }


        /*
         * clears the grid
         */

        public void Clear()
        {
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    fullGrid[i, j] = Color.White;
                    landedGrid[i, j] = Color.White;
                }
            }
        }

        /*
         * draws the grid on the screen
         */
        public void Draw(GameTime gameTime, SpriteBatch s)
        {
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    s.Draw(gridblock, new Vector2(i * gridblock.Width, j * gridblock.Height), fullGrid[i, j]);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int j = 0; j < activeBlock.arraySize; j++)
            {
                for (int i = 0; i < activeBlock.arraySize; i++)
                {
                    if (activeBlock.blockArray[i, j])
                    {
                        fullGrid[i + (int)blockPosition.X, j + (int)blockPosition.Y] = activeBlock.blockColor;
                    }
                }
            }
            moveTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (moveTimer > timeToMove)
            {
                moveTimer = 0;
                blockPosition.Y += 1; //todo add collision check
            }
            moveTimer = 0; //In the original Tetris your blocks don't fall if you rotate (+ it works and looks better)
        }
    }
}
