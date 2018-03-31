using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using GraphPartition.Gui.GraphCreatorWindow;
using GraphPartition.Gui.Programming;
using Optimizations;
using Optimizations.BranchAndBound;
using Optimizations.GeneticAlgorithm;
using Optimizations.LocalSearch;
using Utils.ExtensionMethods;

namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow
    {
        private OptimizationType OptimizationType { get; set; }
        private GeneticSettings GeneticSettings { get; set; } = GeneticSettings.Default;
        private BranchAndBoundSettings BranchAndBoundSettings { get; set; } = BranchAndBoundSettings.Default;
        private LocalSearchSettings LocalSearchSettings { get; set; } = LocalSearchSettings.Default;
        private string InputGraphPath { get; set; } = @"C:\Users\Yuval\Desktop\yuval.graph";
        public string OutputResultPath { get; set; } = @"C:\Users\Yuval\Desktop";

        private void OptimizationTypeChanged(OptimizationType optimizationType)
        {
            OptimizationType = optimizationType;
            StackPanel settingsStackPanel;
            switch (optimizationType)
            {
                case OptimizationType.Genetic:
                    settingsStackPanel = InitGeneticSettings();
                    break;
                case OptimizationType.BranchAndBound:
                    settingsStackPanel = InitBranchAndBoundSettings();
                    break;
                case OptimizationType.LocalSearch:
                    settingsStackPanel = InitLocalSearchSettings();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(optimizationType), optimizationType, null);
            }

            settingsStackPanel.Children.Add(InputGraphBullet());
            settingsStackPanel.Children.Add(CreateGraph());
            settingsStackPanel.Children.Add(OutputPathBullet());

            SettingsViewer.Content = settingsStackPanel;
        }

        private UIElement CreateGraph()
        {
            return ButtonCreator.Create("Create Graph", UserCreateGraph);
        }

        private void UserCreateGraph()
        {
            var window = new GraphCreator(path => InputGraphPath = path);
            window.ShowDialog();
        }

        private UIElement OutputPathBullet()
        {
            Predicate<string> validityCheck = path => Directory.Exists(path);
            Action<string> onPathChanged = path => OutputResultPath = path;
            var textBox = new InteractiveTextBox(OutputResultPath, validityCheck, onPathChanged, Dispatcher);
            return HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("Result Output Folder").WithBullet())
                .AddRight(ButtonCreator.Create("...", () => DialogCreator.ChooseFolder(path => textBox.Text = path)))
                .AsDock(textBox);
        }


        private UIElement InputGraphBullet()
        {
            Predicate<string> validityCheck = path => File.Exists(path) && path.EndsWith(".graph");
            Action<string> onPathChanged = path => InputGraphPath = path;
            var textBox = new InteractiveTextBox(InputGraphPath, validityCheck, onPathChanged, Dispatcher);
            return HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("Input Graph").WithBullet())
                .AddRight(ButtonCreator.Create("...", () => DialogCreator.ChooseFile(path => textBox.Text = path)))
                .AsDock(textBox);
        }

        private StackPanel InitLocalSearchSettings()
        {
            var stackPanel = new StackPanel();

            stackPanel.Children.Add(TextBlockCreator.CreateTitle("Local Search Settings"));

            return stackPanel;
        }

        private StackPanel InitBranchAndBoundSettings()
        {
            var stackPanel = new StackPanel();

            stackPanel.Children.Add(TextBlockCreator.CreateTitle("Branch & Bound Settings"));

            return stackPanel;
        }

        private StackPanel InitGeneticSettings()
        {
            var stackPanel = new StackPanel();


            // Population
            var populationValidity = StringExtensions.IsInt().And(StringExtensions.InRange(2, 10000));
            var updatePopulation = new Action<string>(str => GeneticSettings = GeneticSettings.WithPopulation(int.Parse(str)));
            var populationContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("Population").WithBullet())
                .AsDock(new InteractiveTextBox(this.GeneticSettings.Population.ToString(), populationValidity, updatePopulation, Dispatcher));

            // Mutation Rate
            var mutationRateValidity = StringExtensions.IsDouble().And(StringExtensions.InRange(0, 1));
            var updateMutationRate = new Action<string>(str => GeneticSettings = GeneticSettings.WithMutationRate(double.Parse(str)));
            var mutationRateContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("Mutation Rate").WithBullet())
                .AsDock(new InteractiveTextBox(this.GeneticSettings.MutationRate.ToString(), mutationRateValidity, updateMutationRate, Dispatcher));

            // Elitism Rate
            var elitismRateValidity = StringExtensions.IsDouble().And(StringExtensions.InRange(0, 1));
            var updateElitismRate = new Action<string>(str => GeneticSettings = GeneticSettings.WithMutationRate(double.Parse(str)));
            var elitismRateContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("Elitism Rate").WithBullet())
                .AsDock(new InteractiveTextBox(this.GeneticSettings.ElitismRate.ToString(), elitismRateValidity, updateElitismRate, Dispatcher));


            // New Genes Rate
            var newGenesRateValidity = StringExtensions.IsDouble().And(StringExtensions.InRange(0, 1));
            var updateNewGenesRate = new Action<string>(str => GeneticSettings = GeneticSettings.WithMutationRate(double.Parse(str)));
            var newGenesRateContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("New Genes Rate").WithBullet())
                .AsDock(new InteractiveTextBox(this.GeneticSettings.NewGenesRate.ToString(), newGenesRateValidity, updateNewGenesRate, Dispatcher));


            stackPanel.Children.Add(TextBlockCreator.CreateTitle("Genetic Settings"));
            stackPanel.Children.Add(populationContainer);
            stackPanel.Children.Add(mutationRateContainer);
            stackPanel.Children.Add(elitismRateContainer);
            stackPanel.Children.Add(newGenesRateContainer);
            return stackPanel;
        }

    }
}
