using System;

public static class StringExtentions
{
	public static int IndexOf<T>(this Span<T> str, T value)
	{
		for (int i = 0; i < str.Length; i++)
		{
			if (str[i].Equals(value)) return i;
		}
		return -1;
	}
}
