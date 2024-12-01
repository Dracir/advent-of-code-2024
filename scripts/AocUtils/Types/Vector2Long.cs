using System;

namespace AocUtils;

public struct Vector2Long
{
	public long X;
	public long Y;

	public Vector2Long(long x, long y)
	{
		X = x;
		Y = y;
	}

	public readonly void Deconstruct(out long x, out long y)
	{
		x = X;
		y = Y;
	}

	public readonly long DistanceManhattan(Vector2Long other)
	{
		return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
	}

}