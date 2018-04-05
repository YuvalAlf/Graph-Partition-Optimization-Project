using System;
using System.Windows;
using System.Windows.Controls;
using Optimizations.GeneticAlgorithm;
using Utils.ExtensionMethods;
using Utils.UiUtils;
using Utils.UiUtils.CustomUi.Creator;
using Utils.UiUtils.CustomUi.CustomType;

// ReSharper disable once CheckNamespace
namespace GraphPartition.Gui.MainApplication
{
    public partial class MainWindow : Window
    {
        private GeneticSettings GeneticSettings { get; set; } = GeneticSettings.Default;


        private StackPanel InitGeneticSettings()
        {
            // Population
            var populationValidity = StringExtensions.IsInt().And(StringExtensions.InRange(2, 10000));
            var updatePopulation = new Action<string>(str => GeneticSettings = GeneticSettings.WithPopulation(int.Parse(str)));
            var populationContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.RegularTextBlock("Population").WithBullet())
                .AsDock(InteractiveTextBox.Create(GeneticSettings.Population.ToString(), populationValidity, updatePopulation, Dispatcher));

            // Mutation Rate
            var mutationRateValidity = StringExtensions.IsDouble().And(StringExtensions.InRange(0, 1));
            var updateMutationRate = new Action<string>(str => GeneticSettings = GeneticSettings.WithMutationRate(double.Parse(str)));
            var mutationRateContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.RegularTextBlock("Mutation Rate").WithBullet())
                .AsDock(InteractiveTextBox.Create(this.GeneticSettings.MutationRate.ToString(), mutationRateValidity, updateMutationRate, Dispatcher));

            // Elitism Rate
            var elitismRateValidity = StringExtensions.IsDouble().And(StringExtensions.InRange(0, 1));
            var updateElitismRate = new Action<string>(str => GeneticSettings = GeneticSettings.WithMutationRate(double.Parse(str)));
            var elitismRateContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.RegularTextBlock("Elitism Rate").WithBullet())
                .AsDock(InteractiveTextBox.Create(this.GeneticSettings.ElitismRate.ToString(), elitismRateValidity, updateElitismRate, Dispatcher));


            // New Genes Rate
            var newGenesRateValidity = StringExtensions.IsDouble().And(StringExtensions.InRange(0, 1));
            var updateNewGenesRate = new Action<string>(str => GeneticSettings = GeneticSettings.WithMutationRate(double.Parse(str)));
            var newGenesRateContainer = HorizontalContainerStrecherd.Create()
                .AddLeft(TextBlockCreator.RegularTextBlock("New Genes Rate").WithBullet())
                .AsDock(InteractiveTextBox.Create(this.GeneticSettings.NewGenesRate.ToString(), newGenesRateValidity, updateNewGenesRate, Dispatcher));

            var title = TextBlockCreator.TitleTextBlock("Genetic Settings");

            return GuiExtensions.CreateStackPanel(title, populationContainer, mutationRateContainer, elitismRateContainer, newGenesRateContainer);
        }
    }
}
