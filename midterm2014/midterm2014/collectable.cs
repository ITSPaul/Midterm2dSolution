using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sprites;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace midterm2014
{
    class collectable : SimpleSprite
    {
        int _value;

        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        Color tint;

        public collectable(Texture2D tx, Vector2 position)
            : base(tx, position)
        {
            Random r = new Random();
            _value = r.Next(10, 30);
            if (_value > 20)
                tint = Color.Aqua;
            else tint = Color.White;
        }

        public void draw(SpriteBatch sp)
        {
            if (Visible)
                sp.Draw(Image, Position, tint);
        }
    }
}
