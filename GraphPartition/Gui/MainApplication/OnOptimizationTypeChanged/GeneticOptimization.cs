using System;
using System.Windows;
using System.Windows.Controls;
using GraphPartition.Gui.ProgrammedGui;
using Optimizations.GeneticAlgorithm;
using Utils.ExtensionMethods;

// ReSharper disable once CheckNamespace
namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        private GeneticSettings GeneticSettings { get; set; } = GeneticSettings.Default;


        private StackPanel InitGeneticSettings()
        {
            var stackPanel = new StackPanel();


            // Population
            var populationValidity = StringExtensions.IsInt().And(StringExtensions.InRange(2, 10000));
            var updatePopulation = new Action<string>(str => GeneticSettings = GeneticSettings.WithPopulation(int.Parse(str)));
            var populationContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("Population").WithBullet())
                .AsDock(InteractiveTextBox.Create(GeneticSettings.Population.ToString(), populationValidity, updatePopulation, Dispatcher));

            // Mutation Rate
            var mutationRateValidity = StringExtensions.IsDouble().And(StringExtensions.InRange(0, 1));
            var updateMutationRate = new Action<string>(str => GeneticSettings = GeneticSettings.WithMutationRate(double.Parse(str)));
            var mutationRateContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("Mutation Rate").WithBullet())
                .AsDock(InteractiveTextBox.Create(this.GeneticSettings.MutationRate.ToString(), mutationRateValidity, updateMutationRate, Dispatcher));

            // Elitism Rate
            var elitismRateValidity = StringExtensions.IsDouble().And(StringExtensions.InRange(0, 1));
            var updateElitismRate = new Action<string>(str => GeneticSettings = GeneticSettings.WithMutationRate(double.Parse(str)));
            var elitismRateContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("Elitism Rate").WithBullet())
                .AsDock(InteractiveTextBox.Create(this.GeneticSettings.ElitismRate.ToString(), elitismRateValidity, updateElitismRate, Dispatcher));


            // New Genes Rate
            var newGenesRateValidity = StringExtensions.IsDouble().And(StringExtensions.InRange(0, 1));
            var updateNewGenesRate = new Action<string>(str => GeneticSettings = GeneticSettings.WithMutationRate(double.Parse(str)));
            var newGenesRateContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.CreateNormal("New Genes Rate").WithBullet())
                .AsDock(InteractiveTextBox.Create(this.GeneticSettings.NewGenesRate.ToString(), newGenesRateValidity, updateNewGenesRate, Dispatcher));


            stackPanel.Children.Add(TextBlockCreator.CreateTitle("Genetic Settings"));
            stackPanel.Children.Add(populationContainer);
            stackPanel.Children.Add(mutationRateContainer);
            stackPanel.Children.Add(elitismRateContainer);
            stackPanel.Children.Add(newGenesRateContainer);
            return stackPanel;
        }
    }
}
