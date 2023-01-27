﻿using NovelTech.models.tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NovelTech.views.usercontrols.tools
{
    /// <summary>
    /// Interaction logic for UC_edit_blades.xaml
    /// </summary>
    public partial class UC_edit_blades : BaseEditItemUserControl<Blade_type>
    {
        public UC_edit_blades(IList parent, Blade_type item, Mode mode) : base(parent, item, mode)
        {
            InitializeComponent();
        }

        public override void LoadCreateUI()
        {
            l_tools_count.Content = $"Sub Families Count : {item.tools.Count.ToString()}";
        }

        public override void LoadEditUI()
        {
            b_left.Content = "Update";
            b_right.Content = "Delete";
            b_revert.Visibility = Visibility.Visible;
            l_tools_count.Content = $"Sub Families Count : {item.tools.Count.ToString()}";
        }


        public override void LoadItemData()
        {
            tb_name.Text = item.name;
        }

        public override void UpdateItemData()
        {
            item.name = tb_name.Text;
        }

        private void b_left_Click(object sender, RoutedEventArgs e)
        {
            LeftButtonClicked();
        }

        private void b_right_Click(object sender, RoutedEventArgs e)
        {
            Delete();
        }

        private void b_revert_Click(object sender, RoutedEventArgs e)
        {
            Revert();
        }

        public override bool Leaving()
        {
            throw new NotImplementedException();
        }
    }
}
