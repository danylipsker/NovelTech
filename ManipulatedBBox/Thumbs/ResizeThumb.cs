using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;

namespace ManipulatedBBox.Thumbs
{
    public class ResizeThumb : Thumb
    {
        private double angle;
        private Adorner adorner;
        private Point transformOrigin;
        private ManipulatedContentControl designerItem;
        //private Canvas canvas;
        private Visual parent;
        public static ResizeThumb instance;
        public double originalX, originalY;
        int firstOnly = 0;
        public event EventHandler Resizing;


        public ResizeThumb()
        {
            if(ResizeThumb.instance == null)
                instance = this;


            DragStarted += new DragStartedEventHandler(this.ResizeThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
            DragCompleted += new DragCompletedEventHandler(this.ResizeThumb_DragCompleted);
        }
        public ResizeThumb(object sender)
        {
            DragStarted += new DragStartedEventHandler(this.ResizeThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
            DragCompleted += new DragCompletedEventHandler(this.ResizeThumb_DragCompleted);
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = this.DataContext as ManipulatedContentControl;

            if (firstOnly <2)
            {
                firstOnly++;
                originalX = designerItem.ActualWidth;
                originalY = designerItem.ActualHeight;
            }
            if (this.designerItem != null)
            {
                this.parent = VisualTreeHelper.GetParent(this.designerItem) as Visual;

                this.transformOrigin = this.designerItem.RenderTransformOrigin;

                if (this.designerItem.rotateTransform != null)
                {
                    this.angle = this.designerItem.rotateTransform.Angle * Math.PI / 180.0;
                }
                else
                {
                    this.angle = 0.0d;
                }

                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.parent);
                if (adornerLayer != null)
                {
                    this.adorner = new Adorners.SizeAdorner(this.designerItem);
                    adornerLayer.Add(this.adorner);
                }
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null)
            {
                Resizing?.Invoke(sender, e);
                double deltaVertical, deltaHorizontal;
                // CURRENTLY vertical Alignment of cocntrol set to bottom doesnt work
                
                switch (VerticalAlignment)
                {
                    case System.Windows.VerticalAlignment.Bottom:
                        deltaVertical = Math.Min(-e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
                        switch (this.designerItem.Parent.GetType().Name.Trim())
                        {
                            case "Canvas":
                                Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                                Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) - deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle));
                                break;
                            case "Grid":
                                var current_margin = this.designerItem.Margin;
                                switch (this.designerItem.VerticalAlignment.ToString())
                                {
                                    case "Top":
                                        this.designerItem.Margin = new Thickness(current_margin.Left - deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle),
                                            current_margin.Top + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))),
                                            current_margin.Right,
                                            current_margin.Bottom);
                                        break;
                                    case "Bottom":

                                        this.designerItem.Margin = new Thickness(current_margin.Left - deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle),
                                            current_margin.Top,
                                            current_margin.Right,
                                            current_margin.Bottom - (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                                        break;
                                }
                                break;
                        }

