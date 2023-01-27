using NovelTech.views.usercontrols;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace NovelTech.libraries
{
    public static class margins
    {
        public static Thickness merge_centers(Control stable, Control moving)
        {
            double left = stable.Margin.Left - 0.5 * moving.Width + 0.5 * stable.Width;
            double bottom = stable.Margin.Bottom - 0.5 * moving.Height + 0.5 * stable.Height;
            return new Thickness(left, 0, 0, bottom);
        }

        public static Thickness merge_centers(Control stable, Control moving, Point offset)
        {
            double left = stable.Margin.Left - 0.5 * moving.Width + 0.5 * stable.Width + offset.X;
            double bottom = stable.Margin.Bottom - 0.5 * moving.Height + 0.5 * stable.Height + offset.Y;
            return new Thickness(left, 0, 0, bottom);
        }

        public static Thickness move_step(string direction, Thickness margin, double step = 1)
        {
            var output = margin;
            switch (direction)
            {
                case "right":
                    output.Left += step;
                    break;
                case "left":
                    output.Left -= step;
                    break;
                case "up":
                    output.Bottom += step;
                    break;
                case "down":
                    output.Bottom -= step;
                    break;

            }
            return output;
        }

        public static void center_control_by_point(Point point, Control control, bool anchor_bottom = true)
        {
            if (anchor_bottom)
            {
                control.Margin = new Thickness(point.X - control.Width / 2, 0, 0, point.Y - control.Height / 2);
            }
            else
            {
                control.Margin = new Thickness(point.X - control.Width / 2, point.Y - control.Height / 2, 0, 0);
            }
        }

        public static void center_shape_by_point(Point point, Shape shape, bool anchor_bottom = true)
        {
            if (anchor_bottom)
            {
                shape.Margin = new Thickness(point.X - shape.Width / 2, 0, 0, point.Y - shape.Height / 2);
            }
            else
            {
                shape.Margin = new Thickness(point.X - shape.Width / 2, point.Y - shape.Height / 2, 0, 0);
            }
        }

        public static Thickness update_margin(Thickness margin, double l = 0, double t = 0, double r = 0, double b = 0)
        {
            UC_machine_table.instance.ChangeAngles();
            return new Thickness(margin.Left + l, margin.Top + t, margin.Right + r, margin.Bottom + b);
        }
    }
}
