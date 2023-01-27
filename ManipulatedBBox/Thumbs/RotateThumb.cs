using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace ManipulatedBBox.Thumbs
{
    public class RotateThumb : Thumb
    {
        private double initialAngle;
        private Vector startVector;
        private Point centerPoint;
        private ManipulatedContentControl designerItem;
        private Canvas canvas;
        private Grid grid;

        public RotateThumb()
        {
            DragDelta += new DragDeltaEventHandler(this.RotateThumb_DragDelta);
            DragStarted += new DragStartedEventHandler(this.RotateThumb_DragStarted);
        }

        private void RotateThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as ManipulatedContentControl;

            if (this.designerItem != null)
            {
                Point startPoint;
                switch (this.designerItem.Parent.GetType().Name)
                {
                    case "Canvas":
                        this.canvas = VisualTreeHelper.GetParent(this.designerItem) as Canvas;
                        this.centerPoint = this.designerItem.TranslatePoint(
                        new Point(this.designerItem.Width * this.designerItem.RenderTransformOrigin.X,
                                  this.designerItem.Height * this.designerItem.RenderTransformOrigin.Y),
                                  this.canvas);
                        startPoint = Mouse.GetPosition(this.canvas);

                        break;
                    case "Grid":
                        this.grid = VisualTreeHelper.GetParent(this.designerItem) as Grid;
                        this.centerPoint = this.designerItem.TranslatePoint(
                        new Point(this.designerItem.Width * this.designerItem.RenderTransformOrigin.X,
                                 this.designerItem.Height * this.designerItem.RenderTransformOrigin.Y),
                                 this.grid);
                        startPoint = Mouse.GetPosition(this.grid);

                        break;
                }

                this.startVector = Point.Subtract(startPoint, this.centerPoint);
                this.initialAngle = this.designerItem.rotateTransform.Angle;
            }
        }

        private void RotateThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null)
            {
                Point currentPoint;
                switch (this.designerItem.Parent.GetType().Name)
                {
                    case "Canvas":
                        currentPoint = Mouse.GetPosition(this.canvas);


                        break;
                    case "Grid":
                        currentPoint = Mouse.GetPosition(this.grid);

                        break;
                }
                Vector deltaVector = Point.Subtract(currentPoint, this.centerPoint);

                double angle = Vector.AngleBetween(this.startVector, deltaVector);

                this.designerItem.rotateTransform.Angle = this.initialAngle + Math.Round(angle, 0);
                this.designerItem.InvalidateMeasure();
            }
        }
    }
}
