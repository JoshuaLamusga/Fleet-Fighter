using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleXnaFramework;
using System;

namespace FleetFighter
{
    /// <summary>
    /// Represents any type of enemy.
    /// AI is based on type.
    /// 
    /// Types and their uses:
    /// 
    /// 0: Fly straight down at a slow speed with only one life.
    /// 1: A powerful version of type 0 that banks after y > actionFloat1 by accel. actionFloat2.
    /// 2: Fly straight down, but stop after y > actionFloat1 and shoot at intervals for actionFloat2 ticks.
    /// 3: Come in traveling at actionFloat1 xSpeed and wraps around the screen.
    /// 4: Fly straight down with interval shooting; speed up after y > actionFloat1 by accel. actionFloat2.
    /// 5: Same as 2, but points at the player and takes more damage.
    /// 6: Fly straight down and do nothing else (acts like a bullet).
    /// 7: Fly straight down launching type 6 enemies every actionFloat1 ticks.
    /// </summary>
    public class ObjEnemy
    {
        public MainLoop game;
        public ObjSpawnControl objSpawnControl;
        public Sprite sprite;
        public SpritePhysics spritePhysics;
        public SpriteAtlas spriteAtlas;

        public int type = 0; //The type of enemy.
        public float hitsTotal; //How many hits it takes to destroy.
        public float hits; //How many hits it takes to destroy.
        
        public float angle = 0;
        public bool destroyed = false; //Checked by the enemyIndex and deleted if true.

        public int healthDuration = 60; //How long health is visible when hit.
        public int healthVisible = 0; //The number of ticks in which health is visible.

        public bool actionBool1 = false, actionBool2 = false, actionBool3 = false; //used by AI.
        public float actionFloat1 = 0, actionFloat2 = 0; //used by AI.
        public int actionTimeDuration = 0, actionTime = 0; //used by AI.
        public int actionTimeDuration2 = 0, actionTime2 = 0; //used by AI.

        public Color bulletColor; //The color of the bullets.

        /// <summary>
        /// Creates a new enemy.
        /// </summary>
        public ObjEnemy(MainLoop game, ObjSpawnControl objSpawnControl, int type)
        {
            this.game = game;
            this.objSpawnControl = objSpawnControl;
            this.type = type;
            sprite = new Sprite();
            spritePhysics = new SpritePhysics(sprite);
            sprite.drawBehavior = SpriteDraw.all;
            switch (type)
            {
                case (0):
                    sprite.SetTexture(true, game.texEnemy01);
                    spriteAtlas = new SpriteAtlas(sprite, 36, 27, 3, 1, 3);
                    bulletColor = Color.OrangeRed;
                    break;
                case (1):
                    sprite.SetTexture(true, game.texEnemy01);
                    spriteAtlas = new SpriteAtlas(sprite, 36, 27, 3, 1, 3);
                    bulletColor = Color.OrangeRed;
                    break;
                case (2):
                    sprite.SetTexture(true, game.texEnemy02);
                    spriteAtlas = new SpriteAtlas(sprite, 36, 33, 3, 1, 3);
                    bulletColor = Color.Aqua;
                    break;
                case (3):
                    sprite.SetTexture(true, game.texEnemy03);
                    spriteAtlas = new SpriteAtlas(sprite, 42, 43, 4, 1, 4);
                    bulletColor = Color.Fuchsia;
                    break;
                case (4):
                    sprite.SetTexture(true, game.texEnemy04);
                    spriteAtlas = new SpriteAtlas(sprite, 43, 49, 6, 1, 6);
                    bulletColor = Color.Yellow;
                    break;
                case (5):
                    sprite.SetTexture(true, game.texEnemy05);
                    spriteAtlas = new SpriteAtlas(sprite, 92, 81, 6, 1, 6);
                    bulletColor = Color.Blue;
                    break;
                case (6):
                    sprite.SetTexture(true, game.texEnemy06);
                    spriteAtlas = new SpriteAtlas(sprite, 10, 10, 1, 1, 1);
                    bulletColor = Color.White; //Fires no bullets.
                    break;
                case (7):
                    sprite.SetTexture(true, game.texEnemy07);
                    spriteAtlas = new SpriteAtlas(sprite, 52, 50, 3, 1, 3);
                    bulletColor = Color.White; //Fires no bullets.
                    break;
            }
        }

