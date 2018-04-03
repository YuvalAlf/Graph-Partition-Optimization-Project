using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GraphPartition.Gui.ProgrammedGui;
using Utils.MathUtils;
using Utils.UiUtils.DrawingUtils;

namespace GraphPartition.Gui.GraphCreator
{
    public sealed class WeightsHandler
    {
        public const int Steps = 4;
        private Dictionary<int, TextBlock> StepTextBoxes { get; }

        public WeightsHandler(Dictionary<int, TextBlock> stepTextBoxes)
        {
            this.StepTextBoxes = stepTextBoxes;
        }

        public static WeightsHandler Create(StackPanel stackPanel, Brush nodeBrush, Brush lineBrush, double nodeWidth, double minThickness, double maxThickness, double minWeight, double maxWeight)
        {
            var thicknessLine = MathLine.Create(minWeight, maxWeight, minThickness, maxThickness);
            var stepTextBoxes = new Dictionary<int, TextBlock>();
            stackPanel.Children.Add(TextBlockCreator.CreateTitle("Proportions"));
            for (int step = 0; step <= Steps; step++)
            {
                var weight = WeightOfStep(step, minWeight, maxWeight);
                var thickness = thicknessLine.Compute(weight);
                var textBlock = TextBlockCreator.CreateNormal(AsString(weight));
                stepTextBoxes[step] = textBlock;
                var canvas = CreateCanvas(nodeBrush, lineBrush, nodeWidth, thickness);
                stackPanel.Children.Add(HorizontalContainerStrecherd.Create()
                    .AddLeft(textBlock.WithBullet())
                    .AsDock(canvas)
                    .WithMargin(5));
            }
            return new WeightsHandler(stepTextBoxes);
        }

        public void ChangeWeights(double newMinWeight, double newMaxWeight)
        {
            for (int step = 0; step <= Steps; step++)
                StepTextBoxes[step].Text = AsString(WeightOfStep(step, newMinWeight, newMaxWeight));
        }

        private static string AsString(double weight)
        {
            return weight.ToString("e2") + ": ";
        }

        private static double WeightOfStep(int step, double minWeight, double maxWeight)
        {
            return maxWeight * ((double) step / Steps) + minWeight * (Steps - step) / Steps;
        }

        private static UIElement CreateCanvas(Brush nodeBrush, Brush lineBrush, double nodeWidth, double thickness)
        {
            Canvas canvas = new Canvas();
            canvas.Width = 4 * nodeWidth;
            canvas.Height = nodeWidth;
            var ellipse1 = EllipseUtils.CreateEllipse(nodeWidth, nodeWidth, 500, nodeBrush, new Point(0, 0));
            var ellipse2 = EllipseUtils.CreateEllipse(nodeWidth, nodeWidth, 500, nodeBrush, new Point(canvas.Width - nodeWidth, 0));
            canvas.Children.Add(LineUtils.CreateLine(ellipse1.GetCanvasCenter(), ellipse2.GetCanvasCenter(), lineBrush, thickness, PenLineCap.Flat));
            canvas.Children.Add(ellipse1);
            canvas.Children.Add(ellipse2);
            return canvas.SurroundViewBox(Stretch.UniformToFill);
        }
    }
}
