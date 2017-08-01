using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleXnaFramework;

namespace FleetFighter
{
    /// <summary>
    /// The crosshair, used as a replacement for the mouse
    /// </summary>
    public class ObjCrosshair
    {
        public MouseState mouseState;
        public Texture2D texture;
        public Sprite sprite;

        /// <summary>
        /// Creates a new mouse cursor in the shape of a crosshair.
        /// </summary>
        public ObjCrosshair()
        {
            sprite = new Sprite();            
        }

        /// <summary>
        /// Loads the crosshair texture.
        /// </summary>
        /// <param name="Content">The content manager used in the main loop.</param>
        public void LoadContent(ContentManager Content)
        {
            texture = Content.Load<Texture2D>("TexCrosshair");
            sprite.SetTexture(true, texture);
            sprite.CenterOrigin();
            sprite.drawBehavior = SpriteDraw.all;
        }

        /// <summary>
        /// Call this in the main update loop.
        /// </summary>
        public void Update()
        {
            mouseState = Mouse.GetState();
            sprite.rectDest = new SmoothRect(mouseState.X, mouseState.Y, texture.Width, texture.Height);
            sprite.angle += 0.01f;
        }

        /// <summary>
        /// Call this in the main draw loop.
        /// </summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
        }
    }
}