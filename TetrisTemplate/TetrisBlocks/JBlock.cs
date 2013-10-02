using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class LBlock : TetrisBlock
    {
        public LBlock(Texture2D tex) : base(tex)
        {
            blockColor = Color.Yellow;
            blockArray = new bool[4,4] {
            {false, false, false, false},
            {false, true, false, false},
            {false, true, false, false},
            {false, true, true, false}};
        }
    }
}
