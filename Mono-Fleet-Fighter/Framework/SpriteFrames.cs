using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SimpleXnaFramework
{
    /// <summary>
    /// Deals with animation using individual sprites.
    /// Indexes individual sprites.
    /// Affects all variables of rectSrc.
    /// Overrides the texture in the sprite.
    /// </summary>
    public class SpriteFrames
    {
        Sprite sprite;
        public List<Texture2D> frameTextures; //the current frame's texture.
        public double frame; //the current frame number.
        public int frames; //the number of frames        
        public double frameSpeed; //the frame speed.
        public FrameEnd frameEndBehavior = FrameEnd.loop;

        /// <summary>Animates through the given sprite's frames. Sets drawBehavior to basicAnimated if it's "basic".</summary>
        /// <param name="sprite">The sprite to use.</param>
        public SpriteFrames(Sprite sprite)
        {
            this.sprite = sprite;
            if (this.sprite.drawBehavior == SpriteDraw.basic)
            {
                this.sprite.drawBehavior = SpriteDraw.basicAnimated;
            }
            AdjustFrame();
        }

        /// <summary>Animates through the given sprite's frames. Sets drawBehavior to basicAnimated if it's "basic".</summary>
        /// <param name="sprite">The sprite to use.</param>
        /// <param name="frameTextures">The textures for each frame, in order, as a List object.</param>
        /// <param name="frames">The amount of frames available.</param>
        public SpriteFrames(Sprite sprite, List<Texture2D> frameTextures, int frames)
        {
            this.sprite = sprite;
            if (this.sprite.drawBehavior == SpriteDraw.basic)
            {
                this.sprite.drawBehavior = SpriteDraw.basicAnimated;
            }
            this.frameTextures = frameTextures;
            this.frames = frames;
            AdjustFrame();
        }

        /// <summary>Animates through the given sprite's frames. Sets drawBehavior to basicAnimated if it's "basic".</summary>
        /// <param name="sprite">The sprite to use.</param>
        /// <param name="frameTextures">The textures for each frame, in order, as a List object.</param>
        /// <param name="frames">The amount of frames available.</param>
        /// <param name="frameEndBehavior">What happens when the frame changes past the last frame (see FrameEnd enum).</param>
        /// <param name="frameSpeed">The speed at which the frames move each step (can be a decimal, can be negative).</param>
        public SpriteFrames(Sprite sprite,
            List<Texture2D> frameTextures,
            int frames,
            FrameEnd frameEndBehavior,
            double frameSpeed)
        {
            this.sprite = sprite;
            if (this.sprite.drawBehavior == SpriteDraw.basic)
            {
                this.sprite.drawBehavior = SpriteDraw.basicAnimated;
            }
            this.frameTextures = frameTextures;
            this.frames = frames;
            this.frameEndBehavior = frameEndBehavior;
            this.frameSpeed = frameSpeed;
            AdjustFrame();
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

        /// <summary>Sets the current frame and resets progress to the next frame.</summary>
        public void SetFrame(double frame)
        {
            this.frame = frame;
            frame = (int)frame;
            AdjustFrame();
        }

        /// <summary>
        /// Adjusts the width and height of the position rectangles for the frame.
        /// </summary>
        private void AdjustFrame()
        {
            sprite.texture = frameTextures[(int)frame];
            sprite.rectSrc.Width = frameTextures[(int)frame].Width;
            sprite.rectSrc.Height = frameTextures[(int)frame].Height;
            sprite.rectDest.Width = frameTextures[(int)frame].Width * sprite.scaleX;
            sprite.rectDest.Height = frameTextures[(int)frame].Height * sprite.scaleY;
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

                //Adjusts the frame attributes to match the current frame
                AdjustFrame();
            }
        }
    }
}
