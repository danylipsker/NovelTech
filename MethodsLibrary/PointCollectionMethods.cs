using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MethodsLibrary
{
    public static class PointCollectionMethods
    {
        #region SVG
        public static PointCollection GetPointsFromSVG(string filename, double scale = 1)
        {
            var points = SVGMethods.GetPathFromSVG(filename);
            points = GetWithoutOffset(points);
            points = GetScaledPoints(points, scale);
            points = GetRoundedPoints(points);
            return new PointCollection(points);
        }
        #endregion
        #region Manipulations
        public static Point[] GetWithoutOffset(Point[] points)
        {
            Point[] output = new Point[points.Length];
            var offset = GetOffset(points);

            for (int i = 0; i < points.Length; i++)
            {
                output[i] = new Point(points[i].X - offset.X, points[i].Y - offset.Y);
            }
            return output;
        }

        public static Point[] GetRoundedPoints(Point[] points, int roundDigits = 2)
        {
            Point[] output = new Point[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                output[i] = new Point(Math.Round(points[i].X, roundDigits), Math.Round(points[i].Y, roundDigits));
            }
            return output;
        }

        public static Point[] GetScaledPoints(Point[] points, double scale = 1)
        {
            Point[] output = new Point[points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                output[i] = new Point(points[i].X * scale, points[i].Y * scale);
            }
            return output;
        }
        #endregion

        #region Calculations
        public static Point GetCenter(PointCollection points)
        {
            Point centroid =
                points.Aggregate(
                    new { xSum = 0.0, ySum = 0.0, n = 0 },
                    (acc, p) => new
                    {
                        xSum = acc.xSum + p.X,
                        ySum = acc.ySum + p.Y,
                        n = acc.n + 1
                    },
                    acc => new Point(acc.xSum / acc.n, acc.ySum / acc.n));

            return centroid;
        }


        public static Point GetOffset(Point[] points)
        {
            double offset_x = 0;
            double offset_y = 0;
            for (int i = 0; i < points.Length; i++)
            {
                if (i == 0)
                {
                    offset_x = points[i].X;
                    offset_y = points[i].Y;
                }
                else
                {
                    if (points[i].X < offset_x)
                    {
                        offset_x = points[i].X;
                    }
                    if (points[i].Y < offset_y)
                    {
                        offset_y = points[i].Y;
                    }
                }
            }
            return new Point(offset_x, offset_y);
        }

        public static Size GetSize(PointCollection points, int roundDigits = 2)
        {
            double left_x = 0;
            double right_x = 0;

            double high_y = 0;
            double low_y = 0;

            for (int i = 0; i < points.Count; i++)
            {
                if (i == 0)
                {
                    left_x = points[i].X;
                    right_x = points[i].X;
                    high_y = points[i].Y;
                    low_y = points[i].Y;
                }
                else
                {
                    if (points[i].X < left_x)
                    {
                        left_x = points[i].X;
                    }
                    else if (points[i].X > right_x)
                    {
                        right_x = points[i].X;
                    }

                    if (points[i].Y < low_y)
                    {
                        low_y = points[i].Y;
                    }
                    else if (points[i].Y > high_y)
                    {
                        high_y = points[i].Y;
                    }
                }
            }
            return new Size(Math.Round(right_x - left_x, roundDigits), Math.Round(high_y - low_y, roundDigits));
        }
        #endregion
    }
}
