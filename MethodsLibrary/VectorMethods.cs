using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MethodsLibrary
{
    public static class VectorMethods
    {
        #region rotate point/vector

        //unused
        /*
        public static Vector RotatePoint(Vector point, Vector origin, double angle)
        {
            Vector translated = point - origin;
            Vector rotated = new Vector
            {
                X = translated.X * Math.Cos(angle) - translated.Y * Math.Sin(angle),
                Y = translated.X * Math.Sin(angle) + translated.Y * Math.Cos(angle)
            };
            var output = rotated + origin;
            return new Vector(Math.Round(output.X, 2), Math.Round(output.Y, 2));
        }*/

        /// <summary>
        /// rotate a vector by angle degrees
        /// </summary>
        /// <param name="vector"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector RotateVector(Vector vector, double angle)
        {
            var angle_in_radians = (Math.PI / 180) * angle;
            return new Vector(
                Math.Round(vector.X * Math.Cos(angle_in_radians) - vector.Y * Math.Sin(angle_in_radians), 2),
                Math.Round(vector.Y * Math.Cos(angle_in_radians) + vector.X * Math.Sin(angle_in_radians), 2));
        } 
        #endregion
        #region get lower

        /// <summary>
        /// returns the vector with the lower y scale after rotating the vector by angle degrees
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector GetLower(Vector v1, Vector v2, double angle = 0)
        {
            v1 = RotateVector(v1, angle);
            v2 = RotateVector(v2, angle);
            if (v1.Y < v2.Y)
            {
                return v1;
            }
            else
            {
                return v2;
            }
        }

        /// <summary>
        /// converts points to vectors and returns the vector with the lower y scale after rotating the vectors by angle degrees
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector GetLower(Point p1, Point p2, double angle = 0)
        {
            return GetLower((Vector)p1, (Vector)p2, angle);
        } 
        #endregion
        #region get higher

        /// <summary>
        /// returns the vector with the higher y scale after rotating the vector by angle degrees
        /// </summary>
        /// <param name="v1"></param>
        /// <param name="v2"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector GetHigher(Vector v1, Vector v2, double angle = 0)
        {
            v1 = RotateVector(v1, angle);
            v2 = RotateVector(v2, angle);
            if (v1.Y > v2.Y)
            {
                return v1;
            }
            else
            {
                return v2;
            }
        }

        /// <summary>
        /// converts points to vector and returns the vector with the lower y scale after rotating the vector by angle degrees
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static Vector GetHigher(Point p1, Point p2, double angle = 0)
        {
            return GetHigher((Vector)p1, (Vector)p2, angle);
        } 
        #endregion
    }
}
