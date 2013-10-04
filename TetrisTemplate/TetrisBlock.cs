using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class TetrisBlock
    {
        public bool[,] blockArray;
        protected Texture2D blockTex;
        public Color blockColor;
        public int arraySize;

        public TetrisBlock(Texture2D tex)
        {
            blockTex = tex;
            arraySize = 3;
            blockArray = new bool[arraySize, arraySize];
        }


        public void RotateCW()
        {
            bool[,] temp = new bool[arraySize, arraySize];
            for (int i = 0; i < arraySize; i++)
            {
                for (int j = 0; j < arraySize; j++)
                {
                    temp[i, j] = blockArray[j, arraySize - i - 1];
                }
            }
            blockArray = temp;

            
        }

        
    }
}
