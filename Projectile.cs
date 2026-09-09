using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Final_Game___Om
{
    internal class Projectile : MovingGameItem
    {
        // constructor
        // sets the bullet texture, starting position, and movement speed
        public Projectile(Texture2D texture, Rectangle bounds, Vector2 velocity)
            : base(texture, bounds)
        {
            // store the velocity passed in
            // this controls the direction and speed of the bullet
            this.velocity = velocity;
        }

        // Update method for the projectile
        // bullets move freely and are NOT clamped to the screen
        public void Update(GameTime gameTime)
        {
            // move the bullet based on its velocity
            bounds.X += (int)velocity.X;
            bounds.Y += (int)velocity.Y;
        }
    }
}
