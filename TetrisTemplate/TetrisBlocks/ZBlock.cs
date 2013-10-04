using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class ZBlock : TetrisBlock
    {
        public ZBlock(Texture2D tex) : base(tex)
        {
            arraySize = 3;
            blockColor = Color.Orange;
            blockArray = new bool[3,3] {
            {false, false, false},
            {true, true, false},
            {false, true, true}};
        }
    }
}
