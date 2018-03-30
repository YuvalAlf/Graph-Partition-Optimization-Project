using System;
using Microsoft.Win32;

namespace GraphPartition.Gui.Programming
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
        }
    }
}
