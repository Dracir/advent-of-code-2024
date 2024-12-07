using AocUtils;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Day2 : Node
{
	[Export]
	TextFileResource Input;

	public override void _Ready()
	{
		var input = Input.GetContent().ParseListOfPairOfInt("\n", "   ");
		GD.Print("Part 1: " + Part1());
		GD.Print("Part 2: " + Part2());
	}

	private string Part1()
	{
		return "part1";
	}


	private string Part2()
	{
		return "part2";
	}

}
