namespace AocUtils;

public struct RangeLong
{
	public long Min;
	public long Max;

	public RangeLong(long min, long max)
	{
		Min = min;
		Max = max;
	}

	public readonly long Width => Max - Min + 1;

	public readonly bool Contains(long value) => value >= Min && value <= Max;

}