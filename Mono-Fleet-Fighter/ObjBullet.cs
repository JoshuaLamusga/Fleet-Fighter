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
    public class ObjBullet
    {
        public MainLoop game;
        public ObjSpawnControl objSpawnControl;
        public Sprite sprite, spriteNoCollision;
        public SpritePhysics spritePhysics;
        public float angle = 0;
        public bool isPlayerFriendly = false;
        public bool destroyed = false; //checked by the bulletIndex and deleted if true.

        /// <summary>
        /// Instantiates a bullet.
        /// </summary>
        public ObjBullet(MainLoop game, Sprite NoCollision, ObjSpawnControl objSpawnControl)
        {            
            Sound.Play(game.sfxBullet, Convert.ToInt16(!game.isMuted) * 0.5f, (float)(game.chance.NextDouble() * 1.5) - 0.5f);
            this.game = game;
            this.objSpawnControl = objSpawnControl;
            spriteNoCollision = NoCollision;
            //Sets what the bullet can hit. NOTE: Depends on the player having texture consistency!
            if (NoCollision.texture == game.objSpawnControl.objPlayer.texPlayer)
            {
                isPlayerFriendly = true;
            }
            else
            {
                isPlayerFriendly = false;
            }
            sprite = new Sprite(true, game.texBullet);
            sprite.drawBehavior = SpriteDraw.all;
            spritePhysics = new SpritePhysics(sprite);            
        }

        /// <summary>
        /// Updates the bullet position and checks for collisions.
        /// </summary>
        public void Update()
        {
            spritePhysics.Update();
            sprite.angle = angle;
            //Controls collisions.
            if (isPlayerFriendly)
            {
                foreach (ObjEnemy enemy in objSpawnControl.enemyIndex)
                {
                    Rectangle intersection = Rectangle.Intersect(
                        sprite.rectDest.ToRect(), enemy.sprite.rectDest.ToRect());
                    if (intersection != Rectangle.Empty)
                    {
                        Sound.Play(game.sfxCollBullet, Convert.ToInt16(!game.isMuted) * 0.5f);
                        enemy.hits--;
                        enemy.CheckDeath();
                        destroyed = true;
                    }
                }
            }
            else
            {
                Rectangle intersection = Rectangle.Intersect(
                    sprite.rectDest.ToRect(),
                    objSpawnControl.objPlayer.sprite.rectDest.ToRect());
                if (intersection != Rectangle.Empty)
                {
                    //Sound.Play(game.sfxCollBullet, Convert.ToInt16(!game.isMuted) * 0.5f);
                    objSpawnControl.objPlayer.hp -= 20;
                    objSpawnControl.objPlayer.CheckDeath(false);
                    destroyed = true;
                }
            }
        }

        /// <summary>
        /// Draws the bullet.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}
