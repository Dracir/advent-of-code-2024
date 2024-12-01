
using System;
using System.Collections.Generic;
using System.Linq;

namespace AocUtils;

public class GrowingGrid<T> : IGrid<T>
{
	private T _defaultValue;

	public int UsedMinX => _grid.UsedMinX;
	public int UsedMinY => _grid.UsedMinY;
	public int UsedMaxX => _grid.UsedMaxX;
	public int UsedMaxY => _grid.UsedMaxY;
	public int MinX => _grid.MinX;
	public int MinY => _grid.MinY;
	public int MaxX => _grid.MaxX;
	public int MaxY => _grid.MaxY;
	public int FullWidth => _grid.FullWidth;
	public int FullHeight => _grid.FullHeight;
	public int UsedWidth => _grid.UsedWidth;
	public int UsedHeight => _grid.UsedHeight;

	private Grid<T> _grid;
	private int _growthIncrement;
	private bool _growsOnRead;
	private bool _growsOnWrite;

	public int GrowthTimesRight = 0;
	public int GrowthTimesLeft = 0;
	public int GrowthTimesUp = 0;
	public int GrowthTimesDown = 0;
	public int GrowthTimes => GrowthTimesRight + GrowthTimesLeft + GrowthTimesUp + GrowthTimesDown;

	public Vector2Int TopLeft => _grid.TopLeft;
	public Vector2Int TopRight => _grid.TopRight;
	public Vector2Int BottomLeft => _grid.BottomLeft;
	public Vector2Int BottomRight => _grid.BottomRight;
	public Vector2Int Center => _grid.Center;

	public bool XInBound(int x) => _grid.XInBound(x);
	public bool YInBound(int y) => _grid.YInBound(y);
	public bool PointInBound(Vector2Int pt) => _grid.PointInBound(pt);

	public Action<GrowingGridEvent>? OnGridGrown;

	public GrowingGrid(T defaultValue, RangeInt xRange, RangeInt yRange, int growthIncrement, bool growsOnRead = true, bool growsOnWrite = true)
	{
		_grid = new Grid<T>(defaultValue, xRange, yRange);
		_defaultValue = defaultValue;
		_growthIncrement = growthIncrement;
		_growsOnRead = growsOnRead;
		_growsOnWrite = growsOnWrite;
	}

	public GrowingGrid(T defaultValue, T[,] startingGrid, GridPlane plane, int growthIncrement, bool growsOnRead = true, bool growsOnWrite = true)
	{
		_defaultValue = defaultValue;
		_growthIncrement = growthIncrement;
		_growsOnRead = growsOnRead;
		_growsOnWrite = growsOnWrite;

		int xIndex = plane == GridPlane.YX ? 1 : 0;
		int yIndex = plane == GridPlane.YX ? 0 : 1;
		var xRange = new RangeInt(0, startingGrid.GetLength(xIndex) - 1);
		var yRange = new RangeInt(0, startingGrid.GetLength(yIndex) - 1);

		_grid = new Grid<T>(defaultValue, xRange, yRange);

		AddGrid(0, 0, startingGrid, plane);
	}

	public T this[Vector2Int key]
	{
		get { return this[key.X, key.Y]; }
		set { this[key.X, key.Y] = value; }
	}

	public T this[int x, int y]
	{
		get
		{
			var targetX = x;
			var targetY = y;
			if (_growsOnRead)
				GrowIfNeeded(targetX, targetY);
			return _grid[targetX, targetY];
		}
		set
		{
			var targetX = x;
			var targetY = y;
			if (_growsOnWrite)
				GrowIfNeeded(targetX, targetY);
			_grid[targetX, targetY] = value;
		}
	}

	private void GrowIfNeeded(int targetX, int targetY)
	{
		var growthNeeded = false;
		int left = 0, right = 0, top = 0, bottom = 0;
		if (targetX < _grid.MinX)
		{
			growthNeeded = true;
			GrowthTimesLeft++;
			left = _growthIncrement * (int)Math.Ceiling(MathF.Abs(targetX - _grid.MinX) / _growthIncrement);
		}
		if (targetX > _grid.MaxX)
		{
			growthNeeded = true;
			GrowthTimesRight++;
			right = _growthIncrement * (int)Math.Ceiling((targetX - _grid.MaxX) * 1f / _growthIncrement);
		}
		if (targetY < _grid.MinY)
		{
			growthNeeded = true;
			GrowthTimesDown++;
			bottom = _growthIncrement * (int)Math.Ceiling(MathF.Abs(targetY - _grid.MinY) / _growthIncrement);
		}
		if (targetY > _grid.MaxY)
		{
			growthNeeded = true;
			GrowthTimesUp++;
			top = _growthIncrement * (int)Math.Ceiling((targetY - _grid.MaxY) * 1f / _growthIncrement);
		}
		if (growthNeeded)
			GrowGrid(top, right, bottom, left);
	}

	public void AddGrid(int leftX, int bottomY, T[,] grid, GridPlane plane) => _grid.AddGrid(leftX, bottomY, grid, plane);

	private void GrowGrid(int up, int right, int down, int left)
	{
		var newGrid = new Grid<T>(_defaultValue, new RangeInt(_grid.MinX - left, _grid.MaxX + right), new RangeInt(_grid.MinY - down, _grid.MaxY + up));

		for (int x = _grid.MinX; x <= _grid.MaxX; x++)
			for (int y = _grid.MinY; y <= _grid.MaxY; y++)
				newGrid[x, y] = _grid[x, y];

		_grid = newGrid;
		OnGridGrown?.Invoke(new GrowingGridEvent(this, up, right, down, left));
	}

	public IEnumerable<Vector2Int> Points() => _grid.Points();
	public IEnumerable<Vector2Int> AreaSquareAround(Vector2Int pt, int radiusDistance) => _grid.AreaSquareAround(pt, radiusDistance);
	public IEnumerable<Vector2Int> AreaAround(Vector2Int pt, int manhattanDistance) => _grid.AreaAround(pt, manhattanDistance);
	public IEnumerable<int> ColIndexs() => _grid.ColIndexs();
	public IEnumerable<int> RowIndexs() => _grid.RowIndexs();
	public T[,] ToArray() => _grid.ToArray();

	public bool TryGet(Vector2Int pt, out T value)
	{
		throw new NotImplementedException();
	}

	public bool TryGet(int x, int y, out T value)
	{
		throw new NotImplementedException();
	}

	public struct GrowingGridEvent
	{
		public GrowingGrid<T> Grid;
		public int Up;
		public int Right;
		public int Down;
		public int Left;

		public GrowingGridEvent(GrowingGrid<T> growingGrid, int up, int right, int down, int left)
		{
			Grid = growingGrid;
			Up = up;
			Right = right;
			Down = down;
			Left = left;
		}
	}
}

