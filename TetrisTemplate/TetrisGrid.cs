using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;

/*
 * a class for representing the Tetris playing grid
 */
namespace TetrisPrac
{
    class TetrisGrid
    {
        Texture2D gridBlockTex, emptyBlockTex; //textures for blocks

        SoundEffect hitSound, rowSound;

        float smallBlockScale, gameScale; //small block scale is the scale of the smaller blocks that show the next block and gameScale is multiplied by our tile width to fit more blocks in the same HUD

        Color[,] levelGrid; //array that represents our grid

        Vector2 blockPosition;

        Vector2 gridOffset, nextBlockOffset; //offset for where to draw on the HUD

        TetrisBlock activeBlock, nextBlock;

        protected double moveTimer, timeToMove, startTime; //using an incrementing counter that resets to 0 every time once it's higher than timeToMove

        Dictionary<Type, int> amountDictionary; //this stores the amount of each dropped block to calculate the suspicion

        bool hasReachedTop; //this is used to signal the gameworld that player1 has reached the top

        float suspicionValue;

        int blockCount, rowsCleared;

        public int Width
        {
            get { return 11; }
        }

        public int Height
        {
            get { return 14; }
        }

        public float SuspicionValue
        {
            get { return suspicionValue; }
        }

        public bool HasReachedTop
        {
            get { return hasReachedTop; }
        }

        public TetrisGrid(ContentManager content)
        {
            levelGrid = new Color[Width, Height];
            gridBlockTex = content.Load<Texture2D>("Block");
            emptyBlockTex = content.Load<Texture2D>("EmptyBlock");

            gridOffset = new Vector2(121, 40);
            nextBlockOffset = new Vector2(600, 65);

            smallBlockScale = 0.5f;
            gameScale = 0.85f;

            //position = Vector2.Zero;
            blockPosition = new Vector2(2, 2);

            startTime = 0.40d;

            hitSound = content.Load<SoundEffect>("BlockLanding");
            rowSound = content.Load<SoundEffect>("RowClear");
            activeBlock = RandomBlock();
            Reset();
        }

        public void Update(GameTime gameTime)
        {
            moveTimer += gameTime.ElapsedGameTime.TotalSeconds;

            suspicionValue -= 0.00005f;
            suspicionValue = Math.Max(0, suspicionValue);

            putBlockInGrid();

            if (moveTimer > timeToMove) //if it's time to drop
            {
                moveTimer = 0;


                if (canMove((int)blockPosition.X, (int)blockPosition.Y + 1, false))
                {
                    removeBlockFromGrid();
                    blockPosition.Y += 1;
                }
                else
                {
                    if (blockPosition.Y == 0) //if we can't move and the block y is 0 then we lose
                    {
                        hasReachedTop = true; //the gameworld checks if this variable is true to set the gameState accordingly
                    }
                    else
                    {
                        blockCount++;
                        hitSound.Play();
                        suspicionValue += 0.05f;
                        calculateSuspicion(); //function that updates the suspicion value
                        ResetActiveBlock();
                        checkRows(); //checks if there are any full rows and shifts the gamefield down if there is one or more full rows
                    }
                }
            }

        }

        
        public void Draw(GameTime gameTime, SpriteBatch s)
        {
            for (int j = 0; j < Height; j++) //draws the main grid
            {
                for (int i = 0; i < Width; i++)
                {
                    if (levelGrid[i, j] == Color.White)
                        s.Draw(emptyBlockTex, new Vector2(i * emptyBlockTex.Width, j * emptyBlockTex.Height) * gameScale + gridOffset, null, Color.White, 0f, Vector2.Zero, gameScale, SpriteEffects.None, 0);
                    else
                        s.Draw(gridBlockTex, new Vector2(i * gridBlockTex.Width, j * gridBlockTex.Height) * gameScale + gridOffset, null, levelGrid[i, j], 0f, Vector2.Zero, gameScale, SpriteEffects.None, 0);
                }
            }

            Vector2 halfOffset = new Vector2(nextBlock.arraySize * gridBlockTex.Width * smallBlockScale, nextBlock.arraySize * gridBlockTex.Height * smallBlockScale) / 2; //the offset/origin for the small next block

            for (int i = 0; i < nextBlock.arraySize; i++) //draws the next block
            {
                for (int j = 0; j < nextBlock.arraySize; j++)
                {
                    
                    if (nextBlock.blockArray[i,j])
                        s.Draw(gridBlockTex, new Vector2(i * gridBlockTex.Width * smallBlockScale, j * gridBlockTex.Height * smallBlockScale) + nextBlockOffset, null, nextBlock.blockColor, 0f, halfOffset, smallBlockScale, SpriteEffects.None, 0);
                }
            }
        }

