#nullable enable
using EarinProject.RouteFinder.Evaluation;
using EarinProject.RouteFinder.GenomeManagement;
using Shared;
using System.Collections.Generic;
using System.Linq;

namespace EarinProject.RouteFinder.GenomeManagement
{
	public class GenomeManager
	{
		private readonly IGenomeManagerConfiguration _configuration;
		private readonly GenotypeGenerator _genotypeGenerator;

		public GenomeManager(IGenomeManagerConfiguration configuration, GenotypeGenerator genotypeGenerator)
		{
			_configuration = configuration;
			_genotypeGenerator = genotypeGenerator;
		}

		public IEnumerable<ScoredGenotype> GenerateInitialPopulation()
		{
			return Enumerable.Range(0, _configuration.PoolSize).Select(_ => new ScoredGenotype(_genotypeGenerator.Generate()));
		}

		public IEnumerable<ScoredGenotype> NextGeneration(IEnumerable<ScoredGenotype> population)
		{
			var populationToSave = (int)(population.ToList().Count * _configuration.Breeders * _configuration.Elitism);
			var newRandomGenotypes = (int)(population.ToList().Count * _configuration.Breeders * (1 - _configuration.Elitism));
			var toBreed = population.ToList().Count - populationToSave - newRandomGenotypes;

			var result = population.OrderBy(g => g.Score).ToList().GetRange(0, populationToSave);
			
			result.AddRange(_genotypeGenerator.Generate(newRandomGenotypes).Select(x => new ScoredGenotype(x)));

			result.AddRange(Enumerable.Range(0, toBreed).Select(_ =>
			{
				var firstParent = result.TakeRandom();
				var secondParent = result.Except(new[] { firstParent }).TakeRandom();
				return _genotypeGenerator.Breed(firstParent.Genotype, secondParent.Genotype, _configuration.Mutation);
			}).Select(x => new ScoredGenotype(x)));

			return result;
		}
	}
}
