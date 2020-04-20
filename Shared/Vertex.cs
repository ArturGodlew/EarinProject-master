#nullable enable
using System;
using System.Collections.Generic;

namespace Shared
{
	public class Vertex
	{
		public List<Edge> Edges { get; set; } = new List<Edge>();
		public int Id { get; set; }

		public Vertex() { }
		public Vertex(int id)
		{
			Id = id;
		}
	}
}