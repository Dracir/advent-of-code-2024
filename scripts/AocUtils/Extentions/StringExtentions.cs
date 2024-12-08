namespace AocUtils;

using System;
using System.Collections.Generic;
using System.Linq;

public static class StringExtentions
{
	public static long[] ParseListOfLong(this string str, char separator = ' ')
		=> str.Split(separator, StringSplitOptions.RemoveEmptyEntries)
		.Select(long.Parse).ToArray();

	public static ulong[] ParseListOfUlong(this string str, char separator = ' ')
		=> str.Split(separator, StringSplitOptions.RemoveEmptyEntries)
		.Select(ulong.Parse).ToArray();

	public static bool[] ParseListOfBool(this string str, char trueValue = '#', char separator = ' ')
		=> str.Split(separator, StringSplitOptions.RemoveEmptyEntries)
		.Select(c => c[0] == trueValue).ToArray();

	public static bool?[] ParseListOfOptionalBool(this string str, char trueValue = '#', char falseValue = '.', char separator = ' ')
		=> str.Split(separator, StringSplitOptions.RemoveEmptyEntries)
		.Select(c =>
		{
			if (c[0] == trueValue) return true;
			else if (c[0] == falseValue) return false;
			else return default(bool?);
		}
		).ToArray();

	public static bool?[] ParseListOfOptionalBool(this string str, char trueValue = '#', char falseValue = '.')
		=> str.Select(c =>
		{
			if (c == trueValue) return true;
			else if (c == falseValue) return false;
			else return default(bool?);
		}
		).ToArray();

	public static int[] ParseListOfInt(this string str, char separator = ' ')
		=> str.Split(separator, StringSplitOptions.RemoveEmptyEntries)
		.Select(int.Parse).ToArray();

	public static int[] ParseListOfInt(this string str, string separator)
		=> str.Split(separator, StringSplitOptions.RemoveEmptyEntries)
		.Select(int.Parse).ToArray();

	public static (int, int)[] ParseListOfPairOfInt(this string str, string pairsSeparator = "\n", string intSeparator = " ")
		=> str.Split(pairsSeparator, StringSplitOptions.RemoveEmptyEntries)
		.Select(pair => pair.ParseListOfInt(intSeparator))
		.Select(pair => (pair[0], pair[1])).ToArray();

	public static string[] ParseListOfString(this string str, char separator = ' ')
		=> str.Split(separator, StringSplitOptions.RemoveEmptyEntries);

	public static List<int[]> ParseListsOfListOfInt(this string str, string listsSeparator = "\n", char elementsSeparator = ' ')
		=> str.Split(listsSeparator, StringSplitOptions.RemoveEmptyEntries)
		.Select(list => list.ParseListOfInt())
		.ToList();
}
