#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared
{
	public class Graph
	{
		public List<Vertex> Vertices { get; }
		public List<Edge> Edges { get; }

		public Graph() { }

		public Graph(List<Vertex> vertices)
		{
			Vertices = vertices;
			Edges = new List<Edge>();
		}

		public Graph(int numOfVertices, int minCost, int maxCost, int everageBranching, int maxAttemptCount = 100)
		{
			var rand = new Random();
			bool done = false;
			var remainingAttempts = maxAttemptCount;
			var connectionPropabilty = (everageBranching * 100) / numOfVertices;
			do
			{
				Vertices = Enumerable.Range(0, numOfVertices).Select(x => new Vertex(x)).ToList();
				Edges = new List<Edge>();
				var processedVertices = new List<Vertex>();

				Vertices.ForEach(vertex1 =>
				{
					processedVertices.Add(vertex1);
					Vertices.Except(processedVertices).Where(_ => rand.Next(0, 100) < connectionPropabilty).ToList().ForEach(vertex2 => Connect(vertex1, vertex2, rand.Next(minCost, maxCost + 1)));
				});
				var validation = Validate();
				if (Vertices.All(x => x.Edges.Any()) && validation)
				{
					done = true;
				}
				else
				{
					if (validation)
						Console.WriteLine($"Failed to generate graph ({Vertices.Count(x => !x.Edges.Any())} vertices without edge). Trying again");
					else
						Console.WriteLine("Graph failed validation. At least one unreachable vertex");
					Console.WriteLine($"Attempts remaining:{remainingAttempts}");
				}
			} while (--remainingAttempts > 0 && !done);
		}

	private bool Validate()
	{
		var result = new List<Vertex>();
		Validate(Vertices.First(), result);
		return result.Count == Vertices.Count;
	}

	private static void Validate(Vertex current, List<Vertex> vistiedVertices)
	{
		if (!vistiedVertices.Contains(current))
			vistiedVertices.Add(current);
		foreach (var vertex in current.Edges.Select(x => x.GetOtherVertex(current)).Except(vistiedVertices))
			Validate(vertex, vistiedVertices);
	}

	public void Connect(int Id, int vertexId1, int vertexId2, int length)
		{
			var vertex1 = Vertices.Find(v => v.Id == vertexId1);
			var vertex2 = Vertices.Find(v => v.Id == vertexId2);
			var newEdge = new Edge(Id, vertex1, vertex2, length);

			Edges.Add(newEdge);
			vertex1.Edges.Add(newEdge);
			vertex2.Edges.Add(newEdge);
		}

		public void Connect(Vertex vertex1, Vertex vertex2, int length)
		{
			Connect(Edges.Count, vertex1.Id, vertex2.Id, length);
		}

		private Vertex GetOtherRandomVertex(Vertex vertex)
		{
			var vertexId = new Random().Next(0, Vertices.Count - 1);

			if(vertexId == vertex.Id)
			{
				vertexId = Vertices.Count - 1;
			}

			return Vertices[vertexId];
		}

		public void PrintToConsole(string graphName = null)
		{
			Console.WriteLine($"Graph {graphName}");
			foreach (var node in Vertices)
			{
				Console.WriteLine($"Vertex {node.Id}:");
				int vertexId;
				foreach(var edge in node.Edges)
				{
					vertexId = edge.GetOtherVertex(node).Id;
					Console.WriteLine(vertexId + new string(' ', 1 + Vertices.Count.ToString().Length - vertexId.ToString().Length) + "by edge " + edge.Id  + new string(' ', 1 + Edges.Count.ToString().Length - edge.Id.ToString().Length) + "length " + edge.Length);
				}
				Console.WriteLine();
			}
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
			Console.WriteLine();
		}
	}
}
