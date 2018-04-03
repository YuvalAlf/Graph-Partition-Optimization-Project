using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GraphPartition.Gui.GraphCreator.GraphCreatorState;
using GraphPartition.Gui.ProgrammedGui;
using Graphs.EmbeddingInPlane;
using Graphs.Visualizing;
using Microsoft.Win32;
using Utils.ExtensionMethods;

namespace GraphPartition.Gui.GraphCreator
{
    public partial class GraphCreatorWindow : Window
    {
        private Action<string> GraphPathChosen { get; }
        private StateController StateController { get; set; }
        private EdgesHandler EdgeHandler { get; }
        private GraphVisual GraphVisual { get; }
        private WeightsHandler WeightsHandler { get; }

        public GraphCreatorWindow(Action<string> graphPathChosen, Brush nodeBrush, Brush numBrush, Brush lineBrush, PenLineCap penLineCap)
        {
            InitializeComponent();
            GraphPathChosen = graphPathChosen;
            this.GraphVisual = GraphVisual.Create(GraphCanvas, nodeBrush, numBrush, lineBrush, penLineCap);
            this.EdgeHandler = EdgesHandler.Create(EdgesScrollViewer, GraphVisual.UpdateWeight, _ => GraphUpdated());
            GraphVisual.EdgeAddedEvent += (sender, args) => EdgeHandler.AddEdge(args.AddedEdge);
            GraphVisual.EdgeRemovedEvent += (sender, args) => EdgeHandler.RemoveEdge(args.RemovedEdge);
            GraphVisual.EdgeAddedEvent += (sender, args) => this.GraphUpdated();
            GraphVisual.EdgeRemovedEvent += (sender, args) => this.GraphUpdated();
            GraphVisual.NodeAmountChangedEvent += (sender, args) => this.GraphUpdated();
            this.StateController = new IdleState(GraphVisual);
            this.WeightsHandler = WeightsHandler.Create(this.ThicknessDictionaryStackPanel,
                nodeBrush, lineBrush, GraphVisual.NodeWidth, GraphVisual.MinLineThickness, GraphVisual.MaxLineThickness, 0.0, 2.0);
        }

        public void GraphUpdated()
        {
            var numOfNodes = GraphVisual.Nodes.Count;
            var numOfEdges = GraphVisual.Edges.Count;
            if (numOfEdges > 0 &&  numOfNodes > 0)
                EmbeddGraphButton.IsEnabled = true;
            else
                EmbeddGraphButton.IsEnabled = false;

            if (numOfNodes > 0 && numOfNodes % 4 == 0)
                SaveGraphButton.IsEnabled = true;
            else
                SaveGraphButton.IsEnabled = false;

            WeightsHandler.ChangeWeights(GraphVisual.MinWeight, GraphVisual.MaxWeight);
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
            this.GraphVisual.EmbeddFor(TimeSpan.FromSeconds(0.5), FlipEnabaling, Dispatcher);
        }

        private void SaveGraphButton_OnClick(object sender, RoutedEventArgs e)
        {
            var embedding = this.GraphVisual.DerivedEmbedding();
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllLines(saveFileDialog.FileName, embedding.ToText());
                GraphPathChosen.Invoke(saveFileDialog.FileName);
                this.Close();
            }
                
        }

        private void OpenGraphButton_OnClick(object __, RoutedEventArgs _)
        {
            try
            {
                void fileChosen(string path)
                {
                    var embedding = GraphEmbedding.FromText(File.ReadLines(path));
                    this.GraphVisual.SetNewEmbedding(embedding);
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
