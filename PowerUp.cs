using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final_Game___Om
{
    internal class PowerUp : GameItem
    {
        // constructor
        // sets the texture and position/size of the powerup
        public PowerUp(Texture2D texture, Rectangle bounds)
            : base(texture, bounds)
        {
            // no extra setup needed
            // all logic for what the powerup does is handled in Game1
        }

    }
}