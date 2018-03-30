using System;
using System.Windows;
using System.Windows.Controls;
using GraphPartition.Gui.Programming;
using Microsoft.Win32;
using Optimizations;
using Optimizations.BranchAndBound;
using Optimizations.GeneticAlgorithm;
using Optimizations.LocalSearch;

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
