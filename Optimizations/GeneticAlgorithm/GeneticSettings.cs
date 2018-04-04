namespace Optimizations.GeneticAlgorithm
{
    public sealed class GeneticSettings
    {
        public static GeneticSettings Default => new GeneticSettings(20, 0.6, 0.3, 0.15);

        public int Population { get; }
        public double MutationRate { get; }
        public double ElitismRate { get; }
        public double NewGenesRate { get; }
        public int ElitismAmount => (int)(Population * ElitismRate);
        public double NewGenesAmount => (int) (Population * NewGenesRate);

        public GeneticSettings(int population, double mutationRate, double elitismRate, double newGenesRate)
        {
            Population = population;
            MutationRate = mutationRate;
            ElitismRate = elitismRate;
            NewGenesRate = newGenesRate;
        }

        public GeneticSettings WithPopulation(int newPopulation)
        {
            return new GeneticSettings(newPopulation, MutationRate, ElitismRate, NewGenesRate);
        }
        public GeneticSettings WithMutationRate(double newMutationRate)
        {
            return new GeneticSettings(Population, newMutationRate, ElitismRate, NewGenesRate);
        }
        public GeneticSettings WithElitism(double newElitismRate)
        {
            return new GeneticSettings(Population, MutationRate, newElitismRate, NewGenesRate);
        }
        public GeneticSettings WithNewGenesRate(double newNewGenesRate)
        {
            return new GeneticSettings(Population, MutationRate, ElitismRate, newNewGenesRate);
        }
    }
}
