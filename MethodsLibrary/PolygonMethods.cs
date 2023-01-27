using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace MethodsLibrary
{
    public static class PolygonMethods
    {
        //checks the angles of the lines forming the shape
        //posiable better implamintation using trigonometry
        public static double[] GetAlignAngles(Polygon polygon, double angle, int roundDigits = 2)
        {
            double[] output = new double[polygon.Points.Count];
            for (int i = 0; i < polygon.Points.Count; i++)
            {
                Vector v1;
                Vector v2;
                if (i < polygon.Points.Count - 1)
                {
                    v1 = (Vector)GetPointAbsolute(polygon, i, Application.Current.MainWindow);
                    v2 = (Vector)GetPointAbsolute(polygon, i + 1, Application.Current.MainWindow);

                    var v3 = VectorMethods.RotateVector(v1, angle);
                    var v4 = VectorMethods.RotateVector(v2, angle);
                }
                else
                {
                    v1 = (Vector)GetPointAbsolute(polygon, i, Application.Current.MainWindow);
                    v2 = (Vector)GetPointAbsolute(polygon, 0, Application.Current.MainWindow);
                }


                double angle2 = 0.000;
                do
                {
                    if (angle2 < 360)
                    {
                        angle2 = Math.Round(angle2 + 0.001, 3);
                    }
                    else
                    {
                        break;
                    }
                }
                while (VectorMethods.RotateVector(v1, angle2).X !=
                    VectorMethods.RotateVector(v2, angle2).X);
                output[i] = Math.Round(angle2, roundDigits);
            }
            return output;
        }

        public static Point[] GetPolygonAbsolute(Polygon polygon, ContentControl ancestor)
        {
            Point[] output = new Point[polygon.Points.Count];
            var polygonGeometryTransform = polygon.RenderedGeometry.Transform;
            var polygonToGridTransform = polygon.TransformToAncestor(ancestor);
            for (int i = 0; i < polygon.Points.Count; i++)
            {
                var transformedPoint = polygonToGridTransform.Transform(
                                       polygonGeometryTransform.Transform(polygon.Points[i]));
                output[i] = new Point(transformedPoint.X, Math.Round(transformedPoint.Y, 2));
            }

            return output;
        }

        public static Point GetPointAbsolute(Polygon polygon, Point point, ContentControl ancestor)
        {
            var polygonGeometryTransform = polygon.RenderedGeometry.Transform;
            var polygonToGridTransform = polygon.TransformToAncestor(ancestor);
            var transformedPoint = polygonToGridTransform.Transform(
                                       polygonGeometryTransform.Transform(point));
            return new Point(Math.Round(transformedPoint.X, 2), Math.Round(transformedPoint.Y, 2));
        }

        public static Point GetPointAbsolute(Polygon polygon, int index, ContentControl ancestor)
        {
            var polygonGeometryTransform = polygon.RenderedGeometry.Transform;
            var polygonToGridTransform = polygon.TransformToAncestor(ancestor);
            var transformedPoint = polygonToGridTransform.Transform(
                                       polygonGeometryTransform.Transform(polygon.Points[index]));
            return new Point(Math.Round(transformedPoint.X, 2), Math.Round(transformedPoint.Y, 2));
        }

        /// <summary>
        /// checks for each point in a polygon whether a line of the poligon intesects with aline created by source and target 
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GetIsPointIntersect(Polygon polygon, Point source, Point target)
        {
            
            for (int i = 1; i < polygon.Points.Count; i++)
            {
                

                Point p1 = GetPointAbsolute(polygon, i - 1, Application.Current.MainWindow);
                Point p2 = GetPointAbsolute(polygon, i, Application.Current.MainWindow);

                p1.Y = Application.Current.MainWindow.ActualHeight - p1.Y;
                p2.Y = Application.Current.MainWindow.ActualHeight - p2.Y;

                if (p1.X == source.X || p2.X == source.X)
                {
                    continue;
                }

                if (IsIntersecting(source, target, p1, p2))
                    return true;
            }
            return false;
            
        }
        public static bool IsIntersecting(Point a, Point b, Point c, Point d)
        {
            var denominator = ((b.X - a.X) * (d.Y - c.Y)) - ((b.Y - a.Y) * (d.X - c.X));
            var numerator1 = ((a.Y - c.Y) * (d.X - c.X)) - ((a.X - c.X) * (d.Y - c.Y));
            var numerator2 = ((a.Y - c.Y) * (b.X - a.X)) - ((a.X - c.X) * (b.Y - a.Y));

            // Detect coincident lines (has a problem, read below)
            if (denominator == 0) return numerator1 == 0 && numerator2 == 0;

            var r = numerator1 / denominator;
            var s = numerator2 / denominator;

            return (r >= 0 && r <= 1) && (s >= 0 && s <= 1);
        }
    }
}
