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
    internal class GameItem
    {
        // the image used to draw this object on screen
        protected Texture2D texture;
        // the position and size of this object on screen (x, y, width, height)
        protected Rectangle bounds;

        // constructor: this code runs when we create a new GameItem
        // it remembers the texture and the rectangle we give it
        public GameItem(Texture2D texture, Rectangle bounds)
        {
            this.texture = texture;
            this.bounds = bounds;
        }

        // plain getter method that returns the rectangle where this item is placed
        // non-coders: "getBounds" lets other code ask "where is this object?"
        public Rectangle getBounds()
        {
            return bounds;
        }

        // simple setter method that updates this object's rectangle
        // non-coders: "setBounds" lets other code move or resize this object
        public void setBounds(Rectangle b)
        {
            bounds = b;
        }

        // Update method: reserved for logic that runs every frame (left empty here)
        // non-coders: this is where you'd put code to change the object over time
        public void Update(GameTime gameTime)
        {
        }

        // Draw method: actually draws the texture at the rectangle on the screen
        // non-coders: this is how the object becomes visible each frame
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, bounds, Color.White);
        }

    }
}
