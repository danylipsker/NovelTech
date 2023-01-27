using Svg;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows;

namespace MethodsLibrary
{
    public static class SVGMethods
    {
        public static Point[] GetPathFromSVG(string filename)
        {
            if (filename == "")
                return null;
            var svg = SvgDocument.Open<SvgDocument>(filename, null);
            List<Point> points = new List<Point>();
            foreach (var point in svg.Path.PathPoints)
            {
                if (points.Count > 0 && points[0].X == point.X && points[0].Y == point.Y)
                {
                    break;
                }
                else
                {
                    points.Add(new Point(point.X, point.Y));
                }
            }
            return points.ToArray();
        }


    }
}
