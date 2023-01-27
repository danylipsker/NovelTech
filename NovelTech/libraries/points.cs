using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace NovelTech.libraries
{
    public static class points
    {
        public static Point get_center(Control control)
        {
            return new Point(control.Margin.Left + control.Width / 2, control.Margin.Bottom + control.Height / 2);
        }

        public static Point get_reversed_point(Point point, double container_height)
        {
            return new Point(point.X, container_height - point.Y);
        }

        public static Point get_two_points_distance(Point p1, Point p2)
        {
            return (Point)((Vector)p1 - (Vector)p2);
        }

        public static Point get_absolute(Control visual, ContentControl ancestor, bool bottom = true)
        {
            var p = visual.TransformToAncestor(ancestor).Transform(new Point(0, 0));
            if (bottom)
            {
                p.Y = ancestor.Height - p.Y - visual.ActualHeight;
            }
            else
            {
                p.Y = ancestor.Height - p.Y;
            }
            return p;
        }

        public static Point get_absolute(Shape visual, ContentControl ancestor, bool bottom = true)
        {
            var p = visual.TransformToAncestor(ancestor).Transform(new Point(0, 0));
            if (bottom)
            {
                p.Y = ancestor.Height - p.Y - visual.Height;
            }
            return p;
        }

        public static Point get_absolute(Panel visual, ContentControl ancestor, bool bottom = true)
        {
            var p = visual.TransformToAncestor(ancestor).Transform(new Point(0, 0));
            if (bottom)
            {
                p.Y = ancestor.Height - p.Y - visual.Height;
            }
            return p;
        }

        public static Point get_absolute_center(Shape visual, ContentControl ancestor, bool bottom = true)
        {
            var p = visual.TransformToAncestor(ancestor).Transform(new Point(visual.Width / 2, visual.Height / 2));
            if (bottom)
            {
                p.Y = ancestor.Height - p.Y;
            }
            return p;
        }

        public static Point get_absolute_center(Panel visual, ContentControl ancestor, bool bottom = true)
        {
            var p = visual.TransformToAncestor(ancestor).Transform(new Point(visual.Width / 2, visual.Height / 2));
            if (bottom)
            {
                p.Y = ancestor.Height - p.Y;
            }
            return p;
        }

        public static Point get_lower(Point p1, Point p2)
        {
            if (p1.Y < p2.Y)
            {
                return p1;
            }
            else
            {
                return p2;
            }
        }

        public static Point get_higher(Point p1, Point p2)
        {
            if (p1.Y > p2.Y)
            {
                return p1;
            }
            else
            {
                return p2;
            }
        }

        public static Point[] ret_sub_points(Point origin, Point[] points)
        {
            Point[] output = new Point[points.Length];

            for (int i = 0; i < points.Length; i++)
            {
                output[i] = new Point(points[i].X - origin.X, points[i].Y - origin.Y);
            }
            return output;
        }
    }
}
