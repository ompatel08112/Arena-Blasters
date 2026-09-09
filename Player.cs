using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Final_Game___Om
{
    internal class Player : Character
    {
        // which controller this player is using (PlayerOne, PlayerTwo, etc.)
        private PlayerIndex playerIndex;

        // weapon used by the player to shoot bullets
        private Weapon weapon;

        // current and previous gamepad states
        // used to detect button presses instead of holding
        private GamePadState pad;
        private GamePadState oldPad;

        // textures for each facing direction
        private Texture2D upTexture;
        private Texture2D downTexture;
        private Texture2D leftTexture;
        private Texture2D rightTexture;

        // direction the player is currently facing
        // used to decide bullet direction
        private Vector2 facingDirection;

        // player number (useful for UI or multiplayer logic)
        private int playerNumber;

        // constructor
        // sets up textures, starting position, controller index, and weapon
        public Player(
            Texture2D up,
            Texture2D down,
            Texture2D left,
            Texture2D right,
            Rectangle bounds,
            PlayerIndex index,
            Texture2D bulletTexture)
            : base(right, bounds, 100, 3)
        {
            // store direction textures
            upTexture = up;
            downTexture = down;
            leftTexture = left;
            rightTexture = right;

            // start facing right
            texture = rightTexture;
            facingDirection = new Vector2(1, 0);

            // store controller index
            playerIndex = index;

            // create the weapon with the bullet texture
            weapon = new Weapon(bulletTexture);

            // save the initial gamepad state
            oldPad = GamePad.GetState(playerIndex);

            // default player number
            playerNumber = 1;
        }

        // setter for player number
        public void setPlayerNumber(int n)
        {
            playerNumber = n;
        }

        // getter for player number
        public int getPlayerNumber()
        {
            return playerNumber;
        }

        // gives access to the player's weapon
        public Weapon GetWeapon()
        {
            return weapon;
        }

        // Update runs every frame
        // handles movement, facing direction, shooting, and bullets
        public void Update(GameTime gameTime)
        {
            // get the current state of the controller
            pad = GamePad.GetState(playerIndex);

            // movement using left thumbstick
            // speedMultiplier comes from Character (for speed boosts)
            Vector2 baseSpeed = new Vector2(5f * speedMultiplier, -5f * speedMultiplier);
            velocity = pad.ThumbSticks.Left * baseSpeed;

            // update character logic
            // this handles speed boost timers and screen boundaries
            base.Update(gameTime);

            // determine facing direction using right thumbstick
            // small values are ignored to prevent jitter
            if (pad.ThumbSticks.Right.X > 0.3f)
            {
                texture = rightTexture;
                facingDirection = new Vector2(1, 0);
            }
            else if (pad.ThumbSticks.Right.X < -0.3f)
            {
                texture = leftTexture;
                facingDirection = new Vector2(-1, 0);
            }
            else if (pad.ThumbSticks.Right.Y > 0.3f)
            {
                texture = upTexture;
                facingDirection = new Vector2(0, -1);
            }
            else if (pad.ThumbSticks.Right.Y < -0.3f)
            {
                texture = downTexture;
                facingDirection = new Vector2(0, 1);
            }

            // shooting using the right trigger
            // only fires once per press, not while holding
            if (pad.Triggers.Right > 0.5f && oldPad.Triggers.Right <= 0.5f)
            {
                // true means this is a player bullet (not enemy)
                weapon.Fire(bounds.Center, facingDirection, true);
            }

            // update all bullets
            weapon.Update(gameTime);

            // save current state as old state for next frame
            oldPad = pad;
        }

        // draws only the bullets fired by this player
        public void DrawBullets(SpriteBatch spriteBatch)
        {
            weapon.Draw(spriteBatch);
        }


    }
}
