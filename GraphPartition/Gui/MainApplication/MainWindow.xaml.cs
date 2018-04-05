using System.ComponentModel;
using System.Threading;
using System.Windows;
using GraphPartition.Gui.MainApplication.Programming;

namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        private volatile object runningMonitor = new object();
        private bool isClosing = false;
        public MainWindow()
        {
            InitializeComponent();
            MethodChoosingViewer.Content = MethodChoosingViewerCreator.Create(Dispatcher, OptimizationTypeChanged);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.isClosing = true;
            base.OnClosing(e);
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Monitor.Enter(runningMonitor);
            this.PlayButton.IsEnabled = true;
            this.PauseButton.IsEnabled = false;
            this.NextSolutionButton.IsEnabled = false;
            this.PrevSolutionButton.IsEnabled = false;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Monitor.Exit(runningMonitor);
            this.PlayButton.IsEnabled = false;
            this.PauseButton.IsEnabled = true;
            this.NextSolutionButton.IsEnabled = false;
            this.PrevSolutionButton.IsEnabled = false;
        }

        private void NextSolutionButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrevSolutionButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
