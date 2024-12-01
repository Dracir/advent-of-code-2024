using System;

namespace AocUtils;

public struct RectInt
{
	public int X;
	public int Y;
	public int Width;
	public int Height;
	public int Left => X;
	public int Bottom => Y;
	public readonly int Top => Y + Height - 1;
	public readonly int Right => X + Width - 1;

	public RangeInt WidthRange => new RangeInt(0, Width);
	public RangeInt HeightRange => new RangeInt(0, Height);

	public RectInt(int x, int y, int w, int h)
	{
		X = x;
		Y = y;
		Width = w;
		Height = h;
	}
	public RectInt(Vector2Int bottomLeft, Vector2Int topRight)
	{
		X = bottomLeft.X;
		Y = bottomLeft.Y;
		Width = topRight.X - bottomLeft.X + 1;
		Height = topRight.Y - bottomLeft.Y + 1;
	}

	public readonly RectInt Grow(int left, int top, int right, int bottom)
	{
		return new RectInt(X - left, Y - bottom, Width + left + right, Height + bottom + top);
	}

	public readonly bool IsOnBorder(Vector2Int pt)
	{
		if (!Contains(pt)) return false;

		return pt.X == X || pt.X == Right || pt.Y == Y || pt.Y == Top;
	}

	public readonly RectInt GrowToInclude(Vector2Int point)
	{
		var x = Math.Min(X, point.X);
		var y = Math.Min(Y, point.Y);
		var w = Math.Max(Right, point.X) - x + 1;
		var h = Math.Max(Top, point.Y) - y + 1;

		return new RectInt(x, y, w, h);
	}

	public readonly bool Contains(Vector2Int point) => Contains(point.X, point.Y);

	public readonly bool Contains(int x, int y) => x >= X && x <= Right && y >= Y && y <= Top;
}
