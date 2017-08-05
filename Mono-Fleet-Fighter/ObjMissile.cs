using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SimpleXnaFramework;
using System;

namespace FleetFighter
{
    /// <summary>
    /// A missile created by a player or enemy.
    /// It cannot collide with the given sprite instance.
    /// </summary>
    public class ObjMissile
    {
        public MainLoop game;
        public ObjSpawnControl objSpawnControl;
        public Sprite sprite, spriteNoCollision;
        public SpritePhysics spritePhysics;
        public ObjTimer timer; //Controls time until detonation.
        public float angle = 0;
        public bool destroyed = false; //checked by the missileIndex and deleted if true.

        /// <summary>
        /// Instantiates a missile.
        /// </summary>
        public ObjMissile(MainLoop game, Sprite NoCollision, ObjSpawnControl objSpawnControl)
        {
            Sound.Play(game.sfxMissile, Convert.ToInt16(!game.isMuted));
            this.objSpawnControl = objSpawnControl;
            spriteNoCollision = NoCollision;
            sprite = new Sprite(true, game.texMissile);
            sprite.drawBehavior = SpriteDraw.all;
            spritePhysics = new SpritePhysics(sprite);
            timer = new ObjTimer(30, 0, 0);
        }

        /// <summary>
        /// Updates the missile position and checks for collisions.
        /// </summary>
        public void Update()
        {
            spritePhysics.Update();
            sprite.angle = angle;
            timer.Update();

            foreach (ObjEnemy enemy in objSpawnControl.enemyIndex)
            {
                Rectangle intersection = Rectangle.Intersect(
                    sprite.rectDest.ToRect(), enemy.sprite.rectDest.ToRect());
                if (intersection != Rectangle.Empty)
                {
                    enemy.isDestroyed = true;
                    destroyed = true;
                }
            }
        }

        /// <summary>
        /// Draws the missile.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}