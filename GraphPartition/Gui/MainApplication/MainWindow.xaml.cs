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
        private volatile ConcurrentSignal killTaskSignal = new ConcurrentSignal(false);
        private volatile ConcurrentSignal taskKilledSignal = new ConcurrentSignal(false);
        private volatile object runPauseLock = new object();
        private PagesNavigator<GraphPartitionSolution> SolutionsNavigator { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            StaticGraphCanvas.Background = EmbeddedGraphViewer.Background;
            MethodChoosingViewer.Content = MethodChoosingViewerCreator.Create(Dispatcher, OptimizationTypeChanged);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            KillRunning();
            base.OnClosing(e);
        }

        private void KillRunning()
        {
            if (Monitor.IsEntered(runPauseLock))
                Monitor.Exit(runPauseLock);
            if (SolutionsNavigator != null)
            {
                if (!taskKilledSignal.TryProcessSignal())
                {
                    killTaskSignal.Signal();
                    taskKilledSignal.WaitForSignalBlocking(TimeSpan.FromMilliseconds(500.0));
                }
            }

            this.PlayButton.IsEnabled = false;
            this.PauseButton.IsEnabled = false;
            this.NextSolutionButton.IsEnabled = false;
            this.PrevSolutionButton.IsEnabled = false;
        }


        private void RunAlgorithmButton_OnClick(object sender, RoutedEventArgs e)
        {
            KillRunning();
            var resultDir = Path.Combine(OutputResultPath, OptimizationType.AsString() + "_" + (int)DateTime.Now.TimeOfDay.TotalMilliseconds);
            Directory.CreateDirectory(resultDir);
            GraphVisual.Canvas.SaveAsJpg(Path.Combine(resultDir, "graph.jpg"), 1);
            var solutions = Run(OptimizationType, GraphVisual.Graph);
            this.PrepareResultWindow();
            this.WindowScrollViewer.ScrollToEnd(TimeSpan.FromSeconds(3.0));
            SolutionsNavigator = PagesNavigator<GraphPartitionSolution>.Create(SetSolution, Dispatcher);
            SolutionsNavigator.StatusEvent += (o, args) => Dispatcher.InvokeAsync(() =>
            {
                NextSolutionButton.IsEnabled = args.CanGoRight;
                PrevSolutionButton.IsEnabled = args.CanGoLeft;
            });
            Task.Run(() =>
            {
                foreach (var solution in solutions)
                {
                    SolutionsNavigator.AddNext(solution);
                    Thread.Sleep(100);
                    Dispatcher.InvokeAsync(() => BestSolutionViewBox.Child.TypeCast<Canvas>().SaveAsJpg(Path.Combine(resultDir, solution.NegativePrice + ".jpg"), 1));
                    Thread.Sleep(100);
                }
            });
            PauseButton.IsEnabled = true;
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
