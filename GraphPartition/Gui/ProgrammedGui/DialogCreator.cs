﻿using System;
using System.Windows.Forms;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace GraphPartition.Gui.ProgrammedGui
{
    public static class DialogCreator
    {
        public static void ChooseFile(Action<string> onFileChosen)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                onFileChosen(openFileDialog.FileName);
        }
        public static void ChooseFolder(Action<string> onFolderChosen)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                    if (dialog.SelectedPath != null)
                        onFolderChosen(dialog.SelectedPath);
            }
        }
    }
}