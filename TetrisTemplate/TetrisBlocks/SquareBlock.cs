using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class SquareBlock : TetrisBlock
    {
        public SquareBlock(Texture2D tex) : base(tex)
        {
            arraySize = 2;
            blockColor = Color.Purple;
            blockArray = new bool[2,2] {
            {true, true},
            {true, true}};
        }
    }
}
