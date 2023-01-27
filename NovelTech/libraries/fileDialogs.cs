using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NovelTech.libraries
{
    public static class fileDialogs
    {
        public static string create_openFileDialog
            (string title = "Open File", string filter = "All files (*.*)|*.*", bool multi = false)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = title;
            dialog.Filter = filter;
            dialog.FilterIndex = 0;
            dialog.CheckFileExists = true;
            dialog.CheckPathExists = true;
            dialog.Multiselect = multi;
            dialog.ShowDialog();
            return dialog.FileName;

        }

        public static bool create_saveFileDialog(string content, string title = "Save File",
            string filter = "All files (*.*)|*.*")
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = title;
            dialog.Filter = filter;
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllText(dialog.FileName, content);
                return true;
            }
            return false;
        }

        public static bool create_saveFileDialog(string[] content, string title = "Save File",
            string filter = "All files (*.*)|*.*")
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Title = title;
            dialog.Filter = filter;
            if (dialog.ShowDialog() == true)
            {
                File.WriteAllLines(dialog.FileName, content);
                return true;
            }
            return false;
        }
    }
}
