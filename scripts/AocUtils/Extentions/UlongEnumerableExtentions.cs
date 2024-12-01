namespace AocUtils;

using System;
using System.Collections.Generic;
using System.Linq;

public static class UlongEnumerableExtentions
{
	public static ulong Sum(this IEnumerable<ulong> ulongs)
		=> ulongs.Aggregate(0ul, (acc, x) => acc + x);

}
