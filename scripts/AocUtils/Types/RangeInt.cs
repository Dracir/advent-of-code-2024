namespace AocUtils;

public struct RangeInt
{
	public int Min;
	public int Max;

	public RangeInt(int min, int max)
	{
		Min = min;
		Max = max;
	}

	public readonly int Width => Max - Min + 1;

	public readonly bool Contains(int value) => value >= Min && value <= Max;

}