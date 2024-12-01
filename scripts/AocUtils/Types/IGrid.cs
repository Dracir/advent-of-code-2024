using System.Collections.Generic;

namespace AocUtils;

public interface IGrid<T>
{
	int UsedMinX { get; }
	int UsedMinY { get; }
	int UsedMaxX { get; }
	int UsedMaxY { get; }
	int MinX { get; }
	int MinY { get; }
	int MaxX { get; }
	int MaxY { get; }
	int UsedWidth { get; }
	int UsedHeight { get; }
	int FullWidth { get; }
	int FullHeight { get; }

	Vector2Int TopLeft { get; }
	Vector2Int TopRight { get; }
	Vector2Int BottomLeft { get; }
	Vector2Int BottomRight { get; }
	Vector2Int Center { get; }

	T this[Vector2Int key] { get; set; }
	T this[int x, int y] { get; set; }


	IEnumerable<Vector2Int> Points();
	IEnumerable<Vector2Int> AreaSquareAround(Vector2Int pt, int radiusDistance);
	IEnumerable<Vector2Int> AreaAround(Vector2Int pt, int manhattanDistance);
	IEnumerable<int> ColIndexs();
	IEnumerable<int> RowIndexs();
	T[,] ToArray();

	void AddGrid(int leftX, int bottomY, T[,] grid, GridPlane plane);

	bool XInBound(int x);
	bool YInBound(int y);
	bool PointInBound(Vector2Int pt);

	bool TryGet(Vector2Int pt, out T value);
	bool TryGet(int x, int y, out T value);

}

public enum GridPlane { XY, YX };
public enum GridAxe { X, Y };