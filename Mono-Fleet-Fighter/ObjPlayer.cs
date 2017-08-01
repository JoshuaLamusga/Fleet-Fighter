using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleXnaFramework;
using System;

namespace FleetFighter
{
    /// <summary>
    /// Controls all of the main aspects of the player during gameplay
    /// </summary>
    public class ObjPlayer
    {
        public MainLoop game;
        public Texture2D texPlayer; //The texture of the player.
        public Sprite sprite; //The sprite for the player.
        public SpritePhysics spritePhysics; //The physics for the sprite.

        public int missileNum = 2; //The number of missiles available.

        public int shootingDelay = 10; //The delay in ticks between bullets.
        public bool canShoot = false; //Player cannot shoot normal bullets when false.

        public float speed = 0.4f; //The current speed of the player.
        public float friction = 0.2f; //The friction on movement.

        public Vector2 pos, posOld; //Controls the coordinates.

        public float hp = 100; //The current health value.
        public int score = 0; //The score (used for highscores).

        public int healthDuration = 120; //How long health is visible when hit.
        public int healthVisible = 0; //The number of ticks in which health is visible.

        public int offscreenTicks = 0; //Off-screen total time of 1 sec kills player.

        /// <summary>
        /// Creates a player instance and sets coordinates.
        /// </summary>
        public ObjPlayer(MainLoop game, int x, int y)
        {
            this.game = game;
            pos = new Vector2(x, y);
            posOld = pos;
        }

        /// <summary>
        /// Loads the player sprite.
        /// </summary>
        public void LoadContent(ContentManager Content)
        {
            texPlayer = Content.Load<Texture2D>("TexPlayer");
            sprite = new Sprite(true, texPlayer);
            spritePhysics = new SpritePhysics(sprite);
            sprite.rectDest.X = (int)pos.X;
            sprite.rectDest.Y = (int)pos.Y;
            sprite.CenterOrigin();
            sprite.drawBehavior = SpriteDraw.all;
            sprite.angle = (float)(-Math.PI / 2);
        }

