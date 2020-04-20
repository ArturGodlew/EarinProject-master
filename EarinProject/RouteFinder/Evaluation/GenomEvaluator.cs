using EarinProject.RouteFinder.GenomeManagement;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EarinProject.RouteFinder.Evaluation
{
	public class GenomEvaluator
	{

		private readonly Graph _graph;
		private readonly IGeneEvaluatorConfiguration _configuration;
		private Dictionary<(Vertex Start, Vertex Goal), Route> RouteToVertex { get; set; }
		private Dictionary<(Vertex Start, Edge Goal), Route> RouteToEdge { get; set; }


		public GenomEvaluator(Graph graph, IGeneEvaluatorConfiguration configuration)
		{
			_graph = graph;
			_configuration = configuration;
			var dijkstra = new Dijkstra(graph);
			RouteToVertex = new Dictionary<(Vertex Start, Vertex Goal), Route>();
			RouteToEdge = new Dictionary<(Vertex Start, Edge Goal), Route>();
			_graph.Vertices.ForEach(v1 =>
			  _graph.Vertices.GetRange(_graph.Vertices.IndexOf(v1) + 1, _graph.Vertices.Count - 1 - _graph.Vertices.IndexOf(v1)).ForEach(v2 =>
			  {
				  var route = dijkstra.FindPath(v1, v2);
				  RouteToVertex.Add((v1, v2), route);
				  RouteToVertex.Add((v2, v1), route.GetReverseRoute());
			  }));

			_graph.Vertices.ForEach(v =>
				_graph.Edges.ForEach(e =>
				{
					Route bestRoute = null;
					if (v != e.Vertex1)
					{
						bestRoute = RouteToVertex[(v, e.Vertex1)];
					}

					if (v != e.Vertex2)
					{
						if (bestRoute != null)
							bestRoute = RouteToVertex[(v, e.Vertex2)];
						else
							bestRoute = (RouteToVertex[(v, e.Vertex2)].Length > bestRoute.Length) ? bestRoute : RouteToVertex[(v, e.Vertex2)];
					}
					RouteToEdge.Add((v, e), bestRoute);
				}));
		}

		public IEnumerable<ScoredGenotype> Evaluate(IEnumerable<ScoredGenotype> genome)
		{
			return genome.ToList().Select(g => { g.Score ??= Evaluate(g.Genotype); return g; });
		}

		private int Evaluate(Genotype genotype)
		{
			var traversalMap = _graph.Edges.ToDictionary(x => x, x => false);
			var orgin = _graph.Vertices.First(x => x.Id == 0);
			var current = orgin;
			var score = 0;
			var sand = _configuration.SandAmount;
			genotype.Edges.ToList().ForEach(x => Move(current, x, sand, score, traversalMap));

			return score;
		}

		private void Move(Vertex start, Edge goal, int sand, int score, Dictionary<Edge, bool> traversalMap)
		{
			if (goal.Length > sand)
			{
				GoToBase(start, sand, score, traversalMap);
			}

			Move(start, goal, sand, score, traversalMap, RouteToEdge[(start, goal)], goal.Length);
		}

		private void Move(Vertex start, Edge geneGoal, int sand, int score, Dictionary<Edge, bool> traversalMap, Route route, int sandNeeded, bool alreadyReturning = false)
		{
			if (!route.EdgePath.Any())
			{
				return;
			}
			var edgeToTraverse = route.EdgePath[0];

			if (getFirstUntraversed(route, traversalMap).Length > sand && edgeToTraverse.Length > sand)
			{
				GoToBase(start, geneGoal, sand, score, traversalMap);
			}
			else
			{

			}
		}

		private void Move(Vertex start, Edge geneGoal, int sand, int score, Dictionary<Edge, bool> traversalMap, Route route, int sandNeeded, bool alreadyReturning = false)
		{
			if (!traversalMap[edgeToTraverse])
				{
					traversalMap[edgeToTraverse] = true;
					sand -= edgeToTraverse.Length;
					route.moveOne();
					Move(edgeToTraverse.GetOtherVertex(start), geneGoal, sand, score, traversalMap, route, sandNeeded);
	}
				else
				{
					score += edgeToTraverse.Length;
					route.moveOne();
					Move(edgeToTraverse.GetOtherVertex(start), geneGoal, sand, score, traversalMap, route, sandNeeded);
}
		}

		private void GoToBase(Vertex start, Edge geneGoal, int sand, int score, Dictionary<Edge, bool> traversalMap)
		{
			Move(edgeToTraverse.GetOtherVertex(start), geneGoal, sand, score, traversalMap, route, sandNeeded);
		}

		private Edge getFirstUntraversed(Route route, Dictionary<Edge, bool> traversalMap)
		{
			return route.EdgePath.First(e => traversalMap[e] == false);
		}
	}
}
