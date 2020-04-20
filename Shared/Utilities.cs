using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared
{
	public static class Utilities
	{
		public static IEnumerable<T> TakeRandom<T>(this IEnumerable<T> range, int number)
		{
			var rand = new Random();
			return range.OrderBy(_ => rand.Next()).Take(number);
		}

		public static T TakeRandom<T>(this IEnumerable<T> range)
		{
			var rand = new Random();
			return range.OrderBy(_ => rand.Next()).First();
		}
	}
}