        /// <summary>
        /// Updates the player and responds to user actions.
        /// </summary>
        public void Update(KeyboardState keyState, KeyboardState keyStateOld, MouseState mouseState, MouseState mouseStateOld, ObjSpawnControl objSpawnControl)
        {
            //Updates the time that the healthbar is shown.
            healthVisible--;
            if (healthVisible < 0)
            {
                healthVisible = 0;
            }

            //Sets the current position to the old position
            posOld.X = pos.X;
            posOld.Y = pos.Y;

            //Handles movement of the player
            if (keyState.IsKeyDown(Keys.Right) || keyState.IsKeyDown(Keys.D))
            {
                spritePhysics.AddMotion(speed, 0);
                if (spritePhysics.XMove > speed * 20)
                {
                    spritePhysics.XMove = speed * 20;
                }
            }
            if (keyState.IsKeyDown(Keys.Up) || keyState.IsKeyDown(Keys.W))
            {
                spritePhysics.AddMotion(0, -speed);
                if (spritePhysics.YMove < -speed * 20)
                {
                    spritePhysics.YMove = -speed * 20;
                }
            }
            if (keyState.IsKeyDown(Keys.Left) || keyState.IsKeyDown(Keys.A))
            {
                spritePhysics.AddMotion(-speed, 0);
                if (spritePhysics.XMove < -speed * 20)
                {
                    spritePhysics.XMove = -speed * 20;
                }
            }
            if (keyState.IsKeyDown(Keys.Down) || keyState.IsKeyDown(Keys.S))
            {
                spritePhysics.AddMotion(0, speed);
                if (spritePhysics.YMove > speed * 20)
                {
                    spritePhysics.YMove = speed * 20;
                }
            }

            //Applies friction
            if (spritePhysics.XMove > 0)
            {
                spritePhysics.XMove -= friction;
            }
            else if (spritePhysics.XMove < 0)
            {
                spritePhysics.XMove += friction;
            }
            if (spritePhysics.YMove > 0)
            {
                spritePhysics.YMove -= friction;
            }
            else if (spritePhysics.YMove < 0)
            {
                spritePhysics.YMove += friction;
            }
            
            //Updates the components of the player
            spritePhysics.Update();
            pos.X = sprite.rectDest.X;
            pos.Y = sprite.rectDest.Y;

            //Checks for move-wrapping
            if (sprite.rectDest.X < -(sprite.texture.Width / 2))
            {
                sprite.rectDest.X = game.GraphicsDevice.Viewport.Width + (texPlayer.Width / 2);
            }
            else if (sprite.rectDest.X > game.GraphicsDevice.Viewport.Width + (texPlayer.Width / 2))
            {
                sprite.rectDest.X = -(sprite.texture.Width / 2);
            }

            if (sprite.rectDest.Y < -(sprite.texture.Height / 2))
            {
                sprite.rectDest.Y = game.GraphicsDevice.Viewport.Height + (sprite.texture.Height / 2);
            }
            else if (sprite.rectDest.Y > game.GraphicsDevice.Viewport.Height + (sprite.texture.Height / 2))
            {
                sprite.rectDest.Y = -(sprite.texture.Height / 2);
            }

            //Checks for partial move-wrapping and penalizes player for potential cheating.
            if (sprite.rectDest.X < 0 ||
                sprite.rectDest.X > game.GraphicsDevice.Viewport.Width ||
                sprite.rectDest.Y < 0 ||
                sprite.rectDest.Y > game.GraphicsDevice.Viewport.Height)
            {
                offscreenTicks++;
            }
            else
            {
                offscreenTicks = 0;
            }

            //Reduces player life if offscreenTicks is too high
            if (offscreenTicks >= 20)
            {
                hp--;
                CheckDeath(true);
            }

            //Reduces the bullet delay to 0.
            if (canShoot == false)
            {
                shootingDelay--;
                if (shootingDelay == 0)
                {
                    canShoot = true;
                }
            }

            //Creates bullets
            if (canShoot && mouseState.LeftButton == ButtonState.Pressed)
            {
                if (objSpawnControl.difficulty < 1)
                {
                    shootingDelay = 10;
                }
                else
                {
                    shootingDelay = 15;
                }
                canShoot = false;
                ObjBullet bullet = new ObjBullet(game, sprite, game.objSpawnControl);
                objSpawnControl.bulletIndex.Add(bullet);
                bullet.angle = (float)Mathematics.PointDirection(
                    new Vector2(sprite.rectDest.X, sprite.rectDest.Y),
                    new Vector2(game.objCrosshair.mouseState.X, game.objCrosshair.mouseState.Y));
                bullet.spritePhysics.AddMotionVector(true, 6, bullet.angle);
                bullet.sprite.rectDest.X = sprite.rectDest.X;
                bullet.sprite.rectDest.Y = sprite.rectDest.Y;
                bullet.sprite.CenterOrigin();
            }

            //Creates missiles
            if (missileNum != 0 &&
                mouseState.RightButton == ButtonState.Pressed && mouseStateOld.RightButton == ButtonState.Released)
            {
                missileNum--;
                ObjMissile missile = new ObjMissile(game, this.sprite, game.objSpawnControl);
                objSpawnControl.missileIndex.Add(missile);
                missile.angle = (float)Mathematics.PointDirection(
                    new Vector2(sprite.rectDest.X, sprite.rectDest.Y),
                    new Vector2(game.objCrosshair.mouseState.X, game.objCrosshair.mouseState.Y));
                missile.spritePhysics.AddMotionVector(true, 1, missile.angle, 0.5, missile.angle);
                missile.sprite.rectDest.X = sprite.rectDest.X;
                missile.sprite.rectDest.Y = sprite.rectDest.Y;
                missile.sprite.CenterOrigin();
            }

            //Updates the rotational direction of the player
            if (!pos.Equals(posOld))
            {
                sprite.angle = (float)spritePhysics.GetDirection(true);

                //Fixes an important issue with turning left
                if (spritePhysics.XMove < 0)
                {
                    sprite.angle = (float)(Math.Atan(spritePhysics.YMove / spritePhysics.XMove) + Math.PI);
                }
            }
        }

        /// <summary>
        /// Draws the player sprite.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            if (healthVisible > 0)
            {
                spriteBatch.Draw(
                    game.texHealth,
                    new Rectangle(sprite.rectDest.ToRect().X - (sprite.rectDest.ToRect().Width / 2), sprite.rectDest.ToRect().Y + (sprite.rectDest.ToRect().Height / 2), (int)(hp / 2), 10),
                    Color.Lerp(Color.Red, Color.Green, hp / 100));
            }
            spriteBatch.Draw(
                game.texHud,
                new Rectangle(0, 0, game.texHud.Width, game.texHud.Height),
                new Rectangle(0, 0, game.texHud.Width, game.texHud.Height),
                Color.White);
            spriteBatch.DrawString(game.fntBold, score.ToString(), new Vector2(16, 0), Color.Yellow);
            spriteBatch.DrawString(game.fntBold, missileNum.ToString(), new Vector2(16, 24), Color.Red);
            
        }

        /// <summary>
        /// Checks to see if the player has died.
        /// </summary>
        public void CheckDeath(bool noSound)
        {
            healthVisible = healthDuration;

            if (hp <= 0)
            {
                Sound.Play(game.sfxPlayerDies, Convert.ToInt16(!game.isMuted) * 0.25f);
                game.isDead = true;
            }
            else
            {
                if (!noSound)
                {
                    Sound.Play(game.sfxPlayerHit, Convert.ToInt16(!game.isMuted));
                }
            }
        }
    }
}