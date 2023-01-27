using MethodsLibrary;
using MVVMLibrary.ViewModels;
using System.Windows.Input;
using WPFLibrary;

namespace NovelTech.viewmodels
{
    public class VM_main : BaseViewModel
    {
        #region Commands
        public RelayCommand KeyDownCommand { get; private set; }
        public RelayCommand AddToolCommand { get; private set; }
        #endregion

        #region Properties
        public VM_machine_table vmMachine { get; set; }
        public VM_material vmMaterial{ get; set; }
        public VM_shape vmShape{ get; set; }


        #region Property - appName
        private string _appName = "NovelTech";

        public string appName
        {
            get { return _appName; }
            set { _appName = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - projectName
        private string _projectName;

        public string projectName
        {
            get { return _projectName; }
            set { _projectName = value; OnPropertyChanged(); }
        }
        #endregion

        #region Property - dimensionRatio
        private double _dimensionRatio = 0.75;

        public double dimensionRatio
        {
            get { return _dimensionRatio; }
            set { _dimensionRatio = value; OnPropertyChanged(); }
        }
        #endregion
        #endregion

        #region Fields
        public static VM_main instance;
        #endregion

        #region Constructor
        public VM_main()
        {
            instance = this;
            if (System.Configuration.ConfigurationManager.AppSettings["FirstRun"] == "True") RunFirstTime();
            initVMs();
        }
        #endregion


        #region Methods
        private void RunFirstTime()
        {
            libraries.directories.create_if_not_exist(appName);
            ResourceMethods.CopyResourceToFile("/resources/jsons/materials.json", $"{appName}/materials.json");
            ResourceMethods.CopyResourceToFile("/resources/jsons/ToolBox.json", $"{appName}/ToolBox.json");
            ConfigsMethods.ChangeNSaveConfig("FirstRun", "False");
        }
        protected override void AddCommands()
        {
            KeyDownCommand = new RelayCommand(o =>
            {
                switch ((o as KeyEventArgs).Key)
                {
                    case Key.Up:
                        vmMaterial.MoveUpCommand.Execute(null);
                        break;
                    case Key.Down:
                        vmMaterial.MoveDownCommand.Execute(null);
                        break;
                    case Key.Left:
                        vmMaterial.MoveLeftCommand.Execute(null);
                        break;
                    case Key.Right:
                        vmMaterial.MoveRightCommand.Execute(null);
                        break;
                }
            });

            
            AddToolCommand = new RelayCommand(o =>
            {
                var w_tools = new views.windows.tools.W_tools();
                w_tools.ShowDialog();
            });
            
        }

        private void initVMs()
        {
            vmMachine = new VM_machine_table();
            vmShape = new VM_shape();
            vmMaterial = new VM_material();
        }
        #endregion
    }
}
