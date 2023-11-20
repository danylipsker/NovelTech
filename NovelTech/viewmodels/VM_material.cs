using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using System.Xml;
using MVVMLibrary.ViewModels;
using NovelTech.models.materials;
using NovelTech.views.usercontrols;
using WPFLibrary;

namespace NovelTech.viewmodels
{
    public class VM_material : BaseViewModel
    {
        #region Commands
        #region Movement
        public RelayCommand MoveUpCommand { get; private set; }
        public RelayCommand MoveDownCommand { get; private set; }
        public RelayCommand MoveLeftCommand { get; private set; }
        public RelayCommand MoveRightCommand { get; private set; }
        public RelayCommand TurnLeftCommand { get; private set; }
        public RelayCommand TurnRightCommand { get; private set; }
        #endregion
        public RelayCommand AttachMaterialCommand { get; private set; }
        #endregion
        #region Properties
        #region Triggers

        #region Property - isShowMargin
        private bool _isShowMargin = false;

        public bool isShowMargin
        {
            get { return _isShowMargin; }
            set { _isShowMargin = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - isMaterialAttached
        private bool _isMaterialAttached = false;

        public bool isMaterialAttached
        {
            get { return _isMaterialAttached; }
            set { _isMaterialAttached = value; OnPropertyChanged(); }
        }
        #endregion
        #endregion
        #region Sizes
        #region RealSize

        #region Property - materialRealWidth
        private double _materialRealWidth = 200;

        public double materialRealWidth
        {
            get { return _materialRealWidth; }
            set { _materialRealWidth = value; OnPropertyChanged(); OnPropertyChanged("materialWidth"); OnPropertyChanged("materialMargin"); }
        }
        #endregion

        #region Property - materialRealHeight
        private double _materialRealHeight = 200;

        public double materialRealHeight
        {
            get { return _materialRealHeight; }
            set { _materialRealHeight = value; OnPropertyChanged(); OnPropertyChanged("materialHeight"); OnPropertyChanged("materialMargin"); }
        }
        #endregion

        #region Property - materialThick
        private double _materialThick = 5;

        public double materialThick
        {
            get { return _materialThick; }
            set { _materialThick = value; OnPropertyChanged(); }
        }
        #endregion

        #endregion

        public double materialWidth { get { return materialRealWidth * VM_main.instance.dimensionRatio; }}
        public double materialHeight { get { return materialRealHeight * VM_main.instance.dimensionRatio; }}
        public Size bboxSize { get { return VM_shape.instance.shapeSize; } }



        #region Property - marginSize
        private double _marginSize = 10;

        public double marginSize
        {
            get { return _marginSize * VM_main.instance.dimensionRatio; }
            set { _marginSize = value; OnPropertyChanged(); }
        }
        #endregion
        public double elpMarginSize
        {
            get { return (materialRealWidth + materialRealHeight / 2) * VM_main.instance.dimensionRatio + marginSize; }
        }

        #endregion
        #region Margins

        #region Property - bboxMargin
        private Thickness _bboxMargin = new Thickness();

        public Thickness bboxMargin
        {
            get { return _bboxMargin; }
            set { _bboxMargin = value; OnPropertyChanged(); }
        }
        #endregion

        public Thickness materialMargin
        {
            get { return new Thickness(
                VM_machine_table.instance.pincherX + pincherSize / 2 - materialWidth/2, 0, 0,
                VM_machine_table.instance.pincherY + pincherSize / 2 - materialHeight/2); }
        }

        #region Property - pincherSize

        /// <summary>
        /// calculate arms positioning on mouse move to reset arm at the start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static double GetPincherSize()
        {

            string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.Parent.FullName;
            string propertiesPath = System.IO.Path.Combine(projectDirectory, "properties.xml");

            //get the size value from the properties file
            double pincherSize = ReadXMLDoubleValue(propertiesPath, "PincherSize");

            return pincherSize;
        }


        /// <summary>
        /// used to read a value of the type double from xml file 
        /// </summary>
        /// <param name="nodeName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private static double ReadXMLDoubleValue(string file, string nodeName)
        {
            //load xml file with the size of the arms
            XmlDocument doc = new XmlDocument();
            doc.Load(file);

            XmlNode node = doc.DocumentElement.SelectSingleNode(nodeName);
            string text = node.InnerText;

            if (double.TryParse(text, out var value)) return value * VM_main.instance.dimensionRatio;
            else
            {
                throw new Exception("tried to read a non double value, check your XML file");
            }
        }

        private double _pincherSize = GetPincherSize();

        public double pincherSize
        {
            get { return _pincherSize; }
            set { _pincherSize = value; OnPropertyChanged(); }
        }
        #endregion

        public Thickness pincherMargin
        {
            get { return new Thickness(VM_machine_table.instance.pincherX, 0, 0, VM_machine_table.instance.pincherY); }
        }

        #endregion
        #region Material Selection
        public ObservableCollection<Material_family> materialFamilies { get; set; }
        public ObservableCollection<Material> materials { get; set; }

        #region Property - selectedFamily
        private Material_family _selectedFamily;

        public Material_family selectedFamily
        {
            get { return _selectedFamily; }
            set { _selectedFamily = value;
                materials = value.materials;
                if (materials.Count > 0) 
                    selectedMaterial = materials[0];
                OnPropertyChanged(); }
        }
        #endregion

        #region Property - selectedMaterial
        private Material _selectedMaterial;
        public Material selectedMaterial
        {
            get { return _selectedMaterial; }
            set { _selectedMaterial = value; OnPropertyChanged(); }
        }
        #endregion

        #endregion

        #region Property - angle

        public double angle
        {
            get { return VM_machine_table.instance.pincherAngle; }
        }
        #endregion
        #endregion
        #region Fields
        public static VM_material instance;
        #region UIControls For Calculations
        public Ellipse elpMargin;
        public UC_material uiMaterial;
        #endregion
        #endregion
        public VM_material()
        {
            instance = this;
            materialFamilies = data.init_data.get_metarials();
            materials = new ObservableCollection<Material>();
            VM_machine_table.instance.PropertyChanged += (s, e) => 
            {
                if (e.PropertyName == "pincherAngle")
                    OnPropertyChanged("angle");
            };
        }

        void MoveUPCommandfunc()
        {
            VM_machine_table.instance.pincherY++;
            UC_machine_table.instance.ChangeAngles();
        }
        void MoveDownCommandfunc()
        {
            VM_machine_table.instance.pincherY--;
            UC_machine_table.instance.ChangeAngles();
        }
        void MoveLeftCommandfunc()
        {
            VM_machine_table.instance.pincherX--;
            UC_machine_table.instance.ChangeAngles();
        }
        void MoveRightCommandfunc()
        {
            VM_machine_table.instance.pincherX++;
            UC_machine_table.instance.ChangeAngles();
        }

        protected override void AddCommands()
        {
            #region materialMoving
            MoveUpCommand = new RelayCommand(o => MoveUPCommandfunc(), o => 
                VM_machine_table.instance.pincherY < VM_machine_table.instance.machineHeight 
                * VM_main.instance.dimensionRatio - pincherSize);
            MoveDownCommand = new RelayCommand(o => MoveDownCommandfunc(), o =>
               VM_machine_table.instance.pincherY > 0);
            MoveLeftCommand = new RelayCommand(o => MoveLeftCommandfunc(), o =>
                VM_machine_table.instance.pincherX > 0);
            MoveRightCommand = new RelayCommand(o => MoveRightCommandfunc(), o =>
                VM_machine_table.instance.pincherX < VM_machine_table.instance.machineWidth 
                * VM_main.instance.dimensionRatio - pincherSize);


            TurnLeftCommand = new RelayCommand(o => 
            {
                VM_machine_table.instance.pincherAngle += 5;
                if (VM_machine_table.instance.pincherAngle >= 360)
                    VM_machine_table.instance.pincherAngle -= 360;
            });
            TurnRightCommand = new RelayCommand(o =>
            {
                VM_machine_table.instance.pincherAngle -= 5;
                if (VM_machine_table.instance.pincherAngle <= 0)
                    VM_machine_table.instance.pincherAngle += 360;
            });
            #endregion

            AttachMaterialCommand = new RelayCommand(o => isMaterialAttached = !isMaterialAttached);
        }
        protected override void SetPropertyAnnotation()
        {
            VM_machine_table.instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "pincherX" || e.PropertyName == "pincherY")
                {
                    //Console.WriteLine(e.PropertyName);
                    OnPropertyChanged("pincherMargin");
                    OnPropertyChanged("materialMargin");
                }
            };
            VM_shape.instance.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == "shapeSize")
                    OnPropertyChanged("bboxSize");
            };
            PropertyChanged += (s, e) =>
             {/*
                 Ellipse a= new Ellipse();
                 a.Width = 50;
                 a.Height = 50;
                 a.StrokeThickness = 4;
                 
                 //a.Fill.Opacity = 1;8*/
             };
        }
    }
}
