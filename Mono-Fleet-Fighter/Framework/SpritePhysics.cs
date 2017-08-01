using System;

namespace SimpleXnaFramework
{
    /// <summary>
    /// Deals with motion.
    /// Affects (X,Y) of rectDest.
    /// </summary>
    public class SpritePhysics
    {
        Sprite sprite;
        public double XMove = 0, YMove = 0;
        public double XAccel = 0, YAccel = 0;
        public double rotMove = 0, rotAccel = 0;

        public SpritePhysics(Sprite sprite)
        {
            this.sprite = sprite;
        }

        /// <summary>Sets the motion without vectors.</summary>
        /// <param name="speedH">Horizontal speed.</param>
        /// <param name="speedV">Vertical speed.</param>
        public void SetMotion(double XMove, double YMove)
        {
            this.XMove = XMove;
            this.YMove = YMove;
        }

        /// <summary>Sets the motion without vectors.</summary>
        /// <param name="XMove">Horizontal speed.</param>
        /// <param name="YMove">Vertical speed.</param>
        /// <param name="XAccel">Horizontal acceleration.</param>
        /// <param name="YAccel">Vertical acceleration.</param>
        public void SetMotion(double XMove, double YMove, double XAccel, double YAccel)
        {
            this.XMove = XMove;
            this.YMove = YMove;
            this.XAccel = XAccel;
            this.YAccel = YAccel;
        }

        /// <summary>Sets the motion with vectors</summary>
        /// <param name="isRadians">Whether or not the direction is expressed in radians.</param>
        /// <param name="mag">Magnitude of the vector.</param>
        /// <param name="dir">Direction of the vector.</param>
        public void SetMotionVector(bool isRadians, double mag, double dir)
        {
            if (!isRadians)
            {
                dir *= (Math.PI / 180);
            }
            XMove = mag * Math.Cos(dir);
            YMove = mag * Math.Sin(dir);
        }

        /// <summary>Sets the motion with vectors.</summary>
        /// <param name="isRadians">Whether or not the direction is expressed in radians.</param>
        /// <param name="mag">Magnitude of the current movement vector.</param>
        /// <param name="dir">Direction of the current movement vector.</param>
        /// <param name="magAccel">Magnitude of the acceleration vector.</param>
        /// <param name="dirAccel">Direction of the acceleration vector.</param>
        public void SetMotionVector(bool isRadians, double mag, double dir, double magAccel, double dirAccel)
        {
            if (!isRadians)
            {
                dir *= (Math.PI / 180);
            }
            XMove = mag * Math.Cos(dir);
            YMove = mag * Math.Sin(dir);
        }

        /// <summary>Adds to the motion without vectors.</summary>
        /// <param name="speedH">Horizontal speed.</param>
        /// <param name="speedV">Vertical speed.</param>
        public void AddMotion(double XMove, double YMove)
        {
            this.XMove += XMove;
            this.YMove += YMove;
        }

        /// <summary>Adds to the motion without vectors.</summary>
        /// <param name="XMove">Horizontal speed.</param>
        /// <param name="YMove">Vertical speed.</param>
        /// <param name="XAccel">Horizontal acceleration.</param>
        /// <param name="YAccel">Vertical acceleration.</param>
        public void AddMotion(double XMove, double YMove, double XAccel, double YAccel)
        {
            this.XMove += XMove;
            this.YMove += YMove;
            this.XAccel += XAccel;
            this.YAccel += YAccel;
        }

        /// <summary>Adds to the motion with vectors.</summary>
        /// <param name="isRadians">Whether or not the direction is expressed in radians.</param>
        /// <param name="mag">Magnitude of the vector.</param>
        /// <param name="dir">Direction of the vector.</param>
        public void AddMotionVector(bool isRadians, double mag, double dir)
        {
            if (!isRadians)
            {
                dir *= (180 / Math.PI);
            }
            XMove += mag * Math.Cos(dir);
            YMove += mag * Math.Sin(dir);
        }

        /// <summary>Adds to the motion with vectors.</summary>
        /// <param name="isRadians">Whether or not the direction is expressed in radians.</param>
        /// <param name="mag">Magnitude of the current movement vector.</param>
        /// <param name="dir">Direction of the current movement vector.</param>
        /// <param name="magAccel">Magnitude of the acceleration vector.</param>
        /// <param name="dirAccel">Direction of the acceleration vector.</param>
        public void AddMotionVector(bool isRadians, double mag, double dir, double magAccel, double dirAccel)
        {
            if (!isRadians)
            {
                dir *= (Math.PI / 180);
                dirAccel *= (Math.PI / 180);
            }
            XMove += mag * Math.Cos(dir);
            YMove += mag * Math.Sin(dir);
            XAccel += magAccel * Math.Cos(dir);
            YAccel += magAccel * Math.Sin(dir);
        }

        /// <summary>Sets the magnitude portion of the vector.</summary>
        /// <param name="mag">The new magnitude of the vector.</param>
        public void SetMagnitude(double mag)
        {
            double dir = GetDirection(true);
            XMove = mag * Math.Cos(dir);
            YMove = mag * Math.Sin(dir);
        }

        /// <summary>Sets the direction portion of the vector.</summary>
        /// <param name="mag">The new direction of the vector.</param>
        public void SetDirection(double dir)
        {
            double mag = GetMagnitude();
            XMove = mag * Math.Cos(dir);
            YMove = mag * Math.Sin(dir);
        }

        /// <summary>Returns the overall magnitude of movement</summary>
        public double GetMagnitude()
        {
            return (Math.Sqrt(Math.Pow(XMove, 2) + Math.Pow(YMove, 2)));

        }

        /// <summary>Returns the overall direction of movement.</summary>
        /// <param name="isRadians">Whether or not the direction should be returned in radians.</param>
        public double GetDirection(bool isRadians)
        {
            if (isRadians)
            {
                return Math.Atan(YMove / XMove);
            }
            else
            {
                return Math.Atan((YMove / XMove) * (180 / Math.PI));
            }
        }

        /// <summary>
        /// Sets the linear angular speed of the object.
        /// </summary>
        /// <returns></returns>
        public void AddRotation(double rotMove)
        {
            this.rotMove = rotMove;
        }

        /// <summary>
        /// Sets the linear angular speed of the object.
        /// </summary>
        /// <returns></returns>
        public void AddRotation(double rotMove, double rotAccel)
        {
            this.rotMove = rotMove;
            this.rotAccel = rotAccel;
        }

        /// <summary>
        /// Moves the object in a certain direction
        /// by modifying X and Y coordinates directly.
        /// </summary>
        public void AddMotionDirection(bool isRadians, double mag, double dir)
        {
            if (!isRadians)
            {
                dir *= (Math.PI / 180);
            }
            sprite.rectDest.X += (float)(mag * Math.Cos(dir));
            sprite.rectDest.Y += (float)(mag * Math.Sin(dir));
        }

        /// <summary>Clears all motion.</summary>
        public void Clear()
        {
            XMove = 0;
            YMove = 0;
            XAccel = 0;
            YAccel = 0;
            rotMove = 0;
            rotAccel = 0;
        }

        /// <summary>Updates movement with acceleration.</summary>
        public void Update()
        {
            XMove += XAccel;
            YMove += YAccel;
            rotMove += rotAccel;
            sprite.rectDest.X += (float)XMove;
            sprite.rectDest.Y += (float)YMove;
            sprite.angle += (float)rotMove;
        }
    }
}
