using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using GraphPartition.Gui.MainApplication.Programming;
using Graphs.Algorithms;
using Optimizations;
using Utils;
using Utils.DataStructures;
using Utils.UiUtils;

namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        private volatile object killTaskRunningLock = new object();
        private volatile object runPauseLock = new object();
        private PagesNavigator<GraphPartitionSolution> SolutionsNavigator { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            MethodChoosingViewer.Content = MethodChoosingViewerCreator.Create(Dispatcher, OptimizationTypeChanged);

        }


        private void RunAlgorithmButton_OnClick(object sender, RoutedEventArgs e)
        {
            var resultDir = Path.Combine(OutputResultPath,
                OptimizationType.AsString() + "_" + (int)DateTime.Now.TimeOfDay.TotalSeconds);
            Directory.CreateDirectory(resultDir);
            if (PlayButton.IsEnabled)
                PlayButton_Click(null, null);
            var solutions = Run(OptimizationType, GraphVisual.Graph);
            this.PrepareResultWindow();
            this.WindowScrollViewer.ScrollToEnd(TimeSpan.FromSeconds(3.0));
            SolutionsNavigator = PagesNavigator<GraphPartitionSolution>.Create(SetSolution, Dispatcher);
            SolutionsNavigator.StatusEvent += (o, args) => Dispatcher.Invoke(() =>
            {
                NextSolutionButton.IsEnabled = args.CanGoRight;
                PrevSolutionButton.IsEnabled = args.CanGoLeft;
            });
            Task.Run(() =>
            {
                Monitor.Enter(killTaskRunningLock);
                Thread.Sleep(100);
                Monitor.Exit(killTaskRunningLock);
                foreach (var solution in solutions)
                {
                    SolutionsNavigator.AddNext(solution);
                    Thread.Sleep(200);
                    Dispatcher.Invoke(() => BestSolutionViewBox.Child.TypeCast<Canvas>().SaveAsPng(Path.Combine(resultDir, solution.NegativePrice + ".png"), 1));
                }
                    

            });

            this.PlayButton.IsEnabled = false;
            this.PauseButton.IsEnabled = true;
            this.NextSolutionButton.IsEnabled = false;
            this.PrevSolutionButton.IsEnabled = false;
            this.NextSolutionButton.IsEnabled = this.PrevSolutionButton.IsEnabled = false;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Monitor.Enter(runPauseLock);
            this.PlayButton.IsEnabled = true;
            this.PauseButton.IsEnabled = false;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Monitor.Exit(runPauseLock);
            this.PlayButton.IsEnabled = false;
            this.PauseButton.IsEnabled = true;
        }

        private void NextSolutionButton_Click(object sender, RoutedEventArgs e)
        {
            if (PauseButton.IsEnabled)
                PauseButton_Click(null, null);
            SolutionsNavigator.MoveRight();
        }

        private void PrevSolutionButton_Click(object sender, RoutedEventArgs e)
        {
            if (PauseButton.IsEnabled)
                PauseButton_Click(null, null);
            SolutionsNavigator.MoveLeft();
        }
    }
}
