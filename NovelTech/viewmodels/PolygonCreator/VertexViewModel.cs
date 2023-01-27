using MVVMLibrary.ViewModels;
using NovelTech.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using NovelTech.views.windows.PolygonCreator;

namespace NovelTech.viewmodels.PolygonCreator
{
    public class VertexViewModel : BaseViewModel, IDraggabled
    {
        //#region Events
        //public event EventHandler<Point> OnPositionChanged;
        //#endregion
        #region Properties
        private double x;

        public double X
        {
            get { return x; }
            set { x = value; OnPropertyChanged(); }
        }

        private double _depth;

        public double depth
        {
            get { return _depth; }
            set { _depth = value; OnPropertyChanged(); }
        }

        private double y;

        public double Y
        {
            get { return y; }
            set { y = value; OnPropertyChanged(); }
        }

        #endregion

        #region Methods
        public void PositionChanged(Point point)
        {
            X = point.X;
            Y = point.Y;
            PolygonCreatorView.instance.UpdateText(this);
        }
        #endregion
    }
}
