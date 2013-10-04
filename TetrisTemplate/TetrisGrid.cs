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
            timeToMove = 0.25d;
            activeBlock = RandomBlock();
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
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    fullGrid[i, j] = landedGrid[i, j]; //resets the full grid so it doesn't include moving block position
                }
            }

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
            Console.WriteLine(blockPosition.Y);
            moveTimer += gameTime.ElapsedGameTime.TotalSeconds;
            

            if (moveTimer > timeToMove)
            {
                moveTimer = 0;
                if (canMove((int)blockPosition.X, (int)blockPosition.Y + 1))
                {
                    blockPosition.Y += 1; //todo add collision check
                }
                else
                {
                    
                }
                
            }
           
        }


        public bool canMove(int x, int y)
        {

            for (int j = 0; j < activeBlock.arraySize; j++)
            {
                for (int i = 0; i < activeBlock.arraySize; i++)
                {
                    if (activeBlock.blockArray[i, j] && (x + i > Width || x + i < 0 || y + j > Height)) //is there a block outside the grid?
                    {
                        if (landedGrid[x + i, y + j] != Color.White) //is there a block already there
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        public TetrisBlock RandomBlock()
        {
            int randomValue = GameWorld.Random.Next(7);
            switch (randomValue)
            {
                case 0:
                    return new JBlock(gridblock);
                case 1:
                    return new LBlock(gridblock);
                case 2:
                    return new SBlock(gridblock);
                case 3:
                    return new SquareBlock(gridblock);
                case 4:
                    return new TallBlock(gridblock);
                case 5:
                    return new TBlock(gridblock);
                case 6:
                    return new ZBlock(gridblock);
            }
            return new LBlock(gridblock);
        }

    }
}
