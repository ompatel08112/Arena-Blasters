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
    internal class Enemy : Character
    {
        // weapon used by the enemy to shoot bullets
        private Weapon weapon;

        // timers used to control movement changes and shooting rate
        private int moveTimer;
        private int fireTimer;

        // constructor
        // sets the enemy texture, position, health, lives, and bullet texture
        public Enemy(Texture2D texture, Rectangle bounds, Texture2D bulletTexture)
            : base(texture, bounds, 100, 3)
        {
            weapon = new Weapon(bulletTexture);
            moveTimer = 0;
            fireTimer = 0;
        }

        // getter for the enemy weapon
        public Weapon GetWeapon()
        {
            return weapon;
        }

        // Update method for enemy logic
        public void Update(GameTime gameTime)
        {
            // increase timers every frame
            moveTimer++;
            fireTimer++;

            // change movement direction every 60 frames
            if (moveTimer > 60)
            {
                velocity = new Vector2(
                    Game1.random.Next(-3, 4) * speedMultiplier,
                    Game1.random.Next(-3, 4) * speedMultiplier);

                moveTimer = 0;
            }

            // apply movement and screen clamping
            // also handles speed boost timing
            base.Update(gameTime);

            // enemy shoots occasionally (every 45 frames)
            if (fireTimer > 45)
            {
                Vector2 direction = Vector2.Zero;

                // aim toward player one if they exist
                if (Game1.PlayerOnePosition != Vector2.Zero)
                {
                    Vector2 from = bounds.Center.ToVector2();
                    direction = Game1.PlayerOnePosition - from;

                    // normalize direction so bullets move correctly
                    if (direction != Vector2.Zero)
                        direction.Normalize();
                }

                // fire bullet (false = enemy bullet, no sound)
                weapon.Fire(bounds.Center, direction, false);
                fireTimer = 0;
            }

            // update enemy bullets
            weapon.Update(gameTime);
        }

        // draws all bullets fired by the enemy
        public void DrawBullets(SpriteBatch spriteBatch)
        {
            weapon.Draw(spriteBatch);
        }

    }
}
