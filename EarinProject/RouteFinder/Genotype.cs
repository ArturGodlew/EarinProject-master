using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarinProject
{
	public class Genotype
	{
		public Genotype(Edge[] edges)
		{
			Edges = edges;
		} 

		public Edge[] Edges { get; }
	}
}
