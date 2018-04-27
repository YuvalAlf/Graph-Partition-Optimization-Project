using System;
using Utils;
using Utils.DataStructures;
using Utils.ExtensionMethods;

namespace Optimizations.GeneticAlgorithm
{
    public sealed class Genetic<SolutionInstance, Mating, Mutation, Selection> : OptimizationSolver<SolutionInstance, GeneticSettings<Mating, Mutation, Selection>>
        where SolutionInstance : IGeneticSolver<SolutionInstance, Mating, Mutation, Selection>
    {
        public override void Run(
            Func<Random, SolutionInstance> genRandom,
            GeneticSettings<Mating, Mutation, Selection> settings,
            DistributedInt killTask,
            Action<SolutionInstance> reportSolution,
            Random rnd)

        {
            var population = ArrayExtensions.InitArray(settings.Population, _ => genRandom(rnd));
            var newPopulation = population.Copy();


            double bestNegativePrice = double.MaxValue;
            do
            {
                Array.Sort(population, this);
                double currentNegativePrice = population[0].NegativePrice;
                if (currentNegativePrice < bestNegativePrice)
                {
                    reportSolution(population[0]);
                    bestNegativePrice = currentNegativePrice;
                }

                int index = 0;
                for (int i = 0; i < settings.ElitismAmount && index < newPopulation.Length; i++)
                    newPopulation[index] = population[index++];
                for (int i = 0; i < settings.NewGenesAmount && index < newPopulation.Length; i++)
                    newPopulation[index++] = genRandom(rnd);
                var selectionMethod = population[0].SelectionMethod(settings.SelectionScheme, population);
                while (index < newPopulation.Length)
                {
                    var (mom, dad) = selectionMethod(rnd);
                    var son = mom.Mate(settings.MatingScheme, dad, rnd);
                    if (rnd.NextDouble() <= settings.MutationRate)
                        son = son.Mutate(settings.MutatingScheme, rnd);
                    newPopulation[index++] = son;
                }

                GeneralUtils.SwapValues(ref population, ref newPopulation);
            } while (killTask.Num == 0);

            killTask.MinusOne();
        }
    }
}
