using System.Collections.Generic;
using System.Linq;
using System;

namespace Shared
{
    public struct EdgeEntity
    {
        public int Id;
        public int VertexId1;
        public int VertexId2;
        public int Length;

    }

    public class Edge
    {
        public int Id { get; }
        public Vertex Vertex1 { get; }
        public Vertex Vertex2 { get; }
        public int Length { get; }

        //niechce mi sie bawic z jakim mapowaniem
        public bool Covered { get; set; } = false;

        public Edge() { }

        public Edge(int id, Vertex vertex1, Vertex vertex2, int lenght)
        {
            Id = id;
            Vertex1 = vertex1;
            Vertex2 = vertex2;
            Length = lenght;
        }

        public Vertex GetOtherVertex(Vertex vertex)
        {
            if (vertex == Vertex1)
            {
                return Vertex2;
            }

            if (vertex == Vertex2)
            {
                return Vertex1;
            }

            throw new Exception($"Vertex {vertex.Id} is not one of vertexes of edge {Id}. Correct vertecis are {Vertex1.Id} and {Vertex2.Id}");
        }
    }

}
