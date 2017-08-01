using Microsoft.Xna.Framework;
using System;

namespace SimpleXnaFramework
{
    /// <summary>
    /// Provides simple and useful mathematical functions
    /// </summary>
    static class Mathematics
    {
        static Random random;

        static Mathematics()
        {
            random = new Random();
        }

        /// <summary>
        /// Returns one of the listed numbers.
        /// </summary>
        /// <param name="nums">Any amount of numbers in any order.</param>
        public static double Choose(params double[] nums)
        {
            return nums[random.Next(nums.Length)];
        }

        /// <summary>
        /// Calculates the direction in radians between two points.
        /// </summary>
        public static double PointDirection(Vector2 ptOld, Vector2 ptNew)
        {
            return (Math.Atan2(ptNew.Y - ptOld.Y, ptNew.X - ptOld.X));
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        public static double PointDistance(Vector2 ptOld, Vector2 ptNew)
        {
            return Math.Sqrt(Math.Pow(ptOld.X - ptNew.X, 2) + Math.Pow(ptOld.Y - ptNew.Y, 2));
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        public static double PointDistance(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        }

        /// <summary>Generates a point within a rectangular region.</summary>
        public static Vector2 PointInRectangle(Rectangle rectangle, Vector2 point)
        {
            return new Vector2(rectangle.X - (rectangle.Width / 2) + random.Next(rectangle.Width),
                rectangle.X - (rectangle.Width / 2) + random.Next(rectangle.Width));
        }

        /// <summary>Generates a point within an elliptical region.</summary>
        public static Vector2 PointInEllipse(Rectangle rectangle, Vector2 point)
        {
            double phi = random.NextDouble() * (2 * Math.PI);
            double rho = random.NextDouble();
            return new Vector2((float)(Math.Sqrt(rho) * Math.Cos(phi) * rectangle.Width / 2),
                (float)(Math.Sqrt(rho) * Math.Sin(phi) * rectangle.Height / 2));
        }

        ///<summary>
        ///Returns whether or not the ray intersects the rectangle.
        ///</summary>
        ///<param name="V1Start">The x,y coordinates for the beginning of the ray.</param>
        ///<param name="V1End">The x,y coordinates for the end of the ray.</param>
        ///<param name="V2Start">The x,y coordinates for the beginning of the second ray.</param>
        ///<param name="V2End">The x,y coordinates for the end of the second ray.</param>
        ///<param name="intersection">The value that defines where an intersection occured.</param>
        // a1 is line1 start, a2 is line1 end, b1 is line2 start, b2 is line2 end
        public static bool RayRectangle(Vector2 V1Start,
            Vector2 V1End,
            Vector2 V2Start,
            Vector2 V2End,
            out Vector2 intersection)
        {
            intersection = Vector2.Zero; //The point of intersection, if any.

            Vector2 V1 = V1End - V1Start; //The first line segment.
            Vector2 V2 = V2End - V2Start; //The second line segment.
            float Perp = V1.X * V2.Y - V1.Y * V2.X; //The orientation of the lines.

            // if the lines aren't perpendicular, they never intersect.
            if (Perp == 0)
            {
                return false;
            }

            Vector2 VStart = V2Start - V1Start;
            float t = (VStart.X * V2.Y - VStart.Y * V2.X) / Perp;
            if (t < 0 || t > 1)
            {
                return false;
            }

            float u = (VStart.X * V1.Y - VStart.Y * V1.X) / Perp;
            if (u < 0 || u > 1)
            {
                return false;
            }

            intersection = V1Start + t * V1;
            return true;
        }

        /// <summary>Returns whether or not the point is in the rectangle.</summary>
        public static bool PointRectangle(Rectangle rectangle, Vector2 point)
        {
            if (point.X >= rectangle.X && point.X <= rectangle.X + rectangle.Width &&
                point.Y >= rectangle.Y && point.Y <= rectangle.Y + rectangle.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns whether or not the rectangles overlap.
        /// Usually you can use Rectangle.Intersect(rect, rect) instead.
        /// </summary>
        public static bool RectangleRectangle(Rectangle rectangle1, Rectangle rectangle2)
        {
            if (rectangle1.X < (rectangle2.X + rectangle2.Width) &&
                (rectangle1.X + rectangle1.Width) > rectangle2.X &&
                rectangle1.Y < (rectangle2.Y + rectangle2.Height) &&
                (rectangle1.Y + rectangle1.Height) > rectangle2.Y)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns whether or not two sprites are
        /// colliding based on their sprites.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool IntersectPixels(Rectangle rectangleA, Color[] dataA, Rectangle rectangleB, Color[] dataB, byte alphaThreshold)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A > alphaThreshold && colorB.A > alphaThreshold)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        /// <summary>
        /// Returns whether or not two sprites are
        /// colliding based on their textures taking
        /// rotation into account.
        /// </summary>
        /// <param name="tex1">The color data of the first texture.</param>
        /// <param name="mat1">The matrix corresponding with texture 1.</param>
        /// <param name="tex2">The color data of the second texture.</param>
        /// <param name="mat2">The matrix corresponding with texture 2.</param>
        /// <returns></returns>
        private static Vector2 IntersectPixelsRotated(Color[,] tex1, Matrix mat1, Color[,] tex2, Matrix mat2)
        {
            Matrix mat1to2 = mat1 * Matrix.Invert(mat2);
            int width1 = tex1.GetLength(0);
            int height1 = tex1.GetLength(1);
            int width2 = tex2.GetLength(0);
            int height2 = tex2.GetLength(1);

            for (int x1 = 0; x1 < width1; x1++)
            {
                for (int y1 = 0; y1 < height1; y1++)
                {
                    Vector2 pos1 = new Vector2(x1, y1);
                    Vector2 pos2 = Vector2.Transform(pos1, mat1to2);

                    int x2 = (int)pos2.X;
                    int y2 = (int)pos2.Y;
                    if ((x2 >= 0) && (x2 < width2))
                    {
                        if ((y2 >= 0) && (y2 < height2))
                        {
                            if (tex1[x1, y1].A > 0)
                            {
                                if (tex2[x2, y2].A > 0)
                                {
                                    Vector2 screenPos = Vector2.Transform(pos1, mat1);
                                    return screenPos;
                                }
                            }
                        }
                    }
                }
            }

            return new Vector2(-1, -1);
        }

        /// <summary>Returns a constant in the ShapeLocation enum that says where the object is.</summary>
        public static bool PointCircle(Vector2 circleLocation, int circleRad, Vector2 Point)
        {
            if (Math.Sqrt(Math.Pow(Point.X - circleLocation.X, 2) + Math.Pow(Point.Y - circleLocation.Y, 2)) <= circleRad)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>Returns a constant in the ShapeLocation enum that says where the object is.</summary>
        public static bool PointSphere(Vector3 circleLocation, int circleRad, Vector3 Point)
        {
            if (Math.Sqrt(Math.Pow(Point.X - circleLocation.X, 2) + Math.Pow(Point.Y - circleLocation.Y, 2) + Math.Pow(Point.Z - circleLocation.Z, 2)) < circleRad)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