        public void checkRows()
        {
            bool hasPlayedSound = false;

            for (int j = 0; j < Height; j++)
            {
                bool isRowFull = true;
                for (int i = 0; i < Width; i++)
                {
                    if (levelGrid[i, j] == Color.White)
                    {
                        isRowFull = false; //if one of the cells is not coloured then the row is not full
                        break; //we don't need to check if any other cells aren't full either
                    }
                }

                if (isRowFull) //shift array down 
                {
                    if (!hasPlayedSound)
                    {
                        hasPlayedSound = true;
                        rowSound.Play();
                    }
                    rowsCleared++;
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


        public bool canMove(int x, int y, bool rotate) //checks if the active block can be on the position x and y
        {
            if (!rotate) // we don't want to remove the rotated block from the grid if it was just rotated
            {
                removeBlockFromGrid();
            }

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
                                if (!rotate) //if we fail, we don't want to put the rotated block back in the grid
                                {
                                    putBlockInGrid();
                                }
                                return false;
                            }
                        }
                        else
                        {
                            if (!rotate)
                            {
                                putBlockInGrid();
                            }
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

            if (canMove((int)blockPosition.X, (int)blockPosition.Y, true))
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

        public void putBlockInGrid() //puts the active block into the grid
        {
            for (int j = 0; j < activeBlock.arraySize; j++)
            {
                for (int i = 0; i < activeBlock.arraySize; i++)
                {
                    if (i + blockPosition.X < Width && i + blockPosition.X >= 0 && j + blockPosition.Y < Height) //is it within bounds?
                    {
                        if (activeBlock.blockArray[i, j]) //is there a block in the shape
                        {
                            levelGrid[i + (int)blockPosition.X, j + (int)blockPosition.Y] = activeBlock.blockColor;
                        }
                    }
                }
            }
        }

        public void removeBlockFromGrid() //removes active block from grid
        {
            for (int j = 0; j < activeBlock.arraySize; j++)
            {
                for (int i = 0; i < activeBlock.arraySize; i++)
                {
                    if (i + blockPosition.X < Width && i + blockPosition.X >= 0 && j + blockPosition.Y < Height) //is it within bounds?
                    {
                        if (activeBlock.blockArray[i, j]) //is there a block in the shape
                        {
                            levelGrid[i + (int)blockPosition.X, j + (int)blockPosition.Y] = Color.White;
                        }
                    }
                }
            }
        }



        public void HandleInput(GameTime gameTime, InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.A, false))
            {
                if (canMove((int)blockPosition.X-1, (int)blockPosition.Y, false))
                {
                    blockPosition.X--;
                }
            }

            if (inputHelper.KeyPressed(Keys.D, false))
            {
                if (canMove((int)blockPosition.X + 1, (int)blockPosition.Y, false))
                {
                    blockPosition.X++;
                }

            }

            if (inputHelper.KeyPressed(Keys.W, false))
            {
                if (canRotate())
                {
                    activeBlock.RotateCW();
                    putBlockInGrid();
                }
            }

            if (inputHelper.IsKeyDown(Keys.S))
            {
                if (timeToMove >= 0.10d) //so we can't actually slow down if we fall faster than the fast speed
                    timeToMove = 0.10d;
            }
            else
            {
                timeToMove = startTime - (blockCount / 10) * 0.035; //increasing difficulty
            }

            IlluminatiInput(inputHelper); //checks all the numpad keys to change the nextBlock
        }

        public void IlluminatiInput(InputHelper inputHelper)
        {
            if (inputHelper.KeyPressed(Keys.NumPad1, false))
            {
                nextBlock = new TallBlock(gridBlockTex);
            }

            if (inputHelper.KeyPressed(Keys.NumPad2, false))
            {
                nextBlock = new JBlock(gridBlockTex);
            }

            if (inputHelper.KeyPressed(Keys.NumPad3, false))
            {
                nextBlock = new LBlock(gridBlockTex);
            }

            if (inputHelper.KeyPressed(Keys.NumPad4, false))
            {
                nextBlock = new TBlock(gridBlockTex);
            }

            if (inputHelper.KeyPressed(Keys.NumPad5, false))
            {
                nextBlock = new SBlock(gridBlockTex);
            }

            if (inputHelper.KeyPressed(Keys.NumPad6, false))
            {
                nextBlock = new ZBlock(gridBlockTex);
            }

            if (inputHelper.KeyPressed(Keys.NumPad7, false))
            {
                nextBlock = new SquareBlock(gridBlockTex);
            }
        }

       
        public void calculateSuspicion()
        {
            if (!amountDictionary.ContainsKey(activeBlock.GetType()))
            {
                amountDictionary.Add(activeBlock.GetType(), 0);
            }

            amountDictionary[activeBlock.GetType()]++;

            List<Type> keyList = new List<Type>(amountDictionary.Keys);

            float averageOccurance = (float)blockCount / 7; // divide by seven because we have seven block types
            averageOccurance = Math.Max(averageOccurance, 1);
            
            suspicionValue = rowsCleared/8; //the more rows there were cleared, the higher suspicion

            foreach (Type k in keyList)
            {
                if (amountDictionary[k] > averageOccurance)
                {
                    suspicionValue += (amountDictionary[k] - averageOccurance) * 0.14f;
                }
            }
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
            amountDictionary = new Dictionary<Type, int>(); //reset suspicion dictionary
            suspicionValue = 0f;
            timeToMove = 0.25d;
            blockCount = 0;
            rowsCleared = 0;
            Clear();
            ResetActiveBlock();
            hasReachedTop = false;
        }

        public void ResetActiveBlock()
        {
            activeBlock = nextBlock;
            nextBlock = RandomBlock();
            blockPosition.Y = 0;
            blockPosition.X = Width / 2 - 2;
        }



        public TetrisBlock RandomBlock()
        {
            int randomValue = GameWorld.Random.Next(7);
            switch (randomValue)
            {
                case 0:
                    return new JBlock(gridBlockTex);
                case 1:
                    return new LBlock(gridBlockTex);
                case 2:
                    return new SBlock(gridBlockTex);
                case 3:
                    return new SquareBlock(gridBlockTex);
                case 4:
                    return new TallBlock(gridBlockTex);
                case 5:
                    return new TBlock(gridBlockTex);
                case 6:
                    return new ZBlock(gridBlockTex);
            }
            return new LBlock(gridBlockTex);
        }

    }
}
