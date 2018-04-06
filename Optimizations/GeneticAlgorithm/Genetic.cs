using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Utils.ExtensionMethods;

namespace Optimizations.GeneticAlgorithm
{
    public sealed class Genetic<SolutionInstance> : IComparer<SolutionInstance>
        where SolutionInstance : IGeneticSolution<SolutionInstance>
    {
        public Func<Random, SolutionInstance> SolutionsRandomGenerator { get; }

        public Genetic(Func<Random, SolutionInstance> solutionsRandomGenerator)
        {
            SolutionsRandomGenerator = solutionsRandomGenerator;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Compare(SolutionInstance s1, SolutionInstance s2) => s1.NegativePrice.CompareTo(s2.NegativePrice);

        public IEnumerable<SolutionInstance> Run(GeneticSettings settings,
            object runPauseLock, object killTaskRunningLock, Random rnd)
        {
            var population = new SolutionInstance[settings.Population];
            var newPopulation = population.Copy();

            for (int i = 0; i < population.Length; i++)
                population[i] = SolutionsRandomGenerator(rnd);
            Array.Sort(population, this);

            double lastNegativePrice = double.MaxValue;

            while (Monitor.TryEnter(killTaskRunningLock))
                lock(runPauseLock)
                {
                    double newNegativePrice = population[0].NegativePrice;
                    if (newNegativePrice < lastNegativePrice)
                    {
                        yield return population[0];
                        lastNegativePrice = newNegativePrice;
                    }
                    
                    int index = 0;
                    for (int i = 0; i < settings.ElitismAmount; i++)
                        newPopulation[index] = population[index++];
                    for (int i = 0; i < settings.NewGenesAmount; i++)
                        newPopulation[index++] = SolutionsRandomGenerator(rnd);
                    while (index < newPopulation.Length)
                    {
                        var mom = population.ChooseRandomly(rnd);
                        var dad = population.ChooseRandomly(rnd);
                        newPopulation[index++] = mom.MateMutate(dad, settings.MutationRate, rnd);
                    }

                    var temp = population;
                    population = newPopulation;
                    newPopulation = temp;

                    Array.Sort(population, this);
                    Monitor.Exit(killTaskRunningLock);
                }
        }
    }
}
