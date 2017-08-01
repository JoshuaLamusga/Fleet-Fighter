using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleXnaFramework;
using System;

namespace FleetFighter
{
    /// <summary>
    /// A bullet created by a player or enemy.
    /// It cannot collide with the given sprite instance.
    /// </summary>
    public class ObjFragment
    {
        public MainLoop game;
        public ObjSpawnControl objSpawnControl;
        public Sprite sprite;
        public SpritePhysics spritePhysics;
        public SpriteAtlas spriteAtlas;
        public ObjTimer timer;
        //set to false repeatedly for a
        //flashing effect before disappearance
        public int visible = 4;
        public Random random;
        public bool destroyed = false; //checked by the fragmentIndex and deleted if true.        

        /// <summary>
        /// Instantiates a bullet.
        /// </summary>
        public ObjFragment(MainLoop game, ObjSpawnControl objSpawnControl)
        {
            timer = new ObjTimer();
            this.game = game;
            this.objSpawnControl = objSpawnControl;
            random = new Random();
            sprite = new Sprite(true, game.texFragment);
            spritePhysics = new SpritePhysics(sprite);
            spriteAtlas = new SpriteAtlas(sprite, 16, 16, 4, 1, 4);
            sprite.drawBehavior = SpriteDraw.all;
            spriteAtlas.CenterOrigin();
            spriteAtlas.frame = random.Next(0, 4);
            spriteAtlas.Update(true);
        }

        /// <summary>
        /// Updates the bullet position and checks for collisions.
        /// </summary>
        public void Update()
        {
            timer.Update();
            spritePhysics.Update();

            //Destroys the fragment after awhile.
            if (timer.secs >= 10)
            {
                visible++;
                if (visible >= 6)
                {
                    visible = 0;
                }
            }
            if (timer.secs >= 12)
            {
                destroyed = true;
                if (objSpawnControl.difficulty >= 2)
                {
                    ObjEnemy enemy = new ObjEnemy(game, objSpawnControl, 0);
                    enemy.SetCharacteristics();
                    enemy.sprite.rectDest.X = sprite.rectDest.X;
                    enemy.sprite.rectDest.Y = sprite.rectDest.Y;
                    objSpawnControl.enemyIndex.Add(enemy);
                }
            }

            //Slows the fragment over time
            if (spritePhysics.XMove > 0)
            {
                spritePhysics.XMove -= 0.1;
            }
            else if (spritePhysics.XMove < 0)
            {
                spritePhysics.XMove += 0.1;
            }
            if (spritePhysics.YMove > 0)
            {
                spritePhysics.YMove -= 0.1;
            }
            else if (spritePhysics.YMove < 0)
            {
                spritePhysics.YMove += 0.1;
            }
            if (spritePhysics.rotMove > 0)
            {
                spritePhysics.rotMove -= 0.1;
            }

            //Controls collisions.
            Rectangle intersection = Rectangle.Intersect(
                sprite.rectDest.ToRect(),
                objSpawnControl.objPlayer.sprite.rectDest.ToRect());
            if (intersection != Rectangle.Empty)
            {
                Sound.Play(game.sfxFragmentCollected, Convert.ToInt16(!game.isMuted) * 0.5f, (float)(game.chance.NextDouble() * 1.5) - 0.5f);
                objSpawnControl.objPlayer.score += 10;
                objSpawnControl.objPlayer.hp++;
                if (objSpawnControl.objPlayer.hp >= 100)
                {
                    objSpawnControl.objPlayer.hp = 100;
                }
                else
                {
                    objSpawnControl.objPlayer.CheckDeath(true);
                }
                destroyed = true;
            }
        }

        /// <summary>
        /// Draws the bullet.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible >= 3)
            {
                sprite.Draw(spriteBatch);
            }
        }
    }
}