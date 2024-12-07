using System;
using System.Collections.Generic;
using Godot;

[GlobalClass]
public partial class TextFileResource : Resource
{
	[Export(PropertyHint.File)]
	public string File;

	private string content = null;

	public void LoadFile()
	{
		try
		{
			var file = FileAccess.Open(File, FileAccess.ModeFlags.Read);
			content = file.GetAsText();
		}
		catch (Exception e)
		{
			GD.PrintErr("Error loading text file resource: " + File + " " + e.Message);
		}
	}

	public string GetContent()
	{
		if (content == null)
		{
			LoadFile();
		}
		return content;
	}

	public string RenderTemplate(Dictionary<string, string> variables)
	{
		if (content == null)
		{
			LoadFile();
		}
		string renderedOutput = content;
		foreach (KeyValuePair<string, string> variable in variables)
		{
			renderedOutput = renderedOutput.Replace("{" + variable.Key + "}", variable.Value);
		}
		return renderedOutput;
	}
}