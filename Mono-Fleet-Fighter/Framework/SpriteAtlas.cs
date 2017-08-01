using Microsoft.Xna.Framework;

namespace SimpleXnaFramework
{
    /// <summary>
    /// Defines what happens when a frame end is reached.
    /// loop: loops the animation.
    /// end: sets frameSpeed to 0, stopping animation.
    /// reverse: reverses frameSpeed and animation.
    /// </summary>
    public enum FrameEnd { loop, end, reverse };

    /// <summary>
    /// Deals with animation using spritesheets.
    /// Affects all variables of rectSrc.
    /// </summary>
    public class SpriteAtlas
    {
        Sprite sprite;
        public double frame; //the current frame.
        public int frames; //the number of frames        
        public double frameSpeed; //the frame speed.
        public FrameEnd frameEndBehavior = FrameEnd.loop;
        public int frameWidth = 1; //The width of each frame.
        public int frameHeight = 1; //The height of each frame.
        public int atlasRows = 1; //The number of total rows.
        public int atlasCols = 1; //The number of total columns.
        public int frameOffsetH = 0; //The horizontal offset.
        public int frameOffsetV = 0; //The vertical offset.

        /// <summary>Animates through the given sprite's frames. Sets drawBehavior to basicAnimated if it's "basic".</summary>
        /// <param name="sprite">The sprite to use (must have a texture defined).</param>
        public SpriteAtlas(Sprite sprite)
        {
            this.sprite = sprite;
            if (this.sprite.drawBehavior == SpriteDraw.basic)
            {
                this.sprite.drawBehavior = SpriteDraw.basicAnimated;
            }
            sprite.rectSrc.Width = frameWidth;
            sprite.rectSrc.Height = frameHeight;
            sprite.rectDest.Width = frameWidth * this.sprite.scaleX;
            sprite.rectDest.Height = frameHeight * this.sprite.scaleY;
        }

        /// <summary>Animates through the given sprite's frames. Sets drawBehavior to basicAnimated if it's "basic".</summary>
        /// <param name="sprite">The sprite to use (must have a texture defined).</param>
        /// <param name="frameWidth">The width of each frame in the texture.</param>
        /// <param name="frameHeight">The height of each frame in the texture.</param>
        /// <param name="frames">The number of total frames.</param>
        /// <param name="rows">The number of rows (vertical frames) in the texture.</param>
        /// <param name="cols">The number of columns (side-by-side frames) in the texture.</param>
        public SpriteAtlas(Sprite sprite, int frameWidth, int frameHeight, int frames, int rows, int cols)
        {
            this.sprite = sprite;
            if (this.sprite.drawBehavior == SpriteDraw.basic)
            {
                this.sprite.drawBehavior = SpriteDraw.basicAnimated;
            }
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frames = frames;
            atlasRows = rows;
            atlasCols = cols;
            sprite.rectSrc.Width = frameWidth;
            sprite.rectSrc.Height = frameHeight;
            sprite.rectDest.Width = frameWidth * this.sprite.scaleX;
            sprite.rectDest.Height = frameHeight * this.sprite.scaleY;
        }

        /// <summary>Animates through the given sprite's frames. Sets drawBehavior to basicAnimated if it's "basic".</summary>
        /// <param name="sprite">The sprite to use (must have a texture defined).</param>
        /// <param name="frameWidth">The width of each frame in the texture.</param>
        /// <param name="frameHeight">The height of each frame in the texture.</param>
        /// <param name="frames">The number of total frames.</param>
        /// <param name="rows">The number of rows (vertical frames) in the texture.</param>
        /// <param name="cols">The number of columns (side-by-side frames) in the texture.</param>
        /// <param name="frameSpeed">Sets the speed at which frames update.</param>
        public SpriteAtlas(Sprite sprite, int frameWidth, int frameHeight, int frames, int rows, int cols, double frameSpeed)
        {
            this.sprite = sprite;
            if (this.sprite.drawBehavior == SpriteDraw.basic)
            {
                this.sprite.drawBehavior = SpriteDraw.basicAnimated;
            }
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frames = frames;
            atlasRows = rows;
            atlasCols = cols;
            this.frameSpeed = frameSpeed;
            sprite.rectSrc.Width = frameWidth;
            sprite.rectSrc.Height = frameHeight;
            sprite.rectDest.Width = frameWidth * this.sprite.scaleX;
            sprite.rectDest.Height = frameHeight * this.sprite.scaleY;
        }

        /// <summary>
        /// Centers the origin for the sprite atlas.
        /// </summary>
        public void CenterOrigin()
        {
            sprite.origin = new Vector2(frameWidth / 2, frameHeight / 2);
        }

        /// <summary>
        /// Sets the dimensions of the frames, how many there are, and the number of rows/cols.
        /// </summary>
        /// <param name="frameWidth">The width of each frame in the texture.</param>
        /// <param name="frameHeight">The height of each frame in the texture.</param>
        /// <param name="rows">The number of rows (vertical frames) in the texture.</param>
        /// <param name="cols">The number of columns (side-by-side frames) in the texture.</param>
        public void SetDimensions(int frameWidth, int frameHeight, int frames, int rows, int cols)
        {
            this.frameWidth = frameWidth;
            this.frameHeight = frameHeight;
            this.frames = frames;
            atlasRows = rows;
            atlasCols = cols;
        }

        /// <summary>
        /// Sets the behavior of the animation at the end of the frame.
        /// Examples are to continue from the first frame, or reverse animation.
        /// </summary>
        /// <param name="endBehavior">The end behavior (see FrameEnd enum).</param>
        public void SetEndBehavior(FrameEnd endBehavior)
        {
            frameEndBehavior = endBehavior;
        }

        /// <summary>
        /// Sets the frame offsets, or spaces between each image.
        /// </summary>
        /// <param name="frameOffsetH">The number of horz. pixels between each image.</param>
        /// <param name="frameOffsetV">The number of vert pixels between each image.</param>
        public void SetOffsets(int frameOffsetH, int frameOffsetV)
        {
            this.frameOffsetH = frameOffsetH;
            this.frameOffsetV = frameOffsetV;
        }

        /// <summary>Updates the current frame if changed.</summary>
        /// <param name="doUpdateAlways">If false, update is only called when there are more than 0 frames and framespeed is not 0.</param>
        public void Update(bool doUpdateAlways)
        {
            if (!doUpdateAlways && (frames <= 0 || frameSpeed == 0))
            {
                return;
            }
            else
            {
                frame += frameSpeed;

                //If the frame actually changed
                if (((int)(frame - frameSpeed) != (int)frame))
                {
                    /* The following switch statement applies end behaviors.
                     * If the current frame is out of bounds (exceeding the
                     * number of frames or less than 0), then it will switch
                     * the frameSpeed or current frame to either stop the
                     * animation, reverse it, or loop from the beginning.
                    */

                    if ((int)frame >= frames || (int)frame < 0)
                    {
                        switch (frameEndBehavior)
                        {
                            case (FrameEnd.end):
                                frameSpeed = 0;
                                break;
                            case (FrameEnd.loop):
                                if ((int)frame >= frames)
                                {
                                    frame = 0;
                                }
                                else
                                {
                                    frame = frames;
                                }
                                break;
                            case (FrameEnd.reverse):
                                frameSpeed = -frameSpeed;
                                break;
                        }
                    }
                }

                /*
                    * Sets the actual positions of the subimages
                    * This sets the x-value based on the number of frames, ignoring columns and rows.
                    * The maximum x-value per row is computed and subtracted from the existing x-value
                    * until it is under the max.  Every time it is subtracted, the column number is
                    * increased.  Offsets are computed afterwards.
                */
                sprite.rectSrc.X = (int)frame * frameWidth;
                sprite.rectSrc.Y = 0;
                int maxWidth = atlasCols * frameWidth;
                int maxHeight = atlasRows * frameHeight;

                //Creates row wrapping
                while (sprite.rectSrc.X >= maxWidth)
                {
                    sprite.rectSrc.X -= maxWidth;
                    if (frame != frames)
                    {
                        sprite.rectSrc.Y += frameHeight;
                    }
                }

                //Calculates offsets
                sprite.rectSrc.X += frameOffsetH * /*(int)*/(sprite.rectSrc.X / frameWidth);
                sprite.rectSrc.Y += frameOffsetV * /*(int)*/(sprite.rectSrc.Y / frameHeight);
            }
        }

    }
}
