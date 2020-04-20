using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProblemGenerator
{
	class Program
	{
		public static void Main(int numOfVertices = 120, int minCost = 1, int maxCost = 10, int everageBranching = 3, FileInfo file = null)
		{
			file ??= new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "graph.txt"));
			var graph = new Graph(numOfVertices, minCost, maxCost, everageBranching);
			graph.PrintToConsole("before serialization");

			GraphSerializer.Serialize(graph, file);

			var graph2 = GraphSerializer.Deserialize(file);

			graph2.PrintToConsole("after serialization");

		}

	}
}
