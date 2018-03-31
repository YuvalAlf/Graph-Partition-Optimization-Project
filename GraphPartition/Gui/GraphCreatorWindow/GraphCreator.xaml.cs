using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GraphPartition.Gui.GraphCreatorWindow.GraphCreatorState;
using Graphs;

namespace GraphPartition.Gui.GraphCreatorWindow
{
    public partial class GraphCreator : Window
    {
        private StateController StateController { get; set; }
        private EdgesHandler EdgeHandler { get; }

        public GraphCreator(Action<string> graphPathChosen)
        {
            InitializeComponent();
            this.EdgeHandler = EdgesHandler.Create(EdgesScrollViewer, UpdateWeight);
            this.StateController = new IdleState(CreatorState.Create(this.GraphCanvas, EdgeHandler));
        }

        private void UpdateWeight(Edge edge)
        {
            StateController.CreatorState.UpdateWeight(edge);
        }


        private static void SaveFileDialog(string fileName)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = fileName; // Default file name
            dlg.DefaultExt = ".graph"; // Default file extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result.HasValue && result == true)
            {
                // Save document
                string filename = dlg.FileName;

            }
        }

        private void GraphCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(GraphCanvas);
            StateController = StateController.LeftMouseDownAt(pos, e.ClickCount);
        }

        private void GraphCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(GraphCanvas);
            StateController = StateController.LeftMouseUpAt(pos);

        }

        private void GraphCanvas_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(GraphCanvas);
            StateController = StateController.RightMouseDownAt(pos, e.ClickCount);
        }

        private void GraphCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            var pos = e.GetPosition(GraphCanvas);
            StateController = StateController.MouseMoveTo(pos);
        }
    }
}
