using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class TallBlock : TetrisBlock
    {
        public TallBlock(Texture2D tex) : base(tex)
        {
            blockColor = Color.Pink;
            blockArray = new bool[4,4] {
            {false, true, false, false},
            {false, true, false, false},
            {false, true, false, false},
            {false, true, false, false}};
        }
    }
}