        //Sets the characteristics of the enemy.  Not placed in the constructor
        //so that you can set stuff in objSpawnControl before behaviors are finalized.
        public void SetCharacteristics()
        {
            switch (type)
            {
                case (0):
                    spritePhysics.YMove = 2;
                    spriteAtlas.frame = 2;
                    spriteAtlas.Update(true);
                    hitsTotal = 1;
                    hits = hitsTotal;
                    break;
                case (1):
                    spritePhysics.YMove = 4;
                    hitsTotal = 3;
                    hits = hitsTotal;
                    break;
                case (2):
                    actionTimeDuration = 60;
                    actionTime = 60;
                    actionTimeDuration2 = 20;
                    actionTime2 = 20;
                    spritePhysics.YMove = 2;
                    hitsTotal = 3;
                    hits = hitsTotal;
                    break;
                case (3):
                    spritePhysics.YMove = 3;
                    spritePhysics.XMove = actionFloat1;
                    hitsTotal = 4;
                    hits = hitsTotal;
                    break;
                case (4):
                    actionTimeDuration2 = 60;
                    actionTime2 = 60;
                    spritePhysics.YMove = 1;
                    hitsTotal = 6;
                    hits = hitsTotal;
                    break;
                case (5):
                    actionTimeDuration = 120;
                    actionTime = 120;
                    actionTimeDuration2 = 80;
                    actionTime2 = 80;
                    spritePhysics.YMove = 2;
                    hitsTotal = 6;
                    hits = hitsTotal;
                    break;
                case (6):
                    spritePhysics.YMove = 3;
                    spritePhysics.AddRotation(0.05f);
                    spriteAtlas.Update(true);
                    hitsTotal = 1;
                    hits = hitsTotal;
                    break;
                case (7):
                    actionTimeDuration = (int)actionFloat1;
                    actionTime = actionTimeDuration;
                    spritePhysics.YMove = 2;
                    spriteAtlas.Update(true);
                    hitsTotal = 3;
                    hits = hitsTotal;
                    break;
            }
            spriteAtlas.CenterOrigin();
        }

        public void Update()
        {
            //Updates the time that the healthbar is shown.
            healthVisible--;
            if (healthVisible < 0)
            {
                healthVisible = 0;
            }

            if (!(spritePhysics.XMove == 0 && spritePhysics.YMove == 0) && type != 6)
            {
                //Updates the angle for the enemies that change it.
                sprite.angle = (float)spritePhysics.GetDirection(true);

                //Fixes an important issue with turning left
                if (spritePhysics.XMove < 0)
                {
                    sprite.angle = (float)(Math.Atan(spritePhysics.YMove / spritePhysics.XMove) + Math.PI);
                }
            }

            //If the enemy is on-screen.
            if (sprite.rectDest.Y > -spriteAtlas.frameHeight / 2)
            {
                //Updates the current action time.
                actionTime--;
                if (actionTime < 0)
                {
                    actionTime = 0;
                }
                actionTime2--;
                if (actionTime2 < 0)
                {
                    actionTime2 = 0;
                }

                //Damages the player and destroys the enemy on collision.
                Rectangle intersection = Rectangle.Intersect(
                    sprite.rectDest.ToRect(), objSpawnControl.objPlayer.sprite.rectDest.ToRect());
                if (intersection != Rectangle.Empty)
                {
                    Sound.Play(game.sfxCollPlayerEnemy, Convert.ToInt16(!game.isMuted));
                    objSpawnControl.objPlayer.hp -= 30;
                    objSpawnControl.objPlayer.CheckDeath(false);
                    destroyed = true;
                }

                //Updates the frame based on the current life.
                spriteAtlas.frame = (hits - 1);

                //The main AI that distinguishes the enemies.
                //Used in bullet generation
                int chance = 0;

                switch (type)
                {
                    //Very basic red-orange enemies.
                    case (0):
                        chance = objSpawnControl.random.Next(0, 200);
                        break;
                    //Red-orange enemies that shoot and curve.
                    case (1):
                        chance = objSpawnControl.random.Next(0, 100);
                        if (sprite.rectDest.Y > actionFloat1 && !actionBool1)
                        {
                            actionBool1 = true;
                            spritePhysics.XAccel = actionFloat2;
                        }
                        break;
                    //Blue enemies that stop and shoot.
                    case (2):
                        chance = 0;
                        if (actionBool1 && spritePhysics.YMove == 0)
                        {
                            CreateBulletTimed(20, 40);
                        }
                        if (sprite.rectDest.Y > actionFloat1 && !actionBool1)
                        {
                            actionBool1 = true;
                            actionTime = (int)actionFloat2;
                            spritePhysics.YMove = 0;
                        }
                        if (actionTime == 0 && actionBool1 && !actionBool2)
                        {
                            actionBool2 = true;
                            spritePhysics.YMove = 2;
                            spritePhysics.YAccel = 0.1f;
                        }
                        break;
                    //Colorful enemies that wrap around the screen.
                    //They move diagonally.
                    case (3):
                        chance = objSpawnControl.random.Next(0, 200);

                        //Wraps around the screen horizontally only.
                        if (sprite.rectDest.X < -(sprite.rectDest.Width / 2))
                        {
                            sprite.rectDest.X = game.GraphicsDevice.Viewport.Width + (sprite.rectDest.Width / 2);
                        }
                        else if (sprite.rectDest.X > game.GraphicsDevice.Viewport.Width + (sprite.rectDest.Width / 2))
                        {
                            sprite.rectDest.X = -(sprite.rectDest.Width / 2);
                        }
                        if (sprite.rectDest.Y > game.GraphicsDevice.Viewport.Height + (sprite.rectDest.Height / 2))
                        {
                            objSpawnControl.enemyIndexDelete.Add(this);
                        }
                        break;
                    //Enemies that shoot at intervals and accelerate y.
                    case (4):
                        chance = 0;
                        CreateBulletTimed(20, 40);
                        if (sprite.rectDest.Y > actionFloat1 && !actionBool1)
                        {
                            actionBool1 = true;
                            spritePhysics.YAccel = actionFloat2;
                        }
                        break;
                    //Enemies that stop to shoot at you, then move on.
                    case (5):
                        chance = 0;
                        if (actionBool1 && spritePhysics.YMove == 0)
                        {
                            sprite.angle = (float)Mathematics.PointDirection(
                                new Vector2(sprite.rectDest.X, sprite.rectDest.Y),
                                new Vector2(
                                    objSpawnControl.objPlayer.sprite.rectDest.X,
                                    objSpawnControl.objPlayer.sprite.rectDest.Y));
                            CreateBulletTimed(20, 40);
                        }
                        if (sprite.rectDest.Y > actionFloat1 && !actionBool1)
                        {
                            actionBool1 = true;
                            actionTime = (int)actionFloat2;
                            spritePhysics.YMove = 0;
                        }
                        if (actionTime == 0 && actionBool1 && !actionBool2)
                        {
                            actionBool2 = true;
                            spritePhysics.YMove = 2;
                            spritePhysics.YAccel = 0.1f;
                        }
                        break;
                    case (6):
                        break;
                    case (7):
                        chance = 0;                        
                        break;
                }
                
                //Creates a bullet if by chance it is creatable.
                if (chance == 1)
                {
                    CreateBullet();
                }
            }
            //Updates the components of the sprite.
            spriteAtlas.Update(true);
            spritePhysics.Update();
        }

