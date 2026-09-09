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
    internal class Character : MovingGameItem
    {
        // current health of the character
        protected int health;

        // maximum health the character can have
        protected int maxHealth;

        // number of lives the character has left
        protected int lives;

        // position where the character respawns after losing a life
        protected Point spawnPoint;

        // multiplier that affects movement speed (1 = normal speed)
        protected float speedMultiplier = 1f;

        // timer that controls how long a speed boost lasts
        protected float speedBoostTimer = 0f;

        // constructor: runs when a Character is created
        // sets texture, position, starting health, and lives
        public Character(Texture2D texture, Rectangle bounds, int health, int lives)
            : base(texture, bounds)
        {
            // set starting health
            this.health = health;

            // set maximum health to starting health
            this.maxHealth = health;

            // set number of lives
            this.lives = lives;

            // save the spawn position
            spawnPoint = bounds.Location;

            // start at normal speed
            speedMultiplier = 1f;

            // no speed boost active at the start
            speedBoostTimer = 0f;
        }

        // getter for current health
        // non-coders: this lets other code check how much health is left
        public int getHealth()
        {
            return health;
        }

        // getter for remaining lives
        public int getLives()
        {
            return lives;
        }

        // sets where the character will respawn after losing a life
        public void SetSpawn(Point p)
        {
            spawnPoint = p;
        }

        // checks if the character is completely out of the game
        // returns true when no lives remain
        public bool IsOut()
        {
            return lives <= 0;
        }

        // reduces health when the character gets hit
        public void TakeDamage(int amount)
        {
            // subtract damage from health
            health -= amount;

            // if health reaches zero or below
            if (health <= 0)
            {
                // remove one life
                lives--;

                // if the character still has lives left
                if (lives > 0)
                {
                    // reset health back to full
                    health = maxHealth;

                    // move character back to spawn position
                    bounds.Location = spawnPoint;

                    // stop all movement
                    velocity = Vector2.Zero;
                }
            }
        }

        // increases health when picking up a healing power-up
        public void Heal(int amount)
        {
            // add health
            health += amount;

            // prevent health from going over the maximum
            if (health > maxHealth)
                health = maxHealth;
        }

        // checks if health is already full
        public bool IsAtMaxHealth()
        {
            return health >= maxHealth;
        }

        // activates a temporary speed boost
        // seconds = how long it lasts
        // multiplier = how much faster the character moves
        public void AddSpeedBoost(float seconds, float multiplier)
        {
            speedMultiplier = multiplier;
            speedBoostTimer = seconds;
        }

        // Update method runs every frame
        // handles speed boost timing and movement
        public void Update(GameTime gameTime)
        {
            // if a speed boost is active
            if (speedBoostTimer > 0f)
            {
                // decrease the timer based on real time
                speedBoostTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                // when the timer ends
                if (speedBoostTimer <= 0f)
                {
                    // return speed back to normal
                    speedMultiplier = 1f;
                    speedBoostTimer = 0f;
                }
            }

            // move the character and keep it on screen
            // this uses the Update method from MovingGameItem
            base.Update(gameTime);
        }

        // Draw method displays the character and health bar
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw the character sprite
            base.Draw(spriteBatch);

            // draw a health bar above the character
            if (Game1.HealthBarTexture != null)
            {
                // dark red background bar showing total health
                Rectangle back = new Rectangle(bounds.X, bounds.Y - 12, bounds.Width, 6);
                spriteBatch.Draw(Game1.HealthBarTexture, back, Color.DarkRed);

                // calculate how full the health bar should be
                float ratio = 0f;
                if (maxHealth > 0)
                    ratio = (float)health / (float)maxHealth;

                // green foreground bar showing current health
                int frontWidth = (int)(bounds.Width * ratio);
                Rectangle front = new Rectangle(bounds.X, bounds.Y - 12, frontWidth, 6);
                spriteBatch.Draw(Game1.HealthBarTexture, front, Color.LimeGreen);
            }
        }


    }
}
