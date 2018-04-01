using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GraphPartition.Gui.GraphCreator.GraphCreatorState;
using GraphPartition.Gui.ProgrammedGui;
using Graphs.EmbeddingInPlane;
using Graphs.GraphProperties;
using Graphs.Visualizing;
using Microsoft.Win32;
using CreatorState = GraphPartition.Gui.GraphCreator.GraphCreatorState.CreatorState;

namespace GraphPartition.Gui.GraphCreator
{
    public partial class GraphCreatorWindow : Window
    {
        private Action<string> GraphPathChosen { get; }
        private StateController StateController { get; set; }
        private EdgesHandler EdgeHandler { get; }
        private GraphVisual GraphVisual { get; }

        public GraphCreatorWindow(Action<string> graphPathChosen, Brush nodeBrush, Brush numBrush, Brush lineBrush)
        {
            InitializeComponent();
            GraphPathChosen = graphPathChosen;
            this.EdgeHandler = EdgesHandler.Create(EdgesScrollViewer, GraphVisual.UpdateWeight);
            this.StateController = new IdleState(CreatorState.Create(this.GraphCanvas, EdgeHandler, GraphUpdated));
            GraphVisual = GraphVisual.Create(GraphCanvas, nodeBrush, numBrush, lineBrush);
        }

        public void GraphUpdated(int numOfNodes, int numOfEdges)
        {
            if (numOfEdges > 0 && numOfNodes > 0)
                EmbeddGraphButton.IsEnabled = true;
            else
                EmbeddGraphButton.IsEnabled = false;

            if (numOfNodes > 0 && numOfNodes % 4 == 0)
                SaveGraphButton.IsEnabled = true;
            else
                SaveGraphButton.IsEnabled = false;
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

        private void EmbeddGraphButton_OnClick(object sender, RoutedEventArgs e)
        {
            void FlipEnabaling()
            {
                GraphCanvas.IsEnabled ^= true;
                EdgesScrollViewer.IsEnabled ^= true;
                this.UpdateLayout();
            }

            FlipEnabaling();
            this.StateController.CreatorState.EmbeddFor(TimeSpan.FromSeconds(0.5), FlipEnabaling, Dispatcher);
        }

        private void SaveGraphButton_OnClick(object sender, RoutedEventArgs e)
        {
            var embedding = this.StateController.CreatorState.DerivedEmbedding();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
                File.WriteAllLines(saveFileDialog.FileName, embedding.ToText());
        }

        private void OpenGraphButton_OnClick(object __, RoutedEventArgs _)
        {
            try
            {
                void fileChosen(string path)
                {
                    var embedding = GraphEmbedding.FromText(File.ReadLines(path));
                    this.StateController.CreatorState.SetNewEmbedding(embedding);
                    this.GraphPathChosen(path);
                    this.Close();
                }
                DialogCreator.ChooseFile(fileChosen);
            }
            catch(Exception e)
            {
                MessageBox.Show("An error occured. file is malformed:" + Environment.NewLine + e.Message);
            }
        }
    }
}
