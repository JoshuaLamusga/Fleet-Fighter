using Microsoft.Xna.Framework.Graphics;
using SimpleXnaFramework;

namespace FleetFighter
{
    /// <summary>
    /// An explosion.
    /// </summary>
    public class ObjExplosion
    {
        public MainLoop game;
        public ObjSpawnControl objSpawnControl;
        public Sprite sprite;
        public SpriteAtlas spriteAtlas;
        public bool destroyed = false; //checked by the explosionIndex and deleted if true.

        /// <summary>
        /// Instantiates an explosion.
        /// </summary>
        public ObjExplosion(MainLoop game, Texture2D texture, ObjSpawnControl objSpawnControl)
        {
            this.game = game;
            this.objSpawnControl = objSpawnControl;
            sprite = new Sprite(true, texture);
            sprite.drawBehavior = SpriteDraw.all;
            if (texture == game.texExplosion)
            {
                spriteAtlas = new SpriteAtlas(sprite, 32, 32, 3, 1, 3);
                spriteAtlas.frameSpeed = 0.2;
            }
            if (texture == game.texMissileExplosion)
            {
                spriteAtlas = new SpriteAtlas(sprite, 71, 55, 6, 1, 6);
                spriteAtlas.frameSpeed = 0.2;
            }
            spriteAtlas.CenterOrigin();
        }

        /// <summary>
        /// Updates the explosion position and checks for collisions.
        /// </summary>
        public void Update()
        {
            sprite.scaleX += 0.1f;
            sprite.scaleY += 0.1f;

            //Qeues the explosion for deletion when it finishes its last frame.
            if (spriteAtlas.frame > (spriteAtlas.frames - 1))
            {
                destroyed = true;
            }

            spriteAtlas.Update(false);
        }

        /// <summary>
        /// Draws the explosion.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}