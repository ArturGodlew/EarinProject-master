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
		private Dictionary<(Vertex Start, Vertex Goal), Route> _routeToVertex { get; set; }
		private Dictionary<(Vertex Start, Edge Goal), Route> _routeToEdge { get; set; }
		private int _score { get; set; }
		private Dictionary<Edge, bool> _traversalMap { get; set; }
		private int _sand { get; set; }

		public GenomEvaluator(Graph graph, IGeneEvaluatorConfiguration configuration)
		{
			_graph = graph;
			_configuration = configuration;
			_routeToVertex = new Dictionary<(Vertex Start, Vertex Goal), Route>();
			_routeToEdge = new Dictionary<(Vertex Start, Edge Goal), Route>();
			_traversalMap = new Dictionary<Edge, bool>();

			var dijkstra = new Dijkstra(graph);
			_graph.Vertices.ForEach(v1 =>
			  _graph.Vertices.GetRange(_graph.Vertices.IndexOf(v1) + 1, _graph.Vertices.Count - 1 - _graph.Vertices.IndexOf(v1)).ForEach(v2 =>
			  {
				  var route = dijkstra.FindPath(v1, v2);
				  _routeToVertex.Add((v1, v2), route);
				  _routeToVertex.Add((v2, v1), route.GetReverseRoute());
			  }));

			_graph.Vertices.ForEach(v =>
				_graph.Edges.ForEach(e =>
				{
					Route bestRoute = null;
					if (v != e.Vertex1)
					{
						bestRoute = _routeToVertex[(v, e.Vertex1)];
					}

					if (v != e.Vertex2)
					{
						if (bestRoute != null)
							bestRoute = _routeToVertex[(v, e.Vertex2)];
						else
							bestRoute = (_routeToVertex[(v, e.Vertex2)].Length > bestRoute.Length) ? bestRoute : _routeToVertex[(v, e.Vertex2)];
					}
					_routeToEdge.Add((v, e), bestRoute);
				}));
		}

		public IEnumerable<ScoredGenotype> Evaluate(IEnumerable<ScoredGenotype> genome)
		{
			return genome.ToList().Select(g => { g.Score ??= Evaluate(g.Genotype); return g; });
		}

		private int Evaluate(Genotype genotype)
		{
			_traversalMap = _graph.Edges.ToDictionary(x => x, x => false);
			var orgin = _graph.Vertices.First(x => x.Id == 0);
			var current = orgin;
			_score = 0;
			_sand = _configuration.SandAmount;
			genotype.Edges.ToList().ForEach(x => Move(current, x));

			return _score;
		}

		private void Move(Vertex start, Edge goal)
		{
			if (goal.Length > _sand)
			{
				GoToBase(start, goal);
			}

			Move(start, goal, _routeToEdge[(start, goal)], goal.Length);
		}

		private void Move(Vertex start, Edge geneGoal, Route route, int sandNeeded)
		{
			if (!route.EdgePath.Any())
			{
				return;
			}
			var edgeToTraverse = route.EdgePath[0];

			if (getFirstUntraversed(route, _traversalMap).Length > _sand && edgeToTraverse.Length > _sand)
			{
				GoToBase(start, geneGoal);
			}
			else
			{
				if (!_traversalMap[edgeToTraverse])
				{
					_traversalMap[edgeToTraverse] = true;
					_sand -= edgeToTraverse.Length;
					route.moveOne();
					Move(edgeToTraverse.GetOtherVertex(start), geneGoal, route, sandNeeded);
				}
				else
				{
					_score += edgeToTraverse.Length;
					route.moveOne();
					Move(edgeToTraverse.GetOtherVertex(start), geneGoal, route, sandNeeded);
				}
			}
		}

		private void GoToBase(Vertex start, Edge geneGoal, Route route = null)
		{
			if(start.Id == 0)
			{
				_sand = _configuration.SandAmount;
				Move(start, geneGoal);
			}
			else
			{
				if (route == null)
					route = _routeToVertex[(start, _graph.Vertices.First(e => e.Id == 0))];

				var edgeToTraverse = route.EdgePath[0];
				
				if(_sand < edgeToTraverse.Length)
				{
					_score += edgeToTraverse.Length;	
				}
				else
				{
					_sand -= edgeToTraverse.Length;
					_traversalMap[edgeToTraverse] = true;
				}

				route.moveOne();
				GoToBase(edgeToTraverse.GetOtherVertex(start), geneGoal, route);
			}
			
		}

		private Edge getFirstUntraversed(Route route, Dictionary<Edge, bool> traversalMap)
		{
			return route.EdgePath.First(e => traversalMap[e] == false);
		}
	}
}
