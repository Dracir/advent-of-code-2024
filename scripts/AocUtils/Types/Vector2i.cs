using System;

namespace AocUtils;

public struct Vector2Int
{
	public static Vector2Int ZERO = new(0, 0);
	public int X;
	public int Y;

	public readonly Vector2Int Up => new(X, Y + 1);
	public readonly Vector2Int Down => new(X, Y - 1);
	public readonly Vector2Int Left => new(X - 1, Y);
	public readonly Vector2Int Right => new(X + 1, Y);
	public readonly Vector2Int UpLeft => new(X - 1, Y + 1);
	public readonly Vector2Int UpRight => new(X + 1, Y + 1);
	public readonly Vector2Int DownLeft => new(X - 1, Y - 1);
	public readonly Vector2Int DownRight => new(X + 1, Y - 1);

	public readonly Vector2Int[] Ortogonal => new Vector2Int[] { Up, Down, Left, Right };
	public readonly Vector2Int[] Diagonal => new Vector2Int[] { UpLeft, UpRight, DownLeft, DownRight };

	public static Vector2Int UpDirection => new(0, 1);
	public static Vector2Int DownDirection => new(0, -1);
	public static Vector2Int LeftDirection => new(-1, 0);
	public static Vector2Int RightDirection => new(1, 0);
	public static Vector2Int UpLeftDirection => new(-1, 1);
	public static Vector2Int UpRightDirection => new(1, 1);
	public static Vector2Int DownLeftDirection => new(-1, -1);
	public static Vector2Int DownRightDirection => new(1, -1);

	public static Vector2Int[] OrtogonalDirections => new Vector2Int[] { UpDirection, DownDirection, LeftDirection, RightDirection };
	public static Vector2Int[] DiagonalDirections => new Vector2Int[] { UpLeftDirection, UpRightDirection, DownLeftDirection, DownRightDirection };

	public Vector2Int(int x, int y)
	{
		X = x;
		Y = y;
	}

	public readonly void Deconstruct(out int x, out int y)
	{
		x = X;
		y = Y;
	}

	public static Vector2Int operator *(Vector2Int other, int multiplier) => new(other.X * multiplier, other.Y * multiplier);

	public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new(a.X + b.X, a.Y + b.Y);
	public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new(a.X - b.X, a.Y - b.Y);

	public readonly int DistanceManhattan(Vector2Int other)
	{
		return Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
	}



	public readonly Vector2Int RotateLeft() => new(-Y, X);
	public readonly Vector2Int RotateRight() => new(Y, -X);

	public readonly override int GetHashCode() => HashCode.Combine(X, Y);

	public readonly override string ToString() => $"({X}, {Y})";

	public readonly override bool Equals(object? obj)
	{
		return obj is Vector2Int point &&
			   X == point.X && Y == point.Y;
	}

	public static bool operator ==(Vector2Int left, Vector2Int right) => left.X == right.X && left.Y == right.Y;

	public static bool operator !=(Vector2Int left, Vector2Int right) => left.X != right.X || left.Y != right.Y;
}