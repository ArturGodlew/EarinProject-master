using System;
using System.Collections.Generic;
using System.Text;

namespace EarinProject.RouteFinder.GenomeManagement
{
    public class ScoredGenotype
    {
        public Genotype Genotype { get; }
        public int? Score { get; set; }

        public ScoredGenotype(Genotype genotype)
        {
            Genotype = genotype;
        }
    }
}
