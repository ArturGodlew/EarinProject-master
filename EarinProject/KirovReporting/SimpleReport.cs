using Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarinProject.KirovReporting
{
    public class SimpleReport
    {
        public Graph Graph { get; set; }
        public Route BestRoute { get; set; }
        public int currentGeneration { get; set; }

        //stats - everage score of the generation, gene diversity ( liczba unikalnych wartosci na danej pozycji w genie), stuff 
    }
}
