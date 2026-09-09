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
    internal class MovingGameItem : GameItem
    {
        // velocity holds how much this object moves each frame on X and Y axes
        // X = horizontal speed, Y = vertical speed (can be positive or negative)
        protected Vector2 velocity;

        // constructor: runs when we create a new MovingGameItem
        // it calls the GameItem constructor to set the image and rectangle
        // then it sets the starting speed to zero so the object is still at first
        public MovingGameItem(Texture2D texture, Rectangle bounds)
            : base(texture, bounds)
        {
            // start with no movement
            velocity = Vector2.Zero;
        }

        // simple getter for velocity using the class style you learned in class
        // non-coders: use getVelocity to ask "how fast is this object moving?"
        public Vector2 getVelocity()
        {
            return velocity;
        }

        // simple setter for velocity using the class style you learned in class
        // non-coders: use setVelocity to change how fast the object will move each frame
        public void setVelocity(Vector2 v)
        {
            velocity = v;
        }

        // Update method: called every frame to move the object and keep it on screen
        // non-coders: this method actually makes the object change position over time
        public void Update(GameTime gameTime)
        {
            // move horizontally by the X part of velocity (cast to int since bounds are integers)
            bounds.X += (int)velocity.X;
            // move vertically by the Y part of velocity
            bounds.Y += (int)velocity.Y;

            // keep the object from going off the left edge of the screen
            if (bounds.X < 0) bounds.X = 0;
            // keep the object from going off the top edge of the screen
            if (bounds.Y < 0) bounds.Y = 0;

            // if the right side of the object passes the screen width, pull it back so it fits
            if (bounds.Right > Game1.ScreenWidth)
                bounds.X = Game1.ScreenWidth - bounds.Width;

            // if the bottom of the object passes the screen height, pull it up so it fits
            if (bounds.Bottom > Game1.ScreenHeight)
                bounds.Y = Game1.ScreenHeight - bounds.Height;
        }

    }
}
