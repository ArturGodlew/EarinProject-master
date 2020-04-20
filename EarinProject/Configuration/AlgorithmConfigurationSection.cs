using EarinProject.RouteFinder.Evaluation;
using EarinProject.RouteFinder.GenomeManagement;
using System.Configuration;

namespace EarinProject.Configuration
{
	public class AlgorithmConfigurationSection : ConfigurationSection, IGenomeManagerConfiguration, IGeneEvaluatorConfiguration
	{
		[ConfigurationProperty(nameof(Generations))]
		public int Generations => (int)this[nameof(Generations)];

		[ConfigurationProperty(nameof(PoolSize))]
		public int PoolSize => (int)this[nameof(PoolSize)];
		
		[ConfigurationProperty(nameof(Breeders))]
		public float Breeders => (float)this[nameof(Breeders)];

		[ConfigurationProperty(nameof(Elitism))]
		public float Elitism => (float)this[nameof(Elitism)];

		[ConfigurationProperty(nameof(Mutation))]
		public float Mutation => (float)this[nameof(Mutation)];

		[ConfigurationProperty(nameof(SandAmount))]
		public int SandAmount => (int)this[nameof(SandAmount)];
		
	}
}
