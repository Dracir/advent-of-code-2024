
using System;
using System.Collections.Generic;
using System.Linq;

namespace AocUtils;

public class Grid<T> : IGrid<T>
{
	private T[,] _values;

	private int _offsetX;
	private int _offsetY;
	private int usedMinX;
	private int usedMinY;
	private int usedMaxX;
	private int usedMaxY;


	public int UsedMinX => usedMinX;
	public int UsedMinY => usedMinY;
	public int UsedMaxX => usedMaxX;
	public int UsedMaxY => usedMaxY;


	public int MinX => -OffsetX;
	public int MinY => -OffsetY;
	public int MaxX => -OffsetX + FullWidth - 1;
	public int MaxY => -OffsetY + FullHeight - 1;

	public int FullWidth => _values.GetLength(0);
	public int FullHeight => _values.GetLength(1);
	public int UsedWidth => usedMaxX - usedMinX + 1;
	public int UsedHeight => usedMaxY - usedMinY + 1;

	public int OffsetX => _offsetX;
	public int OffsetY => _offsetY;

	protected T DefaultValue;

	public Grid(T defaultValue, int xRange, int yRange)
	{
		this.DefaultValue = defaultValue;
		_offsetX = 0;
		_offsetY = 0;
		usedMinX = 0;
		usedMaxX = 0;
		usedMinY = 0;
		usedMaxY = 0;
		_values = new T[xRange, yRange];
		foreach (var item in PointsAndValues())
			this[item.Point] = defaultValue;
	}
	public Grid(T defaultValue, RangeInt xRange, RangeInt yRange)
	{
		this.DefaultValue = defaultValue;
		_offsetX = -xRange.Min;
		_offsetY = -yRange.Min;
		usedMinX = _offsetX;
		usedMaxX = _offsetX;
		usedMinY = _offsetY;
		usedMaxY = _offsetY;
		_values = new T[xRange.Max - xRange.Min + 1, yRange.Max - yRange.Min + 1];
		foreach (var item in PointsAndValues())
			this[item.Point] = defaultValue;
	}


	public virtual T this[Vector2Int key]
	{
		get { return this[key.X, key.Y]; }
		set { this[key.X, key.Y] = value; }
	}

	public virtual T this[int x, int y]
	{
		get { return _values[x + _offsetX, y + _offsetY]; }
		set
		{
			_values[x + _offsetX, y + _offsetY] = value;
			usedMinX = Math.Min(usedMinX, x);
			usedMinY = Math.Min(usedMinY, y);
			usedMaxX = Math.Max(usedMaxX, x);
			usedMaxY = Math.Max(usedMaxY, y);
		}
	}

	public bool TryGet(Vector2Int pt, out T value)
	{
		if (PointInBound(pt))
		{
			value = this[pt];
			return true;
		}
		else
		{
			value = DefaultValue;
			return false;
		}
	}

	public bool TryGet(int x, int y, out T value)
	{
		if (PointInBound(x, y))
		{
			value = this[x, y];
			return true;
		}
		else
		{
			value = DefaultValue;
			return false;
		}
	}


	public Vector2Int TopLeft => new(usedMinX, usedMaxY);
	public Vector2Int TopRight => new(usedMaxX, usedMaxY);
	public Vector2Int BottomLeft => new(usedMinX, usedMinY);
	public Vector2Int BottomRight => new(usedMaxX, usedMinY);
	public Vector2Int Center => new(_offsetX, _offsetY);

	public IEnumerable<Vector2Int> Points()
	{
		for (int x = usedMinX; x <= usedMaxX; x++)
			for (int y = usedMinY; y <= usedMaxY; y++)
				yield return new Vector2Int(x, y);
	}
	public IEnumerable<(Vector2Int Point, T Value)> PointsAndValues()
	{
		for (int x = usedMinX; x <= usedMaxX; x++)
			for (int y = usedMinY; y <= usedMaxY; y++)
				yield return (new Vector2Int(x, y), this[x, y]);
	}


	public IEnumerable<Vector2Int> PointsOnBorder()
	{
		for (int x = MinX; x <= MaxX; x++)
		{
			yield return new Vector2Int(x, MinY);
			yield return new Vector2Int(x, MaxY);
		}
		for (int y = MinY; y <= MaxY; y++)
		{
			yield return new Vector2Int(MinX, y);
			yield return new Vector2Int(MaxX, y);
		}
	}


	public IEnumerable<Vector2Int> PointsExpendedFromBorder(int expendedAmount)
	{
		for (int x = MinX - expendedAmount; x <= MaxX + expendedAmount; x++)
		{
			yield return new Vector2Int(x, MinY - expendedAmount);
			yield return new Vector2Int(x, MaxY + expendedAmount);
		}
		for (int y = MinY - expendedAmount; y <= MaxY + expendedAmount; y++)
		{
			yield return new Vector2Int(MinX - expendedAmount, y);
			yield return new Vector2Int(MaxX + expendedAmount, y);
		}
	}


