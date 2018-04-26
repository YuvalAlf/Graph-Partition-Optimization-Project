namespace Optimizations.GeneticAlgorithm
{
    public sealed class GeneticSettings<Mating, Mutation>
    {
        public static GeneticSettings<Mating, Mutation> Default => new GeneticSettings<Mating, Mutation>(20, 0.6, 0.3, 0.15, default(Mating), default(Mutation));

        public int Population { get; }
        public double MutationRate { get; }
        public double ElitismRate { get; }
        public double NewGenesRate { get; }
        public int ElitismAmount => (int)(Population * ElitismRate);
        public double NewGenesAmount => (int) (Population * NewGenesRate);
        public Mating MatingScheme { get; }
        public Mutation MutatingScheme { get; }

        public GeneticSettings(int population, double mutationRate, double elitismRate, double newGenesRate, Mating matingScheme, Mutation mutatingScheme)
        {
            Population = population;
            MutationRate = mutationRate;
            ElitismRate = elitismRate;
            NewGenesRate = newGenesRate;
            MatingScheme = matingScheme;
            MutatingScheme = mutatingScheme;
        }
    }
}
