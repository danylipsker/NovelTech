using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NovelTech.libraries.ui.popups
{
    /// <summary>
    /// Interaction logic for Popup_text_document.xaml
    /// </summary>
    public partial class Popup_text_document : Window
    {
        private string[] lines;
        private string ext;
        public Popup_text_document(string[] lines, string ext = "Text file (*.txt)|*.txt")
        {
            InitializeComponent();
            this.lines = lines;
            this.ext = ext;
            foreach (string line in lines)
            {
                tb_main.Text += line + Environment.NewLine;
            }
        }

        private void b_save_Click(object sender, RoutedEventArgs e)
        {
            lines = tb_main.Text.Split(Environment.NewLine);
            if (fileDialogs.create_saveFileDialog(lines, filter: ext))
            {
                this.Close();
            }
        }
    }
}
