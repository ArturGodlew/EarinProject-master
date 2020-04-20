
namespace EarinProject.RouteFinder.GenomeManagement
{
	public interface IGenomeManagerConfiguration
	{
		int PoolSize { get; }
		float Breeders { get; }
		float Elitism { get; }
		float Mutation  { get; }
	}
}