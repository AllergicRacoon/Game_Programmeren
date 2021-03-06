﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TetrisPrac
{
    class LBlock : TetrisBlock
    {
        public LBlock(Texture2D tex) : base(tex)
        {
            arraySize = 3;
            blockColor = Color.Red;
            blockArray = new bool[3,3] {
            {false, true, false},
            {false, true, false},
            {false, true, true}};
        }
    }
}
