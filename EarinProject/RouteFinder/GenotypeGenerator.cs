using Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EarinProject
{
	public class GenotypeGenerator
	{
		private readonly List<Edge> _edges;
		private readonly Random _rand = new Random();

		public GenotypeGenerator(List<Edge> edges)
		{
			_edges = edges;
		}

		public Genotype Generate()
		{
			var rand = new Random();
			return new Genotype(_edges.OrderBy(_ => rand.Next(0, 1000 * _edges.Count)).ToArray());
		}

		public List<Genotype> Generate(int amount)
		{
			return Enumerable.Repeat(new Genotype(_edges.OrderBy(_ => _rand.Next(0, 1000 * _edges.Count)).ToArray()), amount).ToList();
		}

		public Genotype Breed(Genotype first, Genotype second, float mutation)
		{
			var crossoverSize = _rand.Next(2, first.Edges.Length);
			var crossoverPosition = _rand.Next(0, 1 + first.Edges.Length - crossoverSize);
			var crossoverPart = (first.Edges.ToList().GetRange(crossoverSize, crossoverPosition), second.Edges.ToList().GetRange(crossoverSize, crossoverPosition));

			var genes = first.Edges.Except(crossoverPart.Item2).ToList();
			genes.InsertRange(crossoverPosition, crossoverPart.Item1);
			
			return (_rand.Next(0, 1000) < (int)(mutation * 1000)) ? new Genotype(Mutate(genes.ToArray())) : new Genotype(genes.ToArray());
		}

		private Edge[] Mutate(Edge[] genes)
		{
			/*
			var position1 = _rand.Next(0, genes.Length);
			var position2 = _rand.Next(0, genes.Length - 1);
			// :[
			if(position1 == position2)
			{
				position2 = genes.Length - 1;
			}
			var tmp = genes[position1];
			genes[position1] = genes[position2];
			genes[position2] = tmp;

			return genes;*/

			var firstGene = genes.TakeRandom();
			var secondGene = genes.Except(new[] { firstGene }).TakeRandom();
			var tmp = firstGene;
			firstGene = secondGene;
			secondGene = tmp;

			return genes;			
		}

	}
}
