using System;

namespace AocUtils;

public struct Point3Int
{
	public static Point3Int ZERO = new(0, 0, 0);
	public int X;
	public int Y;
	public int Z;

	public Point3Int(int x, int y, int z)
	{
		X = x;
		Y = y;
		Z = z;
	}

	public readonly void Deconstruct(out int x, out int y, out int z)
	{
		x = X;
		y = Y;
		z = Z;
	}

	public readonly int DistanceManhattan(Point3Int other)
	{
		return Math.Abs(X - other.X) + Math.Abs(Y - other.Y) + Math.Abs(Z - other.Z);
	}

	public readonly override bool Equals(object? obj)
	{
		return obj is Point3Int d &&
			   X == d.X &&
			   Y == d.Y &&
			   Z == d.Z;
	}

	public readonly override int GetHashCode() => HashCode.Combine(X, Y, Z);

	public readonly override string ToString() => $"({X}, {Y}, {Z})";
}