using Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EarinProject.KirovReporting
{
    public class FullReport
    {
        public List<(Graph Graph, Route BestRoute)> Solutions { get; set; }

        // list of stat changes for every stat - everage score of the generation, gene diversity ( liczba unikalnych wartosci na danej pozycji w genie), stuff 
        
        public FullReport(List<SimpleReport> Reports)
        {
            throw new NotImplementedException();
            //solutions = (graphFromEach , reports.LastFromEach())
            //stat = lista srednich z kazdego etapu  
        }

        public void ToFile(FileInfo file)
        {
            var writer = new StreamWriter(file.FullName, false);
            writer.Write(Encoding.ASCII.GetBytes(this.ToString()));
        }
        
    }
}