        /// <summary>
        /// Checks to see if the enemy object died.
        /// </summary>
        public void CheckDeath()
        {
            healthVisible = healthDuration;

            //queues the enemy for deletion if health is 0 or less.
            if (hits <= 0)
            {
                Sound.Play(game.sfxEnemyDestroyed, Convert.ToInt16(!game.isMuted) * 0.5f);
                if (type != 6)
                {
                    objSpawnControl.objPlayer.score += 10 * type;
                }
                else
                {
                    objSpawnControl.objPlayer.score += 10;
                }
                destroyed = true;
            }
            else
            {
                Sound.Play(game.sfxEnemyHit, Convert.ToInt16(!game.isMuted));
            }
        }
        
        /// <summary>
        /// Creates a bullet.
        /// </summary>
        public void CreateBullet()
        {
            ObjBullet bullet = new ObjBullet(game, sprite, objSpawnControl);
            bullet.isPlayerFriendly = false;

            bullet.angle = sprite.angle; //downwards
            bullet.sprite.color = bulletColor;
            bullet.spritePhysics.AddMotionVector(true, 6 + spritePhysics.GetMagnitude(), bullet.angle);
            bullet.sprite.rectDest.X = sprite.rectDest.X;
            bullet.sprite.rectDest.Y = sprite.rectDest.Y;
            bullet.sprite.CenterOrigin();

            objSpawnControl.bulletIndex.Add(bullet);
        }

        /// <summary>
        /// Creates a bullet on a time interval based on actionTime.
        /// </summary>
        public void CreateBulletTimed(int tickDuration1, int tickDuration2)
        {
            if (actionTime2 == 0)
            {
                //Changes the duration time.
                if (actionTimeDuration2 == tickDuration1)
                {
                    actionTimeDuration2 = tickDuration2;
                }
                else
                {
                    actionTimeDuration2 = tickDuration2;
                }
                //Resets the time to re-trigger bullet creation again.
                actionTime2 = actionTimeDuration2;

                CreateBullet();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (healthVisible > 0)
            {
            spriteBatch.Draw(
                    game.texHealth,
                    new Rectangle((sprite.rectDest.ToRect().X - (spriteAtlas.frameWidth / 2)),
                        sprite.rectDest.ToRect().Y - (spriteAtlas.frameHeight / 2) - 10,
                        (int)((hits / hitsTotal) * spriteAtlas.frameWidth / 2), 10),
                    Color.Lerp(Color.Red, Color.Green, hits / hitsTotal));
                
            }
            sprite.Draw(spriteBatch);
        }
    }
}