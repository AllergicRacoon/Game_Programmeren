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
        public TetrisGrid(Texture2D b)
        {
            levelGrid = new Color[Width, Height];
            gridblock = b;
            position = Vector2.Zero;
            this.Clear();
        }

        /*
         * sprite for representing a single grid block
         */
        Texture2D gridblock;

        Color[,] levelGrid;

        /*
         * the position of the tetris grid
         */
        Vector2 position;

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

        /*
         * clears the grid
         */
        public void Clear()
        {
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    levelGrid[i, j] = Color.White;
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
                    s.Draw(gridblock, new Vector2(i * gridblock.Width, j * gridblock.Height), levelGrid[i, j]);
                }
            }
        }
    }
}
