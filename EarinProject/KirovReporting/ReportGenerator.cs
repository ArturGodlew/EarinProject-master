using EarinProject.RouteFinder.GenomeManagement;
using System;
using System.Collections.Generic;
using System.Text;

namespace EarinProject.KirovReporting
{
	public static class ReportGenerator
	{
		public static SimpleReport GenerateSimpleReport(IEnumerable<ScoredGenotype> pool)
		{
			throw new NotImplementedException();
		}

		public static SimpleReport GenerateSimpleReport(List<SimpleReport> reports)
		{
			//everage them out
			throw new NotImplementedException();
		}

		public static FullReport GenerateFullReport(List<SimpleReport> reports)
		{
			//get all grapghs and best solutions
			// get lists of stat changes for each stat
			throw new NotImplementedException();
		}
	}
}