                        this.designerItem.Height -= deltaVertical;
                        break;
                    case System.Windows.VerticalAlignment.Top:
                        deltaVertical = Math.Min(e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
                        switch (this.designerItem.Parent.GetType().Name.Trim())
                        {
                            case "Canvas":
                                Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                                Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle)));
                                break;
                            case "Grid":
                                var current_margin = this.designerItem.Margin;
                                switch (this.designerItem.VerticalAlignment.ToString())
                                {
                                    case "Top":
                                        this.designerItem.Margin = new Thickness(current_margin.Left + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle)),
                                            current_margin.Top + deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))),
                                            current_margin.Right,
                                            current_margin.Bottom);
                                        break;
                                    case "Bottom":
                                        this.designerItem.Margin = new Thickness(current_margin.Left + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle)),
                                            current_margin.Top,
                                            current_margin.Right,
                                            current_margin.Bottom - deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
                                        break;
                                }
                                break;
                        }

                        this.designerItem.Height -= deltaVertical;
                        break;
                    default:
                        break;
                }

                switch (HorizontalAlignment)
                {
                    case System.Windows.HorizontalAlignment.Left:
                        deltaHorizontal = Math.Min(e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);

                        switch (this.designerItem.Parent.GetType().Name.Trim())
                        {
                            case "Canvas":
                                Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
                                Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle))));
                                break;
                            case "Grid":
                                var current_margin = this.designerItem.Margin;
                                switch (this.designerItem.VerticalAlignment.ToString())
                                {
                                    case "Top":
                                        this.designerItem.Margin = new Thickness(current_margin.Left + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle))),
                                            current_margin.Top + deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle),
                                            current_margin.Right,
                                            current_margin.Bottom);
                                        break;
                                    case "Bottom":
                                        this.designerItem.Margin = new Thickness(current_margin.Left + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle))),
                                           current_margin.Top,
                                           current_margin.Right,
                                           current_margin.Bottom - (deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle)));
                                        break;
                                }
                                break;
                        }

                        this.designerItem.Width -= deltaHorizontal;
                        break;
                    case System.Windows.HorizontalAlignment.Right:
                        deltaHorizontal = Math.Min(-e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);
                        switch (this.designerItem.Parent.GetType().Name.Trim())
                        {
                            case "Canvas":
                                Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
                                Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle))));
                                break;
                            case "Grid":
                                var current_margin = this.designerItem.Margin;
                                switch (this.designerItem.VerticalAlignment.ToString())
                                {
                                    case "Top":
                                        this.designerItem.Margin = new Thickness(current_margin.Left + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle))),
                                            current_margin.Top - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle),
                                            current_margin.Right,
                                            current_margin.Bottom);
                                        break;
                                    case "Bottom":
                                        this.designerItem.Margin = new Thickness(current_margin.Left + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle))),
                                            current_margin.Top,
                                            current_margin.Right,
                                            current_margin.Bottom + this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
                                        break;
                                }
                                break;
                        }

                        this.designerItem.Width -= deltaHorizontal;
                        break;
                    default:
                        break;
                }
            }

            e.Handled = true;
        }

        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (this.adorner != null)
            {
                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.parent);
                if (adornerLayer != null)
                {
                    adornerLayer.Remove(this.adorner);
                }

                this.adorner = null;
            }
        }

        public ManipulatedContentControl shape
        {
            get { return this.DataContext as ManipulatedContentControl; }
        }

    }

    //public class ResizeThumb : Thumb
    //{
    //    private double angle;
    //    private Adorner adorner;
    //    private Point transformOrigin;
    //    private ManipulatedContentControl designerItem;
    //    //private Canvas canvas;
    //    private Visual parent;

    //    public ResizeThumb()
    //    {
    //        DragStarted += new DragStartedEventHandler(this.ResizeThumb_DragStarted);
    //        DragDelta += new DragDeltaEventHandler(this.ResizeThumb_DragDelta);
    //        DragCompleted += new DragCompletedEventHandler(this.ResizeThumb_DragCompleted);
    //    }

    //    private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
    //    {
    //        this.designerItem = this.DataContext as ManipulatedContentControl;

    //        if (this.designerItem != null)
    //        {
    //            this.parent = VisualTreeHelper.GetParent(this.designerItem) as Visual;
    //            //this.canvas = VisualTreeHelper.GetParent(this.designerItem) as Canvas;

    //            this.transformOrigin = this.designerItem.RenderTransformOrigin;

    //            if (this.designerItem.rotateTransform != null)
    //            {
    //                this.angle = this.designerItem.rotateTransform.Angle * Math.PI / 180.0;
    //            }
    //            else
    //            {
    //                this.angle = 0.0d;
    //            }

    //            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.parent);
    //            if (adornerLayer != null)
    //            {
    //                this.adorner = new Adorners.SizeAdorner(this.designerItem);
    //                adornerLayer.Add(this.adorner);
    //            }  
    //        }
    //    }

    //    private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
    //    {
    //        if (this.designerItem != null)
    //        {
    //            double deltaVertical, deltaHorizontal;

    //            switch (VerticalAlignment)
    //            {
    //                case System.Windows.VerticalAlignment.Bottom:
    //                    deltaVertical = Math.Min(-e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
    //                    switch (this.designerItem.Parent.GetType().Name.Trim())
    //                    {
    //                        case "Canvas":
    //                            Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
    //                            Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) - deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle));
    //                            break;
    //                        case "Grid":
    //                            var current_margin = this.designerItem.Margin;
    //                            switch (this.designerItem.VerticalAlignment.ToString())
    //                            {
    //                                case "Top":
    //                                    //MessageBox.Show("t");
    //                                    this.designerItem.Margin = new Thickness(
    //                                        current_margin.Left - deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle),
    //                                        current_margin.Top + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))),
    //                                        current_margin.Right,
    //                                        current_margin.Bottom);
    //                                    break;
    //                                case "Bottom":
    //                                    //MessageBox.Show("b");
    //                                    this.designerItem.Margin = new Thickness(
    //                                        current_margin.Left - deltaVertical * this.transformOrigin.Y * Math.Sin(-this.angle),
    //                                        current_margin.Top,
    //                                        current_margin.Right,
    //                                        current_margin.Bottom  (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
    //                                    break;
    //                            }
    //                            break;
    //                    }

    //                    this.designerItem.Height -= deltaVertical;
    //                    break;
    //                case System.Windows.VerticalAlignment.Top:
    //                    deltaVertical = Math.Min(e.VerticalChange, this.designerItem.ActualHeight - this.designerItem.MinHeight);
    //                    switch (this.designerItem.Parent.GetType().Name.Trim())
    //                    {
    //                        case "Canvas":
    //                            Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
    //                            Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle)));
    //                            break;
    //                        case "Grid":
    //                            var current_margin = this.designerItem.Margin;
    //                            switch (this.designerItem.VerticalAlignment.ToString())
    //                            {
    //                                case "Top":
    //                                    this.designerItem.Margin = new Thickness(current_margin.Left + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle)),
    //                                        current_margin.Top + deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))),
    //                                        current_margin.Right,
    //                                        current_margin.Bottom);
    //                                    break;
    //                                case "Bottom":
    //                                    this.designerItem.Margin = new Thickness(current_margin.Left + deltaVertical * Math.Sin(-this.angle) - (this.transformOrigin.Y * deltaVertical * Math.Sin(-this.angle)),
    //                                        current_margin.Top,
    //                                        current_margin.Right,
    //                                        current_margin.Bottom - deltaVertical * Math.Cos(-this.angle) + (this.transformOrigin.Y * deltaVertical * (1 - Math.Cos(-this.angle))));
    //                                    break;
    //                            }
    //                            break;
    //                    }

    //                    this.designerItem.Height -= deltaVertical;
    //                    break;
    //                default:
    //                    break;
    //            }

    //            switch (HorizontalAlignment)
    //            {
    //                case System.Windows.HorizontalAlignment.Left:
    //                    deltaHorizontal = Math.Min(e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);

    //                    switch (this.designerItem.Parent.GetType().Name.Trim())
    //                    {
    //                        case "Canvas":
    //                            Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
    //                            Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle))));
    //                            break;
    //                        case "Grid":
    //                            var current_margin = this.designerItem.Margin;
    //                            switch (this.designerItem.VerticalAlignment.ToString())
    //                            {
    //                                case "Top":
    //                                    this.designerItem.Margin = new Thickness(current_margin.Left + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle))),
    //                                        current_margin.Top + deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle),
    //                                        current_margin.Right,
    //                                        current_margin.Bottom);
    //                                    break;
    //                                case "Bottom":
    //                                    this.designerItem.Margin = new Thickness(current_margin.Left + deltaHorizontal * Math.Cos(this.angle) + (this.transformOrigin.X * deltaHorizontal * (1 - Math.Cos(this.angle))),
    //                                       current_margin.Top,
    //                                       current_margin.Right,
    //                                       current_margin.Bottom - deltaHorizontal * Math.Sin(this.angle) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
    //                                    break;
    //                            }
    //                            break;
    //                    }

    //                    this.designerItem.Width -= deltaHorizontal;
    //                    break;
    //                case System.Windows.HorizontalAlignment.Right:
    //                    deltaHorizontal = Math.Min(-e.HorizontalChange, this.designerItem.ActualWidth - this.designerItem.MinWidth);
    //                    switch (this.designerItem.Parent.GetType().Name.Trim())
    //                    {
    //                        case "Canvas":
    //                            Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
    //                            Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle))));
    //                            break;
    //                        case "Grid":
    //                            var current_margin = this.designerItem.Margin;
    //                            switch (this.designerItem.VerticalAlignment.ToString())
    //                            {
    //                                case "Top":
    //                                    this.designerItem.Margin = new Thickness(current_margin.Left + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle))),
    //                                        current_margin.Top - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle),
    //                                        current_margin.Right,
    //                                        current_margin.Bottom);
    //                                    break;
    //                                case "Bottom":
    //                                    this.designerItem.Margin = new Thickness(current_margin.Left + (deltaHorizontal * this.transformOrigin.X * (1 - Math.Cos(this.angle))),
    //                                        current_margin.Top - this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle),
    //                                        current_margin.Right,
    //                                        current_margin.Bottom + this.transformOrigin.X * deltaHorizontal * Math.Sin(this.angle));
    //                                    break;
    //                            }
    //                            break;
    //                    }

    //                    this.designerItem.Width -= deltaHorizontal;
    //                    break;
    //                default:
    //                    break;
    //            }
    //        }

    //        e.Handled = true;
    //    }

    //    private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
    //    {
    //        if (this.adorner != null)
    //        {
    //            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this.parent);
    //            if (adornerLayer != null)
    //            {
    //                adornerLayer.Remove(this.adorner);
    //            }

    //            this.adorner = null;
    //        }
    //    }
    //}
}
