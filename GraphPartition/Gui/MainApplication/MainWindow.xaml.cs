using System.Windows;
using GraphPartition.Gui.GraphCreatorWindow;
using GraphPartition.Gui.Programming;

namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            MethodChoosingViewer.Content = MethodChoosingViewerCreator.Create(Dispatcher, OptimizationTypeChanged);
            new GraphCreator(s => { }).ShowDialog();
            this.Close();
        }
    }
}
