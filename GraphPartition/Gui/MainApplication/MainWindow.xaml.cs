using System.Windows;
using GraphPartition.Gui.MainApplication.Programming;

namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MethodChoosingViewer.Content = MethodChoosingViewerCreator.Create(Dispatcher, OptimizationTypeChanged);
        }
    }
}
