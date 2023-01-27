using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using ManipulatedBBox.Thumbs;
using MethodsLibrary;
using MVVMLibrary;
using MVVMLibrary.ViewModels;
using NovelTech.models.tools;
using NovelTech.views.usercontrols;
using NovelTech.views.windows.tools;
using WPFLibrary;

namespace NovelTech.viewmodels
{
    public class VM_machine_table : BaseViewModel
    {
        #region Commands
        public RelayCommand EmualteSawingCommand { get; private set; }
        public RelayCommand SawingGCodeCommand { get; private set; }
        public RelayCommand EditToolCommand { get; private set; }

        #endregion
        #region Properties

        #region Property - machineWidth
        private double _machineWidth = 1000;

        public double machineWidth
        {
            get { return _machineWidth; }
            set { _machineWidth = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - machineHeight
        private double _machineHeight = 600;

        public double machineHeight
        {
            get { return _machineHeight; }
            set { _machineHeight = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - pincherX
        private double _pincherX = 0;

        public double pincherX
        {
            get { return _pincherX; }
            set { _pincherX = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - pincherY
        private double _pincherY = 0;

        public double pincherY
        {
            get { return _pincherY; }
            set { _pincherY = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - pincherAngle
        private double _pincherAngle = 0;

        public double pincherAngle
        {
            get { return _pincherAngle; }
            set { _pincherAngle = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - tools
        private ObservableCollection<Tool_in_action> _tools = new ObservableCollection<Tool_in_action>();

        public ObservableCollection<Tool_in_action> tools
        {
            get { return _tools; }
            set { 
                _tools = value;                
                OnPropertyChanged();
                filterTools();
            }
        }


        #endregion

        public bool gCodesCreated = false;

        public bool pincherSafe = false;

        #endregion
        #region Fields
        private classes.Sawing.Sawing_manager sawer;
        public static VM_machine_table instance;
        public UC_machine_table uiMachineTable; // need to remove
        public ItemsControl uiTools; // need to remove
        #endregion
        #region Constructor
        public VM_machine_table()
        {
            instance = this;

        }
        #endregion
        #region Methods
        protected override void AddCommands()
        {
            EditToolCommand = new RelayCommand(EditTool);
            EmualteSawingCommand = new RelayCommand(o => EmulateSawing(), o => tools.Count >= 0 && ResizeThumb.instance != null );

            SawingGCodeCommand = new RelayCommand(o =>
            {
                //double[] angles = libraries.polygons.calc_angles_to_align(uc_shape.vm.polygon, (Vector)uc_shape.vm.center);
                //var sawer = new classes.Sawing.Sawing_manager
                //    (angles, uc_machine_table, uc_shape, uc_material, uc_pincher, uc_tool,0);
                new libraries.ui.popups.Popup_text_document(sawer.gcodes).ShowDialog();
            }, o => gCodesCreated);
        }

        private void EditTool(object tool)
        {
            UC_tool uC_Tool = tool as UC_tool;

            new W_edit_tool_in_action(new tools.EditToolEquippedViewModel(tools[ChooseToolByPosition(uC_Tool.position)]) { mode = viewmodels.tools.Mode.Edit }).ShowDialog();
        }

        private int ChooseToolByPosition(double position)
        {
            switch (position)
            {
                default:
                    return 0;
                case 225:
                    return 1;
                case 425:
                    return 2;
                case 125:
                    return 3;
                case 325:
                    return 4;
                case 525:
                    return 5;
            }
        }

        //here we start the proccess of sawing 
        private void EmulateSawing()
        {
            //check if pincher is inside the shape and if the shape is inside the material if not return
            if (!VM_shape.instance.CheckPincherInsideShape((float)pincherX, (float)pincherY) || !VM_shape.instance.checkShapeInsideMaterial((float)pincherX, (float)pincherY))
                return;
            //getting the angels of the lines forming the shape
            double[] angles = PolygonMethods.GetAlignAngles(VM_shape.instance.polygon, pincherAngle);

            //System.Windows.Controls.Primitives.Selector.SetIsSelected(bbox_shape.control, false);
            var uc_tool_saw = (uiTools.ItemContainerGenerator.ContainerFromItem(tools[0]) as FrameworkElement).FindVisualChild<UC_tool>();
            var uc_tool_drill = (uiTools.ItemContainerGenerator.ContainerFromItem(tools[1]) as FrameworkElement).FindVisualChild<UC_tool>();

            //by calling this new sawing manager the animation is starting
            sawer = new classes.Sawing.Sawing_manager(angles, VM_shape.instance.drillPoints, uiMachineTable, VM_shape.instance.uiShape, VM_material.instance.uiMaterial, UC_machine_table.instance.uC_Tools[0], UC_machine_table.instance.uC_Tools[1]);
        }
        
        #region Loading
        public void load_data()
        {
            //uc_pincher = new UC_pincher();
            //uc_machine_table.g_machine.Children.Add(uc_pincher);
            locate_tools();
        }
        #endregion
        #region Tools
        public void locate_tools()
        {
            //uc_machine_table.g_machine.Children.Clear();
            //foreach (var tool in VM_tools.toolBox.equipped)
            //{
            //    UC_tool uc = new UC_tool(tool);
            //    switch (uc.tool.orientation)
            //    {
            //        case models.tools.Tool_in_action.Orientaions.Head:
            //            uc.Margin = new System.Windows.Thickness
            //                (uc.tool.position * VM_main.instance.dimensionRatio, -uc.Height - 10, 0, 0);
            //            break;
            //        case models.tools.Tool_in_action.Orientaions.Left:
            //            uc.Margin = new System.Windows.Thickness
            //                (-80, uc.tool.position * VM_main.instance.dimensionRatio - uc.Width / 2, 0, 0);
            //            uc.RenderTransform = new RotateTransform(270, uc.Width / 2, uc.Height / 2);
            //            break;

            //        case models.tools.Tool_in_action.Orientaions.Right:
            //            uc.Margin = new System.Windows.Thickness
            //                (580, uc.tool.position * VM_main.instance.dimensionRatio - uc.Width / 2, 0, 0);
            //            uc.RenderTransform = new RotateTransform(90, uc.Width / 2, uc.Height / 2);
            //            break;
            //    }
            //    tools.Add(uc);
            //    uc_machine_table.g_machine.Children.Add(uc);
            //}
        }
        #endregion
        private void filterTools()
        {
            Predicate<Tool_in_action> predicate = t => t.orientation == Orientaions.Head;
            tools.Filter(t =>  predicate(t as Tool_in_action) ? true : false);

            //filterTools(tools, t => t.orientation == Tool_in_action.Orientaions.Head);
            //void filterTools(Collection<Tool_in_action> collection, Predicate<Tool_in_action> predicate)
            //{
            //    return collection.Filter(t =>
            //    {
            //        if (predicate(t as Tool_in_action))
            //            return true;

            //        return false;
            //    }) > 0;
            //}
        }
        #endregion
    }
}
