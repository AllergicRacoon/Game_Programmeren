using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class JBlock : TetrisBlock
    {
        public JBlock(Texture2D tex) : base(tex)
        {
            blockColor = Color.Yellow;
            blockArray = new bool[4,4] {
            {false, false, false, false},
            {false, false, true, false},
            {false, false, true, false},
            {false, true, true, false}};
        }
    }
}
