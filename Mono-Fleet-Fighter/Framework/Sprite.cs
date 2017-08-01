using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SimpleXnaFramework
{
    public class Sprite
    {
        public Texture2D texture;
        public SmoothRect rectSrc, rectDest;
        public Vector2 origin;
        public float angle = 0, depth = 0;
        public Color color;
        public SpriteEffects spriteEffects;
        public float scaleX = 1, scaleY = 1; //a multiplier for width and height
        public SpriteDraw drawBehavior = SpriteDraw.basic;
               
        
        /// <summary>
        /// Initializes a new Sprite. Note: you must set the texture before drawing!
        /// Failure to set the texture will throw an error. This constructor is meant
        /// for use with SpriteFrames.
        /// </summary>
        public Sprite()
        {
            rectSrc = new SmoothRect(0, 0, 0, 0);
            rectDest = new SmoothRect(0, 0, 0, 0);
            origin = new Vector2(0, 0);
            color = Color.White;
            spriteEffects = SpriteEffects.None;
        }

        /// <summary>Initializes a new Sprite to control drawing.</summary>
        /// <param name="doSetDimensions">Whether or not to set the width and height based on the texture.</param>
        /// <param name="tex">The 2D texture to use.</param>
        public Sprite(
            bool doSetDimensions,
            Texture2D tex)
        {
            rectSrc = new SmoothRect(0, 0, 0, 0);
            rectDest = new SmoothRect(0, 0, 0, 0);
            SetTexture(doSetDimensions, tex);
            origin = new Vector2(0, 0);
            color = Color.White;
            spriteEffects = SpriteEffects.None;
        }

        /// <summary>Includes source/dest rectangles.</summary>
        /// <param name="doSetDimensions">Whether or not to set the width and height based on the texture.</param>
        /// <param name="tex">The 2D texture to use.</param>
        /// <param name="rectSrc">The source rectangle (for spritesheets).</param>
        /// <param name="rectDest">The destination rectangle (for position and stretching). Scaled by default.</param>
        public Sprite(
            Texture2D tex,
            SmoothRect rectSrc,
            SmoothRect rectDest)
        {
            this.rectSrc = rectSrc;
            this.rectDest.X = rectDest.X;
            this.rectDest.Y = rectDest.Y;
            this.rectDest.Width = (int)(rectDest.Width * scaleX);
            this.rectDest.Height = (int)(rectDest.Height * scaleY);
            SetTexture(false, texture);
            origin = new Vector2(0, 0);
            color = Color.White;
            spriteEffects = SpriteEffects.None;
        }

        /// <summary>
        /// Sets all parameters except the SpriteEffects used and scaling.
        /// </summary>
        /// <param name="rectSrc">The source rectangle (for spritesheets).</param>
        /// <param name="rectDest">The destination rectangle (for position and stretching). Scaled by default.</param>
        /// <param name="color">The Color object to be used.</param>
        /// <param name="angle">The angle of rotation, moving clockwise with right = 0, in radians.</param>
        /// <param name="depth">The order in which sprites are drawn. 0 is drawn first.</param>
        /// <param name="origin">Where (0,0) is located on the sprite in local coordinates.</param>
        public Sprite(
            Texture2D tex,
            SmoothRect rectSrc,
            SmoothRect rectDest,
            Vector2 origin,
            float angle,
            float depth,
            Color color
            )
        {
            this.rectSrc = rectSrc;
            this.rectDest.X = rectDest.X;
            this.rectDest.Y = rectDest.Y;
            this.rectDest.Width = (int)(rectDest.Width * scaleX);
            this.rectDest.Height = (int)(rectDest.Height * scaleY);
            SetTexture(false, texture);
            this.origin = origin;
            this.angle = angle;
            this.depth = depth;
            this.color = Color.White;
            spriteEffects = SpriteEffects.None;
        }

        /// <summary>
        /// Sets all parameters except the SpriteEffects used.  Sets scaling, too.
        /// </summary>
        /// <param name="rectSrc">The source rectangle (for spritesheets).</param>
        /// <param name="rectDest">The destination rectangle (for position and stretching). Scaled by default.</param>
        /// <param name="color">The Color object to be used.</param>
        /// <param name="angle">The angle of rotation, moving clockwise with right = 0, in radians.</param>
        /// <param name="depth">The order in which sprites are drawn. 0 is drawn first.</param>
        /// <param name="origin">Where (0,0) is located on the sprite in local coordinates.</param>
        public Sprite(
            Texture2D tex,
            SmoothRect rectSrc,
            SmoothRect rectDest,
            Vector2 origin,
            float angle,
            float depth,
            Color color,
            float scaleX,
            float scaleY
            )
        {
            this.rectSrc = rectSrc;
            this.rectDest.X = rectDest.X;
            this.rectDest.Y = rectDest.Y;
            this.rectDest.Width = (int)(rectDest.Width * scaleX);
            this.rectDest.Height = (int)(rectDest.Height * scaleY);
            SetTexture(false, texture);
            this.origin = origin;
            this.angle = angle;
            this.depth = depth;
            this.color = Color.White;
            spriteEffects = SpriteEffects.None;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        /// <summary>
        /// Sets the source and destination rectangles.
        /// Overridden by both Frames(Src) and Physics(Dest).
        /// </summary>
        /// <param name="rectSrc">The source rectangle to be used.</param>
        /// <param name="rectDest">The destination rectangle to be used.</param>
        /// <param name="doApplyScaling">Whether or not to apply scaling to rectDest (only set to false if scaling is already applied to the input rectDest).</param>
        public void SetCoords(SmoothRect rectSrc, SmoothRect rectDest, bool doApplyScaling)
        {
            this.rectSrc = rectSrc;
            this.rectDest.X = rectDest.X;
            this.rectDest.Y = rectDest.Y;
            if (doApplyScaling)
            {
                this.rectDest.Width = (int)(rectDest.Width * scaleX);
                this.rectDest.Height = (int)(rectDest.Height * scaleY);
            }
            else
            {
                this.rectDest.Width = rectDest.Width;
                this.rectDest.Height = rectDest.Height;
            }
        }

        /// <summary>
        /// Sets the position of the destination rectangle.
        /// Overridden by Physics(Dest); useful if manually done.
        /// </summary>
        /// <param name="xPos">The x-position on the screen.</param>
        /// <param name="yPos">The y-position on the screen.</param>
        public void SetCoords(int xPos, int yPos)
        {
            rectDest.X = xPos;
            rectDest.Y = yPos;
        }

        /// <summary>
        /// Sets the stylistic elements of the sprite
        /// </summary>
        /// <param name="col">The Color object to be used.</param>
        /// <param name="origin">Where (0,0) is located on the sprite in local coordinates.</param>
        public void SetStyles(Color color,
            float angle,
            Vector2 origin,
            SpriteEffects spriteEffects,
            float scaleX,
            float scaleY)
        {
            this.color = color;
            this.angle = angle;
            this.origin = origin;
            this.spriteEffects = spriteEffects;
            this.scaleX = scaleX;
            this.scaleY = scaleY;
        }

        /// <summary>Sets the depth.</summary>
        /// <param name="depth">The order in which sprites are drawn. 0 is drawn first.</param>
        public void SetDepth(float depth)
        {
            this.depth = depth;
        }

        /// <summary>Sets the texture and resets scaling effects.</summary>
        /// <param name="doSetDimensions">Whether or not width and height are set as well.</param>
        /// <param name="texture">The 2D texture to use.</param>
        public void SetTexture(bool doSetDimensions, Texture2D texture)
        {
            this.texture = texture;
            if (doSetDimensions)
            {
                rectSrc.Width = texture.Width;
                rectSrc.Height = texture.Height;
                rectDest.Width = (int)(texture.Width * scaleX);
                rectDest.Height = (int)(texture.Height * scaleY);
            }
        }

        /// <summary>
        /// Centers the origin of the image.
        /// </summary>
        public void CenterOrigin()
        {
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }

        /// <summary>Draws the sprite with a SpriteBatch.</summary>
        public void Draw(SpriteBatch spriteBatch)
        {
            switch (drawBehavior)
            {
                case (SpriteDraw.basic):
                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            rectDest.ToRect().X,
                            rectDest.ToRect().Y,
                            (int)(rectDest.ToRect().Width * scaleX),
                            (int)(rectDest.ToRect().Height * scaleY)
                            ),
                        color);
                    break;
                case (SpriteDraw.basicAnimated):
                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            rectDest.ToRect().X,
                            rectDest.ToRect().Y,
                            (int)(rectDest.ToRect().Width * scaleX),
                            (int)(rectDest.ToRect().Height * scaleY)
                            ),
                        rectSrc.ToRect(),
                        color);
                    break;
                case (SpriteDraw.all):
                    spriteBatch.Draw(
                        texture,
                        new Rectangle(
                            rectDest.ToRect().X,
                            rectDest.ToRect().Y,
                            (int)(rectDest.ToRect().Width * scaleX),
                            (int)(rectDest.ToRect().Height * scaleY)
                            ),
                        rectSrc.ToRect(),
                        color,
                        angle,
                        origin,
                        spriteEffects,
                        depth);
                    break;
            }
        }
    }
}