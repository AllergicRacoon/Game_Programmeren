using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class SquareBlock : TetrisBlock
    {
        public SquareBlock(Texture2D tex) : base(tex)
        {
            arraySize = 4;
            blockColor = Color.Purple;
            blockArray = new bool[4,4] {
            {false, false, false, false},
            {false, true, true, false},
            {false, true, true, false},
            {false, false, false, false}};
        }
    }
}
