using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
	public class Route
	{
		public Vertex Start { get; set; }
		public Vertex Goal { get; set; }
		public List<Vertex> Path { get; set; }
		public List<Edge> EdgePath { get; set; }
		public int Score { get; set; }
		public int Length { get; set; }

		public Route() { }

		public Route(Vertex start, Vertex goal, int length = 0)
		{
			Start = start;
			Goal = goal;
			Path = new List<Vertex>() { goal };
			EdgePath = new List<Edge>();
			Length = length;
		}

		public void PrintToConsole()
		{			
			Console.WriteLine($"target reachable in {Path.Count - 1} moves");
			foreach(var vertex in Path)
			{
				Console.Write($"{vertex.Id}, ");
			}
			Console.WriteLine();
			Console.WriteLine($"by paths");
			foreach (var edge in EdgePath)
			{
				Console.Write($"{edge.Id}, ");
			}
			Console.WriteLine();
			Console.WriteLine();
		}

		public void moveOne()
		{
			Path.RemoveAt(0);
			Length -= EdgePath[0].Length;
			EdgePath.RemoveAt(0);
			Start = Path[0];
		}

		public Route GetReverseRoute()
		{
			var result = new Route()
			{
				Start = Goal,
				Goal = Start,
				Length = Length,
				Score = Score,
				Path = Path,
				EdgePath = EdgePath
			};
			result.Path.Reverse();
			result.EdgePath.Reverse();
			return result;
		}

	}
}
