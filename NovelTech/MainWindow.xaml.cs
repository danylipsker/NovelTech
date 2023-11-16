using NovelTech.models.materials;
using NovelTech.viewmodels;
using NovelTech.viewmodels.tools;
using NovelTech.views.usercontrols;
using System;
using System.Windows;

namespace NovelTech
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            DataContext = new viewmodels.VM_main();
            InitializeComponent();

        }

        #region MyRegion
        //change available materials when changing family
        private void ComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (availableMaterials == null)
                return;
            availableMaterials.Items.Clear();
            foreach (models.materials.Material m in VM_material.instance.materials)
                availableMaterials.Items.Add(m);
            availableMaterials.SelectedItem = VM_material.instance.materials[0];
        }
        //for showing starting values
        private void availableMaterials_Initialize(object sender,EventArgs e)
        {
            foreach (models.materials.Material m in VM_material.instance.materials)
                availableMaterials.Items.Add(m);
            availableMaterials.SelectedItem = VM_material.instance.materials[0];
        }
        #endregion
        //for updating feedrate and RPM
        private void availableMaterials_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            Material selectedMaterial= availableMaterials.SelectedItem as Material;
            if (selectedMaterial == null)
                return;

            if (viewmodels.VM_tools.toolBox.equipped.Count <= 1)
            {
                viewmodels.VM_tools.toolBox.equipped.Add(new models.tools.Tool_in_action(new models.tools.Tool()));
                viewmodels.VM_tools.toolBox.equipped.Add(new models.tools.Tool_in_action(new models.tools.Tool()));
            }

            //viewmodels.VM_tools.toolBox.equipped.Add(EditToolEquippedViewModel.);
            viewmodels.VM_tools.toolBox.equipped[0].tool.rpm = selectedMaterial.RPM;
            viewmodels.VM_tools.toolBox.equipped[0].tool.feed_rate = selectedMaterial.feedrate;
            if (UC_machine_table.instance == null)
                return;
            UC_machine_table.instance.UpdateRPMFeedrateValues();
        }

        /// <summary>
        /// the function that happens when the user wants to enlarge the screen
        /// note that if you change the resolution of the app you need to modify the function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnlargeButton_Click(object sender, RoutedEventArgs e)
        {
            //if normal than enlarge
            //width 100 is normal
            if (this.Width == 1000)
            {
                this.Width += 600;
                UC_machine_table.instance.Width += 600;
                UC_machine_table.instance.Workspace.Width += 600;
                UC_machine_table.instance.TableWidth.Width = new GridLength(1200);
                VM_machine_table.instance.machineWidth += 600;
            }
            else
            {
                this.Width -= 600;
                UC_machine_table.instance.Width -= 600;
                UC_machine_table.instance.Workspace.Width -= 600;
                VM_machine_table.instance.machineWidth -= 600;
                UC_machine_table.instance.TableWidth.Width = new GridLength(600);

                //if pincher is outside of the bounds it will return to the small table
                //posiable to not allow table to become small if pincher is outside
                if (VM_machine_table.instance.pincherX > VM_machine_table.instance.machineWidth
                * VM_main.instance.dimensionRatio - VM_material.instance.pincherSize) VM_machine_table.instance.pincherX = 0;
            }
        }
    }
}
