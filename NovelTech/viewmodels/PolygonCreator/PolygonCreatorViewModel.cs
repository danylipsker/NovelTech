using MethodsLibrary;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using WPFLibrary;

namespace NovelTech.viewmodels.PolygonCreator
{
    public class PolygonCreatorViewModel
    {
        #region Commands

        public RelayCommand FinishCommand { get; private set; }

        #endregion

        #region Properties
        public ObservableCollection<VertexViewModel> Vertices { get; set; }
        public ObservableCollection<EdgeViewModel> Edges { get; set; }
        public bool Answered { get; set; }
        public static PolygonCreatorViewModel instance;
        #endregion

        #region Constructors
        public PolygonCreatorViewModel()
        {
            instance = this;
            Vertices = new ObservableCollection<VertexViewModel>();
            Edges = new ObservableCollection<EdgeViewModel>();
            AddCommands();
            LoadInitData();
        }
        #endregion

        #region Methods
        private void AddCommands()
        {
            FinishCommand = new RelayCommand((window) =>
            {
                //SaveFileDialog dialog = new SaveFileDialog();
                //dialog.Filter = "SVG file (*.svg)|*.svg";
                //dialog.ShowDialog();
                //if(dialog.FileName != "")
                //{
                //    Point[] points = new Point[Vertices.Count];
                //    for (int i = 0; i < Vertices.Count; i++)
                //    {
                //        points[i] = new Point(Vertices[i].X, Vertices[i].Y);
                //    }
                //    points = PointCollectionMethods.GetWithoutOffset(points);
                //    points = PointCollectionMethods.GetScaledPoints(points);
                //}

                Answered = true;
                (window as Window).Close();
            });
        }

        private void LoadInitData()
        {
            Vertices.Add(new VertexViewModel() { X = 100, Y = 100 });
            Vertices.Add(new VertexViewModel() { X = 100, Y = 300 });
            Vertices.Add(new VertexViewModel() { X = 300, Y = 300 });
            Vertices.Add(new VertexViewModel() { X = 300, Y = 100 });
            Edges.Add(new EdgeViewModel(this) { P1 = Vertices[0], P2 = Vertices[1] });
            Edges.Add(new EdgeViewModel(this) { P1 = Vertices[1], P2 = Vertices[2] });
            Edges.Add(new EdgeViewModel(this) { P1 = Vertices[2], P2 = Vertices[3] });
            Edges.Add(new EdgeViewModel(this) { P1 = Vertices[3], P2 = Vertices[0] });
        }

        public void AddVertex(EdgeViewModel oldEdge, Point newPoint)
        {
            Edges.Remove(oldEdge);
            var newVertex = new VertexViewModel() { X = newPoint.X, Y = newPoint.Y };
            Vertices.Insert(Vertices.IndexOf(oldEdge.P2), newVertex);
            Edges.Add(new EdgeViewModel(this) { P1 = oldEdge.P1, P2 = newVertex });
            Edges.Add(new EdgeViewModel(this) { P1 = newVertex, P2 = oldEdge.P2 });
        }
        public Point GetOffset()
        {

            Point[] points = new Point[Vertices.Count];
            for (int i = 0; i < Vertices.Count; i++)
            {
                points[i] = new Point(Vertices[i].X * VM_main.instance.dimensionRatio, Vertices[i].Y * VM_main.instance.dimensionRatio);
            }
            return PointCollectionMethods.GetOffset(points);
        }
        public PointCollection GetPoints()
        {
            Point[] points = new Point[Vertices.Count];
            for (int i = 0; i < Vertices.Count; i++)
            {
                points[i] = new Point(Vertices[i].X * VM_main.instance.dimensionRatio, Vertices[i].Y * VM_main.instance.dimensionRatio);
            }
            points = PointCollectionMethods.GetWithoutOffset(points);
            points = PointCollectionMethods.GetScaledPoints(points);
            points = PointCollectionMethods.GetRoundedPoints(points);
            return new PointCollection(points);
        }

        #endregion
    }
}
