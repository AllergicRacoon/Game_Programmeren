using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class TBlock : TetrisBlock
    {
        public TBlock(Texture2D tex) : base(tex)
        {
            arraySize = 3;
            blockColor = Color.Green;
            blockArray = new bool[3,3] {
            {false, false, false},
            {true, true, true},
            {false, true, false}};
        }
    }
}
