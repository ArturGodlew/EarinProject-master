using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EarinProject.RouteFinder.Evaluation
{
    public class Dijkstra
    {
        private Graph _graph { get; set; }
        private Dictionary<Vertex, int?> _distances { get; set; }

        public Dijkstra(Graph graph)
        {
            _graph = graph;
            
        }
        
        public Route FindPath(Vertex start, Vertex goal)
        {
            Console.WriteLine($"path from {start.Id} to {goal.Id}");
            _distances = _graph.Vertices.ToDictionary(x => x, x => new int?());
            _distances[start] = 0;

            foreach (var edge in start.Edges)
            {
                moveTo(edge.GetOtherVertex(start), goal, edge.Length);
            }

            return ChoosePreviousNode(goal, new Route(start, goal, _distances[goal] ?? -1));
        }

        private void moveTo(Vertex start, Vertex goal, int distance)
        {
            if ((_distances[start].HasValue && _distances[start] <= distance) || _distances[goal].HasValue && _distances[goal] <= distance)
            {
                return;
            }

            _distances[start] = distance;

            if (start.Id == goal.Id)
            {
                return;
            }

            foreach (var edge in start.Edges)
            {
                moveTo(edge.GetOtherVertex(start), goal, distance + edge.Length);
            }
        }

        private Route ChoosePreviousNode(Vertex end, Route route)
        {
            if(_distances[end] == 0)
            {
                return route;
            }

            var aveliablePaths = end.Edges.Except(route.EdgePath);
            var lowestCost = aveliablePaths.Min(x => _distances[x.GetOtherVertex(end)] + x.Length);
            var previousPath = aveliablePaths.ToList().Find(e => _distances[e.GetOtherVertex(end)] == lowestCost - e.Length);



            var previousNode = previousPath.GetOtherVertex(end);
            route.Path = route.Path.Prepend(previousNode).ToList();
            route.EdgePath = route.EdgePath.Prepend(previousPath).ToList();
            return ChoosePreviousNode(previousNode, route);
        }
    }
}
