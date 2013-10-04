using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class SBlock : TetrisBlock
    {
        public SBlock(Texture2D tex) : base(tex)
        {
            arraySize = 3;
            blockColor = Color.Blue;
            blockArray = new bool[3,3] {
            {false, false, false},
            {false, true, true},
            {true, true, false}};
        }
    }
}
