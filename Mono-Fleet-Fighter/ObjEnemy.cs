using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleXnaFramework;
using System;

namespace FleetFighter
{
    /// <summary>
    /// Represents any type of enemy.
    /// AI is based on type.
    /// </summary>
    public class ObjEnemy
    {
        public MainLoop game;
        public ObjSpawnControl objSpawnControl;
        public Sprite sprite;
        public SpritePhysics spritePhysics;
        public SpriteAtlas spriteAtlas;

        public EnemyType type; //The type of enemy.
        public float hitsTotal; //How many hits it takes to destroy.
        public float hits; //How many hits it takes to destroy.
        
        public float angle = 0;
        public bool isDestroyed = false; //Checked by the enemyIndex and deleted if true.

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
        public ObjEnemy(MainLoop game, ObjSpawnControl objSpawnControl, EnemyType type)
        {
            this.game = game;
            this.objSpawnControl = objSpawnControl;
            this.type = type;
            sprite = new Sprite();
            spritePhysics = new SpritePhysics(sprite);
            sprite.drawBehavior = SpriteDraw.all;
            switch (type)
            {
                case (EnemyType.Flyer):
                    sprite.SetTexture(true, game.texEnemy01);
                    spriteAtlas = new SpriteAtlas(sprite, 36, 27, 3, 1, 3);
                    bulletColor = Color.OrangeRed;
                    break;
                case (EnemyType.BankingFlyer):
                    sprite.SetTexture(true, game.texEnemy01);
                    spriteAtlas = new SpriteAtlas(sprite, 36, 27, 3, 1, 3);
                    bulletColor = Color.OrangeRed;
                    break;
                case (EnemyType.Sentry):
                    sprite.SetTexture(true, game.texEnemy02);
                    spriteAtlas = new SpriteAtlas(sprite, 36, 33, 3, 1, 3);
                    bulletColor = Color.Aqua;
                    break;
                case (EnemyType.Distractor):
                    sprite.SetTexture(true, game.texEnemy03);
                    spriteAtlas = new SpriteAtlas(sprite, 42, 43, 4, 1, 4);
                    bulletColor = Color.Fuchsia;
                    break;
                case (EnemyType.ThrustFlyer):
                    sprite.SetTexture(true, game.texEnemy04);
                    spriteAtlas = new SpriteAtlas(sprite, 43, 49, 6, 1, 6);
                    bulletColor = Color.Yellow;
                    break;
                case (EnemyType.BankingFlyerHard):
                    sprite.SetTexture(true, game.texEnemy05);
                    spriteAtlas = new SpriteAtlas(sprite, 92, 81, 6, 1, 6);
                    bulletColor = Color.Blue;
                    break;
                case (EnemyType.Bullet):
                    sprite.SetTexture(true, game.texEnemy06);
                    spriteAtlas = new SpriteAtlas(sprite, 10, 10, 1, 1, 1);
                    bulletColor = Color.White;
                    break;
                case (EnemyType.Spawner):
                    sprite.SetTexture(true, game.texEnemy07);
                    spriteAtlas = new SpriteAtlas(sprite, 52, 50, 3, 1, 3);
                    bulletColor = Color.White;
                    break;
            }
        }

        //Sets the characteristics of the enemy.  Not placed in the constructor
        //so that you can set stuff in objSpawnControl before behaviors are finalized.
        public void SetCharacteristics()
        {
            switch (type)
            {
                case (EnemyType.Flyer):
                    spritePhysics.YMove = 2;
                    spriteAtlas.frame = 2;
                    spriteAtlas.Update(true);
                    hitsTotal = 1;
                    hits = hitsTotal;
                    break;
                case (EnemyType.BankingFlyer):
                    spritePhysics.YMove = 4;
                    hitsTotal = 3;
                    hits = hitsTotal;
                    break;
                case (EnemyType.Sentry):
                    actionTimeDuration = 60;
                    actionTime = 60;
                    actionTimeDuration2 = 20;
                    actionTime2 = 20;
                    spritePhysics.YMove = 2;
                    hitsTotal = 3;
                    hits = hitsTotal;
                    break;
                case (EnemyType.Distractor):
                    spritePhysics.YMove = 3;
                    spritePhysics.XMove = actionFloat1;
                    hitsTotal = 4;
                    hits = hitsTotal;
                    break;
                case (EnemyType.ThrustFlyer):
                    actionTimeDuration2 = 60;
                    actionTime2 = 60;
                    spritePhysics.YMove = 1;
                    hitsTotal = 6;
                    hits = hitsTotal;
                    break;
                case (EnemyType.BankingFlyerHard):
                    actionTimeDuration = 120;
                    actionTime = 120;
                    actionTimeDuration2 = 80;
                    actionTime2 = 80;
                    spritePhysics.YMove = 2;
                    hitsTotal = 6;
                    hits = hitsTotal;
                    break;
                case (EnemyType.Bullet):
                    spritePhysics.YMove = 3;
                    spritePhysics.AddRotation(0.05f);
                    spriteAtlas.Update(true);
                    hitsTotal = 1;
                    hits = hitsTotal;
                    break;
                case (EnemyType.Spawner):
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

            if (!(spritePhysics.XMove == 0 && spritePhysics.YMove == 0) && type != EnemyType.BankingFlyerHard)
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
                    isDestroyed = true;
                }

                //Updates the frame based on the current life.
                spriteAtlas.frame = (hits - 1);

                //The main AI that distinguishes the enemies.
                //Used in bullet generation
                int chance = 0;

                switch (type)
                {
                    case (EnemyType.Flyer):
                        chance = objSpawnControl.random.Next(0, 200);
                        break;

                    case (EnemyType.BankingFlyer):
                        chance = objSpawnControl.random.Next(0, 100);
                        if (sprite.rectDest.Y > actionFloat1 && !actionBool1)
                        {
                            actionBool1 = true;
                            spritePhysics.XAccel = actionFloat2;
                        }
                        break;

                    case (EnemyType.Sentry):
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

                    case (EnemyType.Distractor):
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

                    case (EnemyType.ThrustFlyer):
                        chance = 0;
                        CreateBulletTimed(20, 40);
                        if (sprite.rectDest.Y > actionFloat1 && !actionBool1)
                        {
                            actionBool1 = true;
                            spritePhysics.YAccel = actionFloat2;
                        }
                        break;

                    case (EnemyType.BankingFlyerHard):
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

                    case (EnemyType.Bullet):
                    case (EnemyType.Spawner):
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
                if (type != EnemyType.Bullet)
                {
                    objSpawnControl.objPlayer.score += 10 * (int)type;
                }
                else
                {
                    objSpawnControl.objPlayer.score += 10;
                }
                isDestroyed = true;
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