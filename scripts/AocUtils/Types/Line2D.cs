using System;
using System.Collections.Generic;
using System.Linq;

namespace AocUtils;

public struct Line2DInt
{
	public Vector2Int A;
	public Vector2Int B;

	public Line2DInt(Vector2Int a, Vector2Int b)
	{
		A = a;
		B = b;
	}

	public readonly int MinX => Math.Min(A.X, B.X);
	public readonly int MinY => Math.Min(A.Y, B.Y);
	public readonly int MaxX => Math.Max(A.X, B.X);
	public readonly int MaxY => Math.Max(A.Y, B.Y);
	public readonly int Width => Math.Abs(A.X - B.X) + 1;
	public readonly int Height => Math.Abs(A.Y - B.Y) + 1;

	public readonly bool IsHorizontal => A.Y == B.Y;
	public readonly bool IsVertical => A.X == B.X;
	public readonly bool IsDiagonal => Width == Height;

	public readonly IEnumerable<Vector2Int> Points()
	{
		if (IsHorizontal)
		{
			for (var x = MinX; x <= MaxX; x++)
				yield return new Vector2Int(x, A.Y);
		}
		else if (IsVertical)
		{
			for (var y = MinY; y <= MaxY; y++)
				yield return new Vector2Int(A.X, y);
		}
		else if (IsDiagonal)
		{
			var xFactor = A.X > B.X ? -1 : 1;
			var yFactor = A.Y > B.Y ? -1 : 1;

			foreach (var i in Enumerable.Range(0, Height))
				yield return new Vector2Int(A.X + xFactor * i, A.Y + yFactor * i);
		}
	}

}