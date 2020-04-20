#nullable enable
using EarinProject.Configuration;
using EarinProject.RouteFinder;
using Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using EarinProject.KirovReporting;
using EarinProject.RouteFinder.Evaluation;

namespace EarinProject
{
	class Program
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="generationCount">number of generations after which the simulation should be halted</param>
		/// <param name="sandAmount"></param>
		/// <param name="numOfVertices"></param>
		/// <param name="minCost"></param>
		/// <param name="maxCost"></param>
		/// <param name="everageBranching"></param>
		/// <param name="reportFile">output file containing optimization result</param>
		static void Main( int generationCount = 3000, int sandAmount = 100, int numOfVertices = 5, int minCost = 1, int maxCost = 10, int everageBranching = 4, int repeat = 1, int intermidiateRaports = 5, FileInfo graphPath = null,  FileInfo reportFile = null)
		{
			reportFile ??= new FileInfo("report.txt");
			var config = (AlgorithmConfigurationSection)ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection("algorithm");

			var graph = new Graph(numOfVertices, minCost, maxCost, everageBranching);
			graph.PrintToConsole();

			var dijkstra = new Dijkstra(graph);

			while(true)
			{
				dijkstra.FindPath(graph.Vertices.TakeRandom(), graph.Vertices.TakeRandom()).PrintToConsole();
			}
			
			
			var graphs = Enumerable.Repeat(new Graph(numOfVertices, minCost, maxCost, everageBranching), repeat).ToList();
			var results = new List<List<SimpleReport>>();

			var finder = new RouteFinder.RouteFinder();

			graphs.ForEach(graph => results.Add(finder.FindRouteAndReport(graph, intermidiateRaports)));

			var averageResults = new List<SimpleReport>();
			for(var stage = 0;  stage < results[0].Count; stage++)
			{
				averageResults.Add(ReportGenerator.GenerateSimpleReport(results.Select(r => r[stage]).ToList()));
			}

			var finalRaport = ReportGenerator.GenerateFullReport(averageResults);
			finalRaport.ToFile(reportFile);
		}

		
	}
}
