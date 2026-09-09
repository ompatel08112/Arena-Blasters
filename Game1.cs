// Om Patel
// ICS 4U1 - Morning
// 19 January, 2026
// Final Video Game

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Final_Game___Om
{
    // this is the main game class that MonoGame runs
    public class Game1 : Game
    {
        // manages the graphics settings like window size
        GraphicsDeviceManager graphics;
        // used to draw textures and text to the screen
        SpriteBatch spriteBatch;

        // a random number generator for things like making power-ups appear in random places
        public static System.Random random = new System.Random();

        // the width of the game screen in pixels (constant), I learnt it from here: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/const
        public const int ScreenWidth = 800;
        // the height of the game screen in pixels (constant)
        public const int ScreenHeight = 480;

        // stores the main player's position so the enemy can aim at them
        public static Vector2 PlayerOnePosition = Vector2.Zero;

        // reference to the shooting sound effect, shared so other classes can use it
        public static SoundEffect ShootSound;

        // reference to the 1x1 pixel texture used to draw health bars
        public static Texture2D HealthBarTexture;

        // the font we use to draw HUD text on screen
        SpriteFont font;

        // background image shown behind everything
        Texture2D background;

        // four player facing textures: up, down, left, right
        Texture2D playerUp;
        Texture2D playerDown;
        Texture2D playerLeft;
        Texture2D playerRight;

        // enemy sprite texture
        Texture2D enemyTexture;
        // bullet sprite texture
        Texture2D bulletTexture;
        // power-up sprite texture
        Texture2D powerUpTexture;

        // list holding all player objects (1 or more)
        List<Player> players;
        // single enemy object used in single-player mode
        Enemy enemy;
        // list holding power-up objects currently on the map
        List<PowerUp> powerUps;

        // flag that tells the game whether it is multiplayer (true) or single-player (false)
        bool multiplayerMode;

        // flag that becomes true when the game is over (someone won or lost)
        bool gameOver;
        // text message to show at the end of the game (You Win, You Lose, Winner: Player X)
        string endMessage;

        // how often a new power-up should spawn (in seconds)
        const float PowerUpInterval = 20f;
        // timer that counts down to the next power-up spawn
        float powerUpTimer = PowerUpInterval;
        // power-up drawing size (width)
        const int PowerUpWidth = 30;
        // power-up drawing size (height)
        const int PowerUpHeight = 30;

        // constructor runs when the Game1 object is created
        public Game1()
        {
            // create the graphics manager to control window size and device
            graphics = new GraphicsDeviceManager(this);
            // tell the game where to look for the content (images, sounds)
            Content.RootDirectory = "Content";

            // set the game window size using the constants above
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
        }

        // called once when the game starts; use it to set up lists and default values
        protected override void Initialize()
        {
            // create empty lists for players and power-ups
            players = new List<Player>();
            powerUps = new List<PowerUp>();

            // make sure game over starts false and message is empty
            gameOver = false;
            endMessage = "";

            // start the power-up timer so the first spawn happens after PowerUpInterval seconds
            powerUpTimer = PowerUpInterval;

            // call the base class initialize (required)
            base.Initialize();
        }

        // called after Initialize; load textures, fonts and sounds here
        protected override void LoadContent()
        {
            // create the SpriteBatch used to draw textures and text
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load the background image from the Content folder
            background = Content.Load<Texture2D>("background");

            // load the four player direction images from the Content folder
            playerUp = Content.Load<Texture2D>("UpPlayer");
            playerDown = Content.Load<Texture2D>("DownPlayer");
            playerLeft = Content.Load<Texture2D>("LeftPlayer");
            playerRight = Content.Load<Texture2D>("RightPlayer");

            // load enemy, bullet and power-up sprites
            enemyTexture = Content.Load<Texture2D>("enemy");
            bulletTexture = Content.Load<Texture2D>("bulletsRed");
            powerUpTexture = Content.Load<Texture2D>("powerup");

            // load the shooting sound effect and store it in the shared ShootSound variable
            ShootSound = Content.Load<SoundEffect>("shoot");

            // load a 1x1 pixel texture used to draw health bars (stretched to any size)
            HealthBarTexture = Content.Load<Texture2D>("onebyonepixel(2)");

            // load the font named "File" from the Content project for HUD text
            font = Content.Load<SpriteFont>("Font");

            // make sure the lists are empty and ready
            players = new List<Player>();
            powerUps = new List<PowerUp>();

            // count how many controllers are connected (up to 4)
            int connected = 0;
            for (int i = 0; i < 4; i++)
            {
                if (GamePad.GetState((PlayerIndex)i).IsConnected)
                    connected++;
            }

            // set multiplayerMode to true if more than one controller is connected
            multiplayerMode = connected > 1;

            // create the first player with its direction textures, starting rectangle, controller index and bullet texture
            Player p1 = new Player(playerUp, playerDown, playerLeft, playerRight, new Rectangle(200, 100, 40, 40), PlayerIndex.One, bulletTexture);
            // set the spawn point where player will re-appear after losing health or lives
            p1.SetSpawn(new Point(200, 100));
            // set the simple player number used for the HUD text
            p1.setPlayerNumber(1);
            // add the new player to the players list
            players.Add(p1);

            // if two controllers are connected, create a second player the same way
            if (multiplayerMode)
            {
                Player p2 = new Player(playerUp, playerDown, playerLeft, playerRight, new Rectangle(500, 100, 40, 40), PlayerIndex.Two, bulletTexture);
                p2.SetSpawn(new Point(500, 100));
                p2.setPlayerNumber(2);
                players.Add(p2);
                // if 3 controllers are connected load the 3rd one
                if (connected == 3)
                {
                    Player p3 = new Player(playerUp, playerDown, playerLeft, playerRight, new Rectangle(200, 400, 40, 40), PlayerIndex.Three, bulletTexture);
                    p3.SetSpawn(new Point(200, 400));
                    p3.setPlayerNumber(3);
                    players.Add(p3);
                }
                // if 4 controllers are connected load the 4th one
                if (connected == 4)
                {
                    Player p4 = new Player(playerUp, playerDown, playerLeft, playerRight, new Rectangle(500, 400, 40, 40), PlayerIndex.Four, bulletTexture);
                    p4.SetSpawn(new Point(500, 400));
                    p4.setPlayerNumber(2);
                    players.Add(p4);
                }
            }
            
            else
            {
                // if not multiplayer, create a single Enemy for versus mode
                enemy = new Enemy(enemyTexture, new Rectangle(500, 200, 40, 40), bulletTexture);
                enemy.SetSpawn(new Point(500, 200));
            }

            // add one initial power-up on the map so there is something to pick up at start
            powerUps.Add(new PowerUp(powerUpTexture, new Rectangle(300, 150, PowerUpWidth, PowerUpHeight)));
        }

        // called every frame; main game logic goes here
        protected override void Update(GameTime gameTime)
        {
            // check if the Back button on controller was pressed to quit the game
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            // if the game is not over, update players, enemy and spawn power-ups
            if (!gameOver)
            {
                // call Update on each player so they handle input and movement
                for (int i = 0; i < players.Count; i++)
                    players[i].Update(gameTime);

                // if single-player, update the enemy AI so it moves and shoots
                if (!multiplayerMode && enemy != null)
                    enemy.Update(gameTime);

                // count down the power-up spawn timer by the time that passed since last frame
                powerUpTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
                // when timer reaches zero or below, spawn a new power-up
                if (powerUpTimer <= 0f)
                {
                    // choose a random x position so the power-up stays fully on screen
                    int px = random.Next(0, ScreenWidth - PowerUpWidth);
                    // choose a random y position so the power-up stays fully on screen
                    int py = random.Next(0, ScreenHeight - PowerUpHeight);

                    // add a new power-up at the random position
                    powerUps.Add(new PowerUp(powerUpTexture, new Rectangle(px, py, PowerUpWidth, PowerUpHeight)));

                    // reset the timer to spawn again after the interval
                    powerUpTimer = PowerUpInterval;
                }
            }

            // update the PlayerOnePosition helper so the enemy can aim at player 1
            if (players.Count > 0)
                PlayerOnePosition = players[0].getBounds().Center.ToVector2();
            else
                PlayerOnePosition = Vector2.Zero;

            // loop backwards through powerUps so we can remove items safely while iterating
            for (int pu = powerUps.Count - 1; pu >= 0; pu--)
            {
                // flag to mark whether we removed this power-up
                bool removed = false;

                // check each player to see if they touched the power-up
                for (int p = 0; p < players.Count; p++)
                {
                    // if the power-up rectangle intersects the player's rectangle
                    if (powerUps[pu].getBounds().Intersects(players[p].getBounds()))
                    {
                        // if the player is not at full health, heal them
                        if (!players[p].IsAtMaxHealth())
                        {
                            players[p].Heal(25); // heal 25 HP
                        }
                        else
                        {
                            // if health is full, give a speed boost for 10 seconds
                            players[p].AddSpeedBoost(10f, 2f); // 10 seconds, 2x speed
                        }

                        // remove the power-up from the list so it disappears
                        powerUps.RemoveAt(pu);
                        removed = true;
                        break;
                    }
                }

                // if we removed it because a player picked it up, skip enemy check
                if (removed) continue;

                // allow the enemy to pick up power-ups in single-player mode
                if (!multiplayerMode && enemy != null)
                {
                    if (powerUps[pu].getBounds().Intersects(enemy.getBounds()))
                    {
                        // same rules for enemy: heal if below max, otherwise speed boost
                        if (!enemy.IsAtMaxHealth())
                        {
                            enemy.Heal(25);
                        }
                        else
                        {
                            enemy.AddSpeedBoost(10f, 2f);
                        }

                        // remove the picked-up power-up
                        powerUps.RemoveAt(pu);
                        continue;
                    }
                }
            }

            // handle collisions and damage: player bullets hitting enemy in single-player
            if (!multiplayerMode && enemy != null)
            {
                // get the player's weapon and its bullets list
                Weapon playerWeapon = players[0].GetWeapon();
                List<Projectile> playerBullets = playerWeapon.getBullets();

                // check each bullet from the end to the start so we can remove bullets safely
                for (int i = playerBullets.Count - 1; i >= 0; i--)
                {
                    // if this bullet hits the enemy
                    if (playerBullets[i].getBounds().Intersects(enemy.getBounds()))
                    {
                        // enemy takes damage
                        enemy.TakeDamage(25);
                        // remove the bullet from the list
                        playerBullets.RemoveAt(i);

                        // if the enemy ran out of lives, the player wins and the game ends
                        if (enemy.IsOut())
                        {
                            enemy = null;
                            gameOver = true;
                            endMessage = "You Win!";
                            break;
                        }
                    }
                }

                // enemy bullets hitting player in single-player
                if (!gameOver && enemy != null)
                {
                    // get the enemy weapon and its bullets
                    Weapon enemyWeapon = enemy.GetWeapon();
                    List<Projectile> enemyBullets = enemyWeapon.getBullets();

                    // iterate bullets backward so removal is safe
                    for (int i = enemyBullets.Count - 1; i >= 0; i--)
                    {
                        // if this enemy bullet hits player 1
                        if (enemyBullets[i].getBounds().Intersects(players[0].getBounds()))
                        {
                            // player takes damage
                            players[0].TakeDamage(25);
                            // remove the bullet
                            enemyBullets.RemoveAt(i);

                            // if the player ran out of lives, they lose and the game ends
                            if (players[0].IsOut())
                            {
                                gameOver = true;
                                endMessage = "You Lose!";
                                break;
                            }
                        }
                    }
                }
            }
            else // multiplayer mode: players can hit other players
            {
                // loop through each player to check their bullets
                for (int p = 0; p < players.Count; p++)
                {
                    // get this player's weapon and its bullet list
                    Weapon weapon = players[p].GetWeapon();
                    List<Projectile> bullets = weapon.getBullets();

                    // iterate bullets backward for safe removal
                    for (int i = bullets.Count - 1; i >= 0; i--)
                    {
                        // flag to indicate we removed a bullet this pass
                        bool removedBullet = false;

                        // check against all other players
                        for (int t = 0; t < players.Count; t++)
                        {
                            // skip checking the shooter against themselves
                            if (t == p)
                                continue;

                            // if the bullet hits another player
                            if (bullets[i].getBounds().Intersects(players[t].getBounds()))
                            {
                                // that player takes damage
                                players[t].TakeDamage(25);
                                // remove the bullet
                                bullets.RemoveAt(i);

                                // if that player ran out of lives, remove them from the game
                                if (players[t].IsOut())
                                    players.RemoveAt(t);

                                // mark that we removed this bullet and break out of the loop
                                removedBullet = true;
                                break;
                            }
                        }

                        // if bullet was removed, skip to next bullet index
                        if (removedBullet)
                            continue;
                    }
                }
            }

            // check multiplayer win condition: if only one player remains, show winner message
            if (multiplayerMode && !gameOver)
            {
                if (players.Count == 1)
                {
                    gameOver = true;
                    endMessage = "Winner: Player " + players[0].getPlayerNumber().ToString();
                }
                else if (players.Count == 0)
                {
                    // no players left, show this message
                    gameOver = true;
                    endMessage = "No players remaining!";
                }
            }

            // call the base Update (required)
            base.Update(gameTime);
        }

        // called when the game needs to draw the current frame
        protected override void Draw(GameTime gameTime)
        {
            // clear the screen to black before drawing
            GraphicsDevice.Clear(Color.Black);

            // begin drawing sprites and text
            spriteBatch.Begin();

            // draw the background image stretched to the full screen if it exists
            if (background != null)
                spriteBatch.Draw(background, new Rectangle(0, 0, ScreenWidth, ScreenHeight), Color.White);

            // draw each player and their bullets, plus a small HUD text above them
            for (int i = 0; i < players.Count; i++)
            {
                // draw the player sprite and its health bar (handled in Character.Draw)
                players[i].Draw(spriteBatch);
                // draw the player's bullets on top
                players[i].DrawBullets(spriteBatch);

                // build a small text string showing the player number and remaining lives
                string info = "P" + players[i].getPlayerNumber().ToString() + " Lives: " + players[i].getLives().ToString();
                // position the text slightly above the player
                Vector2 textPosition = new Vector2(players[i].getBounds().X, players[i].getBounds().Y - 36);
                // draw the HUD text using the loaded font
                spriteBatch.DrawString(font, info, textPosition, Color.White);
            }

            // draw the enemy and its bullets for single-player mode and show lives in top-right
            if (!multiplayerMode && enemy != null)
            {
                // draw enemy sprite and health bar
                enemy.Draw(spriteBatch);
                // draw enemy bullets
                enemy.DrawBullets(spriteBatch);

                // create text to show enemy lives and place it in the top-right
                string einfo = "Enemy Lives: " + enemy.getLives().ToString();
                Vector2 epos = new Vector2(ScreenWidth - 200, 10);
                spriteBatch.DrawString(font, einfo, epos, Color.White);
            }

            // draw each power-up that is on the map
            for (int i = 0; i < powerUps.Count; i++)
                powerUps[i].Draw(spriteBatch);

            // if the game is over, show the end message centered and a prompt to quit
            if (gameOver)
            {
                // measure how big the end text is so we can center it
                Vector2 size = font.MeasureString(endMessage);
                // calculate a centered position for the message
                Vector2 pos = new Vector2((ScreenWidth - size.X) / 2, (ScreenHeight - size.Y) / 2);
                // draw the end message in yellow
                spriteBatch.DrawString(font, endMessage, pos, Color.Yellow);

                // draw the quit prompt under the end message
                string prompt = "Press Back to Quit";
                Vector2 psize = font.MeasureString(prompt);
                Vector2 ppos = new Vector2((ScreenWidth - psize.X) / 2, pos.Y + size.Y + 8);
                spriteBatch.DrawString(font, prompt, ppos, Color.White);
            }

            // finish drawing this frame
            spriteBatch.End();

            // call base Draw (required)
            base.Draw(gameTime);
        }
    }
}
