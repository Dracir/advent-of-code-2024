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
		var input = Input.GetContent().ParseListsOfListOfInt("\n", ' ');
		GD.Print("Part 1: " + Part1(input));
		GD.Print("Part 2: " + Part2());
	}

	private string Part1(List<int[]> input)
	{
		var rd = RenderingServer.CreateLocalRenderingDevice();

		var shader = CreateShader(rd, "res://scripts/days/d2/day2part1.glsl");
		var (inputBuffer, inputBufferUniform) = Create2DIntBuffer(input.ToArray(), 0, rd, -1);
		var uniformSet = rd.UniformSetCreate(new Array<RDUniform> { inputBufferUniform }, shader, 0);
		CreateAndRunPipeline(rd, shader, uniformSet, yGroups: (int)input.Count);

		var output = GetOutputBufferData(input, rd, inputBuffer);

		var safeCount = output.Count(x => x[0] == 1);
		GD.Print("Safe count: ", safeCount);

		var linesAsStr = output.Select(x => string.Join(", ", x.Select(x => x.ToString())));

		GD.Print("Output: \n", string.Join("\n", linesAsStr));


		return "part1";
	}

	private static int[][] GetOutputBufferData(List<int[]> input, RenderingDevice rd, Rid inputBuffer)
	{
		var outputBytes = rd.BufferGetData(inputBuffer);
		var output = new int[input.Count][];
		for (int i = 0; i < input.Count; i++)
		{
			var rowLength = input[i].Length;
			output[i] = new int[rowLength];
			var tmpOutput = new float[rowLength];
			Buffer.BlockCopy(outputBytes, i * 1024 * sizeof(float), tmpOutput, 0, rowLength * sizeof(float));
			for (int j = 0; j < rowLength; j++)
				output[i][j] = (int)tmpOutput[j];
		}

		return output;
	}

	private static void CreateAndRunPipeline(RenderingDevice rd, Rid shader, Rid uniformSet, int xGroups = 1, int yGroups = 1, int zGroups = 1)
	{
		var pipeline = rd.ComputePipelineCreate(shader);
		var computeList = rd.ComputeListBegin();
		rd.ComputeListBindComputePipeline(computeList, pipeline);
		rd.ComputeListBindUniformSet(computeList, uniformSet, 0);
		rd.ComputeListDispatch(computeList, xGroups: (uint)xGroups, yGroups: (uint)yGroups, zGroups: (uint)zGroups);
		rd.ComputeListEnd();

		// Submit to GPU and wait for sync
		rd.Submit();
		rd.Sync();
	}

	private static Rid CreateShader(RenderingDevice rd, string shaderPath)
	{
		var shaderFile = GD.Load<RDShaderFile>(shaderPath);
		var shaderBytecode = shaderFile.GetSpirV();
		return rd.ShaderCreateFromSpirV(shaderBytecode);
	}

	private static (Rid buffer, RDUniform bufferUniform) Create2DIntBuffer(int[][] values, int binding, RenderingDevice rd, int nullValue)
	{
		// TODO fill to the max length of the array instead of having them all at 1024
		var bytes = new byte[values.Length * 1024 * sizeof(float)];
		for (int i = 0; i < values.Length; i++)
		{
			var row = new int[1024];
			for (int j = 0; j < values[i].Length; j++)
				row[j] = values[i][j];
			for (int j = values[i].Length; j < 1024; j++)
				row[j] = nullValue;

			Buffer.BlockCopy(row.Select(x => (float)x).ToArray(), 0, bytes, i * 1024 * sizeof(float), 1024 * sizeof(float));
		}
		var buffer = rd.StorageBufferCreate((uint)bytes.Length, bytes);

		// Create a uniform to assign the buffer to the rendering device
		var uniform = new RDUniform
		{
			UniformType = RenderingDevice.UniformType.StorageBuffer,
			Binding = binding
		};
		uniform.AddId(buffer);
		return (buffer, uniform);
	}

	private string Part2()
	{
		return "part2";
	}



}