	public IEnumerable<Vector2Int> AreaSquareAround(Vector2Int pt, int radiusDistance)
	{

		int x1 = Math.Max(usedMinX, pt.X - radiusDistance);
		int y1 = Math.Max(usedMinY, pt.Y - radiusDistance);
		int x2 = Math.Min(usedMaxX, pt.X + radiusDistance);
		int y2 = Math.Min(usedMaxY, pt.Y + radiusDistance);

		for (int x = x1; x <= x2; x++)
			for (int y = y1; y <= y2; y++)
				yield return new Vector2Int(x, y);
	}

	public IEnumerable<T> Row(int y)
	{
		for (int x = usedMinX; x <= usedMaxX; x++)
			yield return this[x, y];
	}

	public IEnumerable<T> Col(int x)
	{
		for (int y = usedMinY; y <= usedMaxY; y++)
			yield return this[x, y];
	}

	public IEnumerable<(Vector2Int Point, T Value)> PointAndValuesSquareAround(Vector2Int pt, int radiusDistance)
	{

		int x1 = Math.Max(usedMinX, pt.X - radiusDistance);
		int y1 = Math.Max(usedMinY, pt.Y - radiusDistance);
		int x2 = Math.Min(usedMaxX, pt.X + radiusDistance);
		int y2 = Math.Min(usedMaxY, pt.Y + radiusDistance);

		for (int x = x1; x <= x2; x++)
			for (int y = y1; y <= y2; y++)
				yield return (new Vector2Int(x, y), this[x, y]);
	}


	public IEnumerable<(Vector2Int Point, T Value)> PointAndValuesInDirection(Vector2Int pt, int dx, int dy, bool includeStartingPoint)
	{
		var x = pt.X;
		var y = pt.Y;
		if (!includeStartingPoint)
		{
			x += dx;
			y += dy;
		}

		while (x >= usedMinX && x <= usedMaxX && y >= usedMinY && y <= usedMaxY)
		{
			yield return (new Vector2Int(x, y), this[x, y]);
			x += dx;
			y += dy;
		}
	}

	public IEnumerable<Vector2Int> AreaAround(Vector2Int pt, int manhattanDistance)
	{
		int x1 = Math.Max(usedMinX, pt.X - manhattanDistance);
		int y1 = Math.Max(usedMinY, pt.Y - manhattanDistance);
		int x2 = Math.Min(usedMaxX, pt.X + manhattanDistance);
		int y2 = Math.Min(usedMaxY, pt.Y + manhattanDistance);

		for (int x = x1; x <= x2; x++)
			for (int y = y1; y <= y2; y++)
			{
				var distance = Math.Abs(pt.X - x) + Math.Abs(pt.Y - y);
				if (distance <= manhattanDistance)
					yield return new Vector2Int(x, y);
			}
	}

	public IEnumerable<int> ColIndexs()
	{
		for (int y = usedMinY; y <= usedMaxY; y++)
			yield return y;
	}

	public IEnumerable<int> RowIndexs()
	{
		for (int x = usedMinX; x <= usedMaxX; x++)
			yield return x;
	}

	public T[,] ToArray()
	{
		var arr = new T[UsedWidth, UsedHeight];
		for (int x = usedMinX; x <= usedMaxX; x++)
			for (int y = usedMinY; y <= usedMaxY; y++)
				arr[x - usedMinX, y - usedMinY] = _values[x, y];
		return arr;
	}

	public void AddGrid(int leftX, int bottomY, T[,] grid, GridPlane plane)
	{
		if (plane == GridPlane.XY)
		{
			for (int x = 0; x < grid.GetLength(0); x++)
				for (int y = 0; y < grid.GetLength(1); y++)
					this[x + leftX, y + bottomY] = grid[x, y];

		}
		else
		{
			for (int x = 0; x < grid.GetLength(1); x++)
				for (int y = 0; y < grid.GetLength(0); y++)
					this[x + leftX, y + bottomY] = grid[y, x];
		}
	}

	public bool XInBound(int x) => x >= MinX && x <= MaxX;
	public bool YInBound(int y) => y >= MinY && y <= MaxY;
	public bool PointInBound(Vector2Int pt) => XInBound(pt.X) && YInBound(pt.Y);
	public bool PointInBound(int x, int y) => XInBound(x) && YInBound(y);

	public static Grid<T> FromArray(T defaultValue, T[,] sourceGrid, GridPlane plane)
	{
		var xRange = new RangeInt(0, 0);
		var yRange = new RangeInt(0, 0);
		if (plane == GridPlane.XY)
		{
			xRange.Max = sourceGrid.GetLength(0);
			yRange.Max = sourceGrid.GetLength(1);
		}
		else
		{
			xRange.Max = sourceGrid.GetLength(1);
			yRange.Max = sourceGrid.GetLength(0);
		}

		var grid = new Grid<T>(defaultValue, xRange, yRange);
		grid.AddGrid(0, 0, sourceGrid, plane);
		return grid;
	}



	public void ApplyLine(Line2DInt line, Func<(T currentValue, Vector2Int position), T> valueChange)
	{
		foreach (var point in line.Points())
			this[point] = valueChange((this[point], point));
	}
}
