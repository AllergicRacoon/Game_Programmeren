using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*
 * a class for representing the Tetris playing grid
 */
namespace TetrisPrac
{
    class TetrisGrid
    {
        Texture2D gridblock, emptyBlock; //texture for block

        Color[,] levelGrid;

        Vector2 position;

        Vector2 blockPosition;

        TetrisBlock activeBlock, nextBlock;

        protected double moveTimer, timeToMove;

        public bool gameOver;

        int blockCount;
        /*
         * width in terms of grid elements
         */
        public int Width
        {
            get { return 7; }
        }

        /*
         * height in terms of grid elements
         */
        public int Height
        {
            get { return 14; }
        }

        public TetrisGrid(Texture2D b, Texture2D empty)
        {
            levelGrid = new Color[Width, Height];
            gridblock = b;
            emptyBlock = empty;
            position = Vector2.Zero;
            blockPosition = new Vector2(2, 2);
            timeToMove = 0.25d;
            
            activeBlock = RandomBlock();
            Reset();
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

        public void Reset()
        {
            Clear();
            ResetActiveBlock();
            gameOver = false;
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
                    if (levelGrid[i,j] == Color.White)
                        s.Draw(emptyBlock, new Vector2(i * gridblock.Width, j * gridblock.Height), Color.Gray);
                    else
                        s.Draw(gridblock, new Vector2(i * gridblock.Width, j * gridblock.Height), levelGrid[i, j]);       
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            moveTimer += gameTime.ElapsedGameTime.TotalSeconds;
            
            putBlockInGrid();


            if (moveTimer > timeToMove) //if it's time to drop
            {
                moveTimer = 0;
                if (canMove((int)blockPosition.X, (int)blockPosition.Y + 1))
                {
                    removeBlockFromGrid();
                    blockPosition.Y += 1;
                    putBlockInGrid();
                }
                else
                {
                    if (blockPosition.Y == 0) //if we can't move and the block y is 0 then we lose
                    {
                        gameOver = true; //the gameworld checks if this variable is true to set the gameState accordingly
                    }
                    else
                    {
                        blockCount++;
                        ResetActiveBlock();
                    }
                }
            }

            checkRows(); //checks if there are any full rows and shifts the gamefield down if there is one or more full rows
            timeToMove = 0.25d - (blockCount / 2)*0.05;
        }

        public void checkRows()
        {
            for (int j = 0; j < Height; j++)
            {
                bool isRowFull = true;
                for (int i = 0; i < Width; i++)
                {
                    if (levelGrid[i, j] == Color.White)
                    {
                        isRowFull = false; //if one of the cells is not coloured than the row is not full
                        break; //we don't need to check if any other cells aren't full either
                    }
                }

                if (isRowFull) //shift array down 
                {
                    for (int w = j; w >= 0; w--)
                    {
                        for (int v = 0; v < Width; v++)
                        {
                            if (w != 0)
                            {
                                levelGrid[v, w] = levelGrid[v, w - 1];
                            }
                            else
                            {
                                levelGrid[v, w] = Color.White;
                            }

                        }
                    }
                }
            }
        }

        public void putBlockInGrid()
        {
            for (int j = 0; j < activeBlock.arraySize; j++)
            {
                for (int i = 0; i < activeBlock.arraySize; i++)
                {
                    if (activeBlock.blockArray[i, j]) //is there a block in the shape
                    {
                        levelGrid[i + (int)blockPosition.X, j + (int)blockPosition.Y] = activeBlock.blockColor;
                    }
                }
            }
        }

        public void removeBlockFromGrid()
        {
            for (int j = 0; j < activeBlock.arraySize; j++)
            {
                for (int i = 0; i < activeBlock.arraySize; i++)
                {
                    if (activeBlock.blockArray[i, j]) //is there a block in the shape
                    {
                        levelGrid[i + (int)blockPosition.X, j + (int)blockPosition.Y] = Color.White;
                    }
                }
            }
        }

        public bool canMove(int x, int y)
        {
            removeBlockFromGrid();

            for (int j = 0; j < activeBlock.arraySize; j++)
            {
                for (int i = 0; i < activeBlock.arraySize; i++)
                {
                    if (activeBlock.blockArray[i, j]) //is there a block in the tetrisblock array?
                    {
                        if (x + i < Width && x + i >= 0 && y + j < Height) //is it within bounds?
                        {
                            if (levelGrid[x + i, y + j] != Color.White) //is there a block already there
                            {
                                putBlockInGrid();
                                return false;
                            }
                        } else
                        {
                            putBlockInGrid();
                            return false;
                        }
                        
                    }
                }
            }

            return true;
        }

        public bool canRotate()
        {
            bool[,] temp = activeBlock.blockArray;

            removeBlockFromGrid();
            activeBlock.RotateCW();
            
            if (canMove((int)blockPosition.X, (int)blockPosition.Y))
            {
                activeBlock.blockArray = temp;
                return true;
            }
            else
            {
                activeBlock.blockArray = temp;
                return false;
            }

        }



        public void HandleInput(GameTime gameTime, InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.Left, false))
            {
                if (canMove((int)blockPosition.X-1, (int)blockPosition.Y))
                {
                    blockPosition.X--;
                }
            
            }
            if (inputHelper.KeyPressed(Keys.Right, false))
            {
                if (canMove((int)blockPosition.X + 1, (int)blockPosition.Y))
                {
                    blockPosition.X++;
                }

            }
            if (inputHelper.KeyPressed(Keys.Up, false))
            {
                if (canRotate())
                {
                    activeBlock.RotateCW();

                }
            }

            /*
            if (inputHelper.IsKeyDown(Keys.Down))
            {
                timeToMove = 0.10d;
            }
            else 
            {
                timeToMove = 0.25d;
            }
            */
        }

        public void ResetActiveBlock()
        {
            activeBlock = nextBlock;
            nextBlock = RandomBlock();
            blockPosition.Y = 0;
            blockPosition.X = Width/2-2;
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
