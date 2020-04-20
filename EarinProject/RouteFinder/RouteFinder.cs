using EarinProject.Configuration;
using EarinProject.KirovReporting;
using EarinProject.RouteFinder.Evaluation;
using EarinProject.RouteFinder.GenomeManagement;
using Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace EarinProject.RouteFinder
{
    class RouteFinder
    {
		public List<SimpleReport> FindRouteAndReport(Graph graph, int intermidiateRaports)
        {
			var config = (AlgorithmConfigurationSection)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection("algorithm");
			var reports = new List<SimpleReport>();

			var genomManager = new GenomeManager(config, new GenotypeGenerator(graph.Edges));
			var genomEvaluator = new GenomEvaluator(graph, config);

			var pool = genomManager.GenerateInitialPopulation();
			var generationToGo = config.Generations;
			while (generationToGo-- > 0)
			{
				pool = genomEvaluator.Evaluate(pool);
				pool = genomManager.NextGeneration(pool);

				if(generationToGo % (config.Generations/(intermidiateRaports +1)) == 0 || generationToGo == 0)
				{
					reports.Add(ReportGenerator.GenerateSimpleReport(pool));
				}
			}

			return reports;
		}
    }
}
