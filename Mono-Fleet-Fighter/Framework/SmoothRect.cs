using Microsoft.Xna.Framework;

namespace SimpleXnaFramework
{
    public class SmoothRect
    {
        public float X, Y, Width, Height;

        public SmoothRect(float X, float Y, float Width, float Height)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
        }

        public float Top
        {
            get
            {
                return Y;
            }
        }
        public float Bottom
        {
            get
            {
                return Y + Height;
            }
        }
        public float Left
        {
            get
            {
                return X;
            }
        }
        public float Right
        {
            get
            {
                return X + Width;
            }
        }
        public Vector2 Position
        {
            get
            {
                return new Vector2(X, Y);
            }
        }
        /// <summary>
        /// Returns an empty SmoothRect instance.
        /// </summary>
        public SmoothRect Empty()
        {
            return new SmoothRect(0, 0, 0, 0);
        }

        /// <summary>
        /// Returns a Rectangle instance.
        /// </summary>
        public Rectangle ToRect()
        {
            return new Rectangle((int)X, (int)Y, (int)Width, (int)Height);
        }
    }
}