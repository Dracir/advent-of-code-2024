using AocUtils;
using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public partial class Day1 : Node
{
	[Export]
	TextFileResource Input;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var watchstop = new System.Diagnostics.Stopwatch();
		watchstop.Start();
		var input = Input.GetContent().ParseListOfPairOfInt("\n", "   ");

		var leftValues = new int[input.Length];
		var rightValues = new int[input.Length];
		for (int i = 0; i < input.Length; i++)
		{
			leftValues[i] = input[i].Item1;
			rightValues[i] = input[i].Item2;
		}

		GD.Print("Parsing: " + watchstop.ElapsedTicks / (float)Stopwatch.Frequency * 1000 + "ms");

		GD.Print("Part 1: " + Part1(leftValues, rightValues));
		GD.Print("Part 2: " + Part2(leftValues, rightValues));
		watchstop.Stop();
	}

	private string Part1(int[] leftValues, int[] rightValues)
	{
		var leftValuesList = new List<int>(leftValues);
		var rightValuesList = new List<int>(rightValues);

		leftValuesList.Sort();
		rightValuesList.Sort();

		var pairedList = leftValuesList.Zip(rightValuesList, (l, r) => (l, r)).ToList();
		var shaderInput = pairedList.SelectMany(pair => new float[] { pair.l, pair.r }).ToArray();
		var pairsCount = leftValues.Length;

		var output = RunShaderGraphGOGO(
			"res://scripts/days/day1part1.glsl",
			shaderInput,
			xGroups: (uint)pairsCount
		);

		// GD.Print("Output: ", string.Join(", ", output.Take(pairsCount)));

		var sum = output.Take(pairsCount).Sum();

		return sum.ToString();
	}

	private static float[] RunShaderGraphGOGO(string shadeFile, float[] shaderInput, uint xGroups = 1)
	{
		var rd = RenderingServer.CreateLocalRenderingDevice();

		// Load GLSL shader
		var shaderFile = GD.Load<RDShaderFile>(shadeFile);
		var shaderBytecode = shaderFile.GetSpirV();
		var shader = rd.ShaderCreateFromSpirV(shaderBytecode);

		// Prepare our data. We use floats in the shader, so we need 32 bit.
		// var shaderInput = new float[] { 10, 2, 55, 23, 5, 6, 7, 8, 9, 10 };
		var inputBytes = new byte[shaderInput.Length * sizeof(float)];
		Buffer.BlockCopy(shaderInput, 0, inputBytes, 0, inputBytes.Length);

		// Create a storage buffer that can hold our float values.
		// Each float has 4 bytes (32 bit) so 10 x 4 = 40 bytes
		var buffer = rd.StorageBufferCreate((uint)inputBytes.Length, inputBytes);

		// Create a uniform to assign the buffer to the rendering device
		var uniform = new RDUniform
		{
			UniformType = RenderingDevice.UniformType.StorageBuffer,
			Binding = 0
		};
		uniform.AddId(buffer);
		var uniformSet = rd.UniformSetCreate(new Array<RDUniform> { uniform }, shader, 0);

		// Create a compute pipeline
		var pipeline = rd.ComputePipelineCreate(shader);
		var computeList = rd.ComputeListBegin();
		rd.ComputeListBindComputePipeline(computeList, pipeline);
		rd.ComputeListBindUniformSet(computeList, uniformSet, 0);
		rd.ComputeListDispatch(computeList, xGroups: xGroups, yGroups: 1, zGroups: 1);
		rd.ComputeListEnd();

		// Submit to GPU and wait for sync
		rd.Submit();
		rd.Sync();

		// Read back the data from the buffers
		var outputBytes = rd.BufferGetData(buffer);

		var output = new float[shaderInput.Length];
		Buffer.BlockCopy(outputBytes, 0, output, 0, outputBytes.Length);
		return output;
	}

	private string Part2(int[] leftValues, int[] rightValues)
	{
		var rd = RenderingServer.CreateLocalRenderingDevice();

		// Load GLSL shader
		var shaderFile = GD.Load<RDShaderFile>("res://scripts/days/day1part2.glsl");
		var shaderBytecode = shaderFile.GetSpirV();
		var shader = rd.ShaderCreateFromSpirV(shaderBytecode);

		// Prepare our data. We use floats in the shader, so we need 32 bit.
		var inputBytes1 = new byte[leftValues.Length * sizeof(float)];
		Buffer.BlockCopy(leftValues.Select(x => (float)x).ToArray(), 0, inputBytes1, 0, inputBytes1.Length);

		var inputBytes2 = new byte[rightValues.Length * sizeof(float)];
		Buffer.BlockCopy(rightValues.Select(x => (float)x).ToArray(), 0, inputBytes2, 0, inputBytes2.Length);

		// Create a storage buffer that can hold our float values.
		// Each float has 4 bytes (32 bit) so 10 x 4 = 40 bytes
		var buffer1 = rd.StorageBufferCreate((uint)inputBytes1.Length, inputBytes1);
		var buffer2 = rd.StorageBufferCreate((uint)inputBytes2.Length, inputBytes2);

		var outbuffer = rd.StorageBufferCreate((uint)inputBytes2.Length, null);

		// Create a uniform to assign the buffer to the rendering device
		var uniform1 = new RDUniform
		{
			UniformType = RenderingDevice.UniformType.StorageBuffer,
			Binding = 0
		};
		uniform1.AddId(buffer1);

		var uniform2 = new RDUniform
		{
			UniformType = RenderingDevice.UniformType.StorageBuffer,
			Binding = 1
		};
		uniform2.AddId(buffer2);

		var uniform3 = new RDUniform
		{
			UniformType = RenderingDevice.UniformType.StorageBuffer,
			Binding = 2
		};
		uniform3.AddId(outbuffer);

		var uniformSet = rd.UniformSetCreate(new Array<RDUniform> { uniform1, uniform2, uniform3 }, shader, 0);

		// Create a compute pipeline
		var pipeline = rd.ComputePipelineCreate(shader);
		var computeList = rd.ComputeListBegin();
		rd.ComputeListBindComputePipeline(computeList, pipeline);
		rd.ComputeListBindUniformSet(computeList, uniformSet, 0);
		rd.ComputeListDispatch(computeList, xGroups: (uint)leftValues.Length, yGroups: 1, zGroups: 1);
		rd.ComputeListEnd();

		// Submit to GPU and wait for sync
		rd.Submit();
		rd.Sync();

		// Read back the data from the buffers
		var outputBytes1 = rd.BufferGetData(buffer1);
		var outputBytes2 = rd.BufferGetData(buffer2);
		var outputBytes3 = rd.BufferGetData(outbuffer);

		var output = new float[leftValues.Length];
		Buffer.BlockCopy(outputBytes1, 0, output, 0, outputBytes1.Length);

		var output2 = new float[rightValues.Length];
		Buffer.BlockCopy(outputBytes2, 0, output2, 0, outputBytes2.Length);

		var output3 = new float[rightValues.Length];
		Buffer.BlockCopy(outputBytes3, 0, output3, 0, outputBytes3.Length);

		// GD.Print("Output1: ", string.Join(", ", output));
		// GD.Print("Output2: ", string.Join(", ", output2));
		// GD.Print("Output3: ", string.Join(", ", output3));

		var sum = output3.Select(x => (int)x).Sum();

		// GD.Print("total values count: ", leftValues.Length);

		return sum.ToString();
	}

}
