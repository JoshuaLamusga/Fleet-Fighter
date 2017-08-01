using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SimpleXnaFramework;

namespace FleetFighter
{
    /// <summary>
    /// Represents something in the menu.
    /// </summary>
    public class ObjMenuItem
    {
        public Sprite sprite;
        public bool isHovered = false;

        public ObjMenuItem(Texture2D texture, float x, float y)
        {
            sprite = new Sprite(true, texture);
            sprite.rectDest.X = (int)x;
            sprite.rectDest.Y = (int)y;
            sprite.rectDest.Width = texture.Width;
            sprite.rectDest.Height = texture.Height;
            sprite.CenterOrigin();
        }

        /// <summary>
        /// Call this in the main update loop.
        /// </summary>
        public void Update(ObjCrosshair mouse)
        {
            //Checks for an intersection between the crosshair
            //and the sprite of the menu item. The boolean given
            //is marked accordingly for use outside of the class.
            Rectangle intersection = Rectangle.Intersect(
                        new Rectangle((int)sprite.rectDest.X,
                            (int)sprite.rectDest.Y + 20,
                            (int)sprite.rectDest.Width,
                            (int)sprite.rectDest.Height - 20),
                        new Rectangle(Mouse.GetState().X,
                            Mouse.GetState().Y,
                            mouse.texture.Width,
                            mouse.texture.Height));
            //Whether or not an intersection occurred
            if (intersection != Rectangle.Empty)
            {
                isHovered = true;
            }
            else
            {
                isHovered = false;
            }
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