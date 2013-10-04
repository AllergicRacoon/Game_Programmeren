using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class JBlock : TetrisBlock
    {
        public JBlock(Texture2D tex) : base(tex)
        {
            arraySize = 3;
            blockColor = Color.Yellow;
            blockArray = new bool[3, 3] {
            {false, true, false},
            {false, true, false},
            {true, true, false}};
        }
    }
}
