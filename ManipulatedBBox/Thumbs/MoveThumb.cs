using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ManipulatedBBox.Thumbs
{
    public class MoveThumb : Thumb
    {
        private ManipulatedContentControl designerItem;

        public MoveThumb()
        {
            DragStarted += new DragStartedEventHandler(this.MoveThumb_DragStarted);
            DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);
        }

        private void MoveThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            this.designerItem = DataContext as ManipulatedContentControl;
        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (this.designerItem != null)
            {
                Point dragDelta = new Point(e.HorizontalChange, e.VerticalChange);

                if (this.designerItem.rotateTransform != null)
                {
                    dragDelta = this.designerItem.rotateTransform.Transform(dragDelta);
                }
                switch (this.designerItem.Parent.GetType().Name.Trim())
                {
                    case "Canvas":
                        Canvas.SetLeft(this.designerItem, Canvas.GetLeft(this.designerItem) + dragDelta.X);
                        Canvas.SetTop(this.designerItem, Canvas.GetTop(this.designerItem) + dragDelta.Y);
                        break;
                    case "Grid":
                        var current_margin = this.designerItem.Margin;
                        switch (this.designerItem.VerticalAlignment.ToString())
                        {
                            case "Top":
                                this.designerItem.Margin = new Thickness(current_margin.Left + dragDelta.X, current_margin.Top + dragDelta.Y, current_margin.Right, current_margin.Bottom);
                                break;
                            case "Bottom":
                                this.designerItem.Margin = new Thickness(current_margin.Left + dragDelta.X, current_margin.Top, current_margin.Right, current_margin.Bottom - dragDelta.Y);
                                break;
                        }
                        break;
                }

            }
        }
    }
}
