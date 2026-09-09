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
    internal class Weapon
    {
        // list that stores all bullets fired by this weapon
        private List<Projectile> bullets;

        // texture used for all bullets
        private Texture2D bulletTexture;

        // constructor
        // sets the bullet texture and creates the bullet list
        public Weapon(Texture2D texture)
        {
            bulletTexture = texture;
            bullets = new List<Projectile>();
        }

        // getter for the bullets list
        // used for collision checks in Game1
        public List<Projectile> getBullets()
        {
            return bullets;
        }

        // Fire method
        // position = where the bullet starts
        // direction = which way the bullet travels
        // playSound = true only for the player, not enemies
        public void Fire(Point position, Vector2 direction, bool playSound)
        {
            // if no direction is given, shoot to the right by default
            if (direction == Vector2.Zero)
                direction = new Vector2(1, 0);

            // normalize so speed stays consistent
            direction.Normalize();

            // create a new projectile and add it to the list
            bullets.Add(new Projectile(
                bulletTexture,
                new Rectangle(position.X - 4, position.Y - 4, 8, 8),
                direction * 6f));

            // play shooting sound only if this is the player weapon
            if (playSound && Game1.ShootSound != null)
                Game1.ShootSound.Play(0.5f, 0f, 0f);
        }

        // Update method
        // moves bullets and removes them if they leave the screen
        public void Update(GameTime gameTime)
        {
            // loop backwards so bullets can be removed safely
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gameTime);

                // remove bullet if it goes off screen
                if (bullets[i].getBounds().Right < 0 ||
                    bullets[i].getBounds().Left > Game1.ScreenWidth ||
                    bullets[i].getBounds().Bottom < 0 ||
                    bullets[i].getBounds().Top > Game1.ScreenHeight)
                {
                    bullets.RemoveAt(i);
                }
            }
        }

        // Draw method
        // draws all bullets on the screen
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < bullets.Count; i++)
            {
                bullets[i].Draw(spriteBatch);
            }
        }
    }
}
