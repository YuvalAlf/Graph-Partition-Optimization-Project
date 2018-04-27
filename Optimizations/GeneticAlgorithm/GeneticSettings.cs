namespace Optimizations.GeneticAlgorithm
{
    public sealed class GeneticSettings<Mating, Mutation, Selection>
    {
        public static GeneticSettings<Mating, Mutation, Selection> Default 
            => new GeneticSettings<Mating, Mutation, Selection>(20, 0.6, 0.3, 0.15, default(Mating), default(Mutation), default(Selection));

        public int Population { get; }
        public double MutationRate { get; }
        public double ElitismRate { get; }
        public double NewGenesRate { get; }
        public Mating MatingScheme { get; }
        public Mutation MutatingScheme { get; }
        public Selection SelectionScheme { get; }

        public int ElitismAmount => (int)(Population * ElitismRate);
        public double NewGenesAmount => (int)(Population * NewGenesRate);

        public GeneticSettings(int population, double mutationRate, double elitismRate, double newGenesRate, Mating matingScheme, Mutation mutatingScheme, Selection selectionScheme)
        {
            Population = population;
            MutationRate = mutationRate;
            ElitismRate = elitismRate;
            NewGenesRate = newGenesRate;
            MatingScheme = matingScheme;
            MutatingScheme = mutatingScheme;
            SelectionScheme = selectionScheme;
        }
    }
}
