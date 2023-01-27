using MethodsLibrary;
using MVVMLibrary;
using MVVMLibrary.Attributes;
using MVVMLibrary.ViewModels;
using NovelTech.views.usercontrols;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFLibrary;
using ManipulatedBBox.Thumbs;
using ManipulatedBBox;

namespace NovelTech.viewmodels
{
    public class VM_shape : BaseViewModel
    {
        #region Commands
        public RelayCommand CreateShapeCommand { get; private set; }
        public RelayCommand ImportShapeCommand { get; private set; }

        #endregion
        #region Properties

        #region Property - isShapeImported
        private bool _isShapeImported = false;

        public bool isShapeImported
        {
            get { return _isShapeImported; }
            set { _isShapeImported = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - points
        private PointCollection _points;

        public PointCollection points
        {
            get { return _points; }
            set { _points = value; OnPropertyChanged(); }
        }

        private PointCollection _drillPoints;

        public PointCollection drillPoints
        {
            get { return _drillPoints; }
            set { _drillPoints = value; OnPropertyChanged(); }
        }

        private List<double> _drillPointsDepths;

        public List<double> drillPointsDepths
        {
            get { return _drillPointsDepths; }
            set { _drillPointsDepths = value; OnPropertyChanged(); }
        }
        #endregion
        #region Property - shapeSize
        private Size _shapeSize = new Size();
        public Size shapeSize
        {
            get { return _shapeSize; }
            set { _shapeSize = value; OnPropertyChanged(); }
        }
        #endregion



        #endregion
        #region Fields
        public static VM_shape instance;
        #region UI Controls For Calculations
        public Polygon polygon;
        public UC_shape uiShape; //need to remove
        #endregion
        #endregion
        #region Constrcutor
        public VM_shape()
        {
            instance = this;
        }
        #endregion
        protected override void AddCommands()
        {
            CreateShapeCommand = new RelayCommand(o =>
            {
                var dataContext = new PolygonCreator.PolygonCreatorViewModel();
                new views.windows.PolygonCreator.PolygonCreatorView() { DataContext = dataContext }.ShowDialog();   
                if(dataContext.Answered)
                {
                    isShapeImported = true;
                    Point offset = dataContext.GetOffset();
                    drillPoints = NovelTech.views.windows.PolygonCreator.PolygonCreatorView.instance.GetDrillPointsAsPoints(offset);
                    drillPointsDepths = NovelTech.views.windows.PolygonCreator.PolygonCreatorView.instance.getGetDrillPointsDepths(offset);
                    points = dataContext.GetPoints();
                    shapeSize = PointCollectionMethods.GetSize(points);
                    #region UC_machine_table values
                    UC_machine_table.instance.AddPointsToList(points);//for showing the distance between each point
                    UC_machine_table.instance.SetStartValuesHeightWidth(shapeSize.Width, shapeSize.Height);//for showing the starting values of the shape 
                    #endregion
                    VM_material.instance.bboxMargin = new Thickness(
                    VM_material.instance.materialWidth / 2 - shapeSize.Width / 2, 0, 0,
                    VM_material.instance.materialHeight / 2 - shapeSize.Height / 2);

                }
            });

            ImportShapeCommand = new RelayCommand(o =>
            {
                var answer = libraries.fileDialogs.create_openFileDialog(filter: "SVG file (*.svg)|*.svg");
                if (answer != null)
                {
                    isShapeImported = true;
                    points = PointCollectionMethods.GetPointsFromSVG(answer, VM_main.instance.dimensionRatio);
                    shapeSize = PointCollectionMethods.GetSize(points);
                    VM_material.instance.bboxMargin = new Thickness(
                    VM_material.instance.materialWidth / 2 - shapeSize.Width / 2, 0, 0,
                    VM_material.instance.materialHeight / 2 - shapeSize.Height / 2);
                    //LoadShape(answer);
                    //if (bbox_shape != null)
                    //{
                    //    //uc_material.g_material.Children.Remove(bbox_shape);
                    //}
                    //vm_main.Is_shape_imported = true;

                    //bbox_shape = new ManipulatedBBox.BoundingBox_grid() { VerticalAlignment = VerticalAlignment.Bottom, HorizontalAlignment = HorizontalAlignment.Left };
                    //uc_shape = new UC_shape(answer[0], uc_material, bbox_shape);
                }
            });
        }

        /// <summary>
        /// check if the pincher is inside the shape
        /// </summary>
        /// <param name="pincherX"></param>
        /// <param name="pincherY"></param>
        /// <returns></returns>
        public bool CheckPincherInsideShape(float pincherX,float pincherY)
        {
            PointCollection actualPoints = new PointCollection();

            for (int i = 0; i< points.Count; i++)
            {
                actualPoints.Add(PolygonMethods.GetPointAbsolute(polygon, i, Application.Current.MainWindow));
            }

            double originalX = DesignerItemDecorator.instance.originalX;
            double originalY = DesignerItemDecorator.instance.originalY;
            //Vector offset = ResizeThumb.instance.shape.offset;

            double xRatio = ResizeThumb.instance.shape.ActualWidth / originalX;
            double yRatio = ResizeThumb.instance.shape.ActualHeight / originalY;

            for (int i = 0; i < 4; i++)
            {
                int intersectioncount = 0;
                float checkingPointx= pincherX;
                float checkingPointY= pincherY;
                //moves the relative point to actual point posable problems with changed resolution
                checkingPointx += 240;
                checkingPointY = 600 - checkingPointY;



                //check for four points left down, left up, right up, right down in that order
                switch (i)
                {
                    case 0:
                        break;
                    case 1:
                         checkingPointY -= 37.5f;
                        break;
                    case 2:
                         checkingPointx +=37.5f;
                         checkingPointY -=37.5f;
                        break;
                    case 3:
                        checkingPointx += 37.5f;
                        break;

                }

                //a line streched to infinity from a point inside a polygon will have an odd number of intersections
                for (int j = 0; j< actualPoints.Count; j++)
                {
                    if(j != actualPoints.Count-1)
                    {
                        if (intersect(checkingPointx, checkingPointY, 10000, 0, (float)actualPoints[j].X, (float)actualPoints[j].Y, (float)actualPoints[j + 1].X, (float)actualPoints[j + 1].Y))
                            intersectioncount++;
                    }
                    else
                    {
                        if (intersect(checkingPointx, checkingPointY, 10000, 0, (float)actualPoints[j].X, (float)actualPoints[j].Y, (float)actualPoints[0].X, (float)actualPoints[0].Y))
                            intersectioncount++;
                    }
                }

                if (intersectioncount % 2 == 0)
                    return false;
            }

            return true;
        }
        //checks if line AB intersects with line CD
        bool intersect(float ax, float ay, float bx, float by, float cx, float cy,float dx,float dy)
        {
            return IntersectSubFunction(ax, ay, cx, cy, dx, dy) != IntersectSubFunction(bx, by, cx, cy, dx, dy) && IntersectSubFunction(ax, ay, bx, by, cx, cy) != IntersectSubFunction(ax, ay, bx, by, dx, dy);
        }
        bool IntersectSubFunction(float ax, float ay, float bx, float by, float cx, float cy)
        {
            return (cy - ay) * (bx - ax) > (by - ay) * (cx - ax);
        }

        /// <summary>
        /// returns true if the shape is inside of the material
        /// </summary>
        /// <param name="pincherX"></param>
        /// <param name="pincherY"></param>
        /// <returns></returns>
        public bool checkShapeInsideMaterial(float pincherX, float pincherY)
        {
            //moves the relative point to actual point posable problems with changed resolution
            pincherX += 240;
            pincherY = 600 - pincherY;

            PointCollection actualPoints = new PointCollection();

            for (int i = 0; i < points.Count; i++)
            {
                actualPoints.Add(PolygonMethods.GetPointAbsolute(polygon, i, Application.Current.MainWindow));
            }

            for(int i= 0;i< actualPoints.Count; i++)
            {
                if (actualPoints[i].X > pincherX + VM_material.instance.materialRealWidth / 2 || actualPoints[i].X < pincherX - VM_material.instance.materialRealWidth / 2 || actualPoints[i].Y > pincherY + VM_material.instance.materialRealHeight / 2 || actualPoints[i].Y < pincherY - VM_material.instance.materialRealHeight / 2)
                    return false;
            }
            return true;
        }

        //public void LoadShape(string filename)
        //{
        //    #region Polygon
        //    points = PointCollectionMethods.GetPointsFromSVG(filename, VM_main.dimensionRatio);
        //    shapeSize = PointCollectionMethods.GetSize(points);

        //    //polygon.Stroke = Brushes.Black;
        //    //polygon.HorizontalAlignment = HorizontalAlignment.Left;
        //    //polygon.VerticalAlignment = VerticalAlignment.Bottom;
        //    //polygon.Stretch = Stretch.Fill;
        //    //uc_shape.g_shape.Children.Add(polygon);

        //    #endregion

        //    #region Update BoundingBox
        //    //uc_shape.box.Width = size.Width;
        //    //uc_shape.box.Height = size.Height;
        //    //uc_shape.box.control.Width = size.Width;
        //    //uc_shape.box.control.Height = size.Height;
        //    //System.Windows.Controls.Panel.SetZIndex(uc_shape.box, 15);
        //    #endregion

        //    #region Locating

        //    //uc_shape.box.Margin = new Thickness(
        //    //    uc_material.ActualWidth/2 - size.Width/2,
        //    //    0,
        //    //    0,
        //    //    uc_material.ActualHeight / 2 - size.Height / 2
        //    //    );
        //    #endregion
        //}
    }
}
