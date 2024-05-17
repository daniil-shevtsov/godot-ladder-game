using Godot;
using System;
using System.Collections.Generic;

public partial class DebugOverlay : Control
{

	public Dictionary<string, VectorToDraw> vectorsToDraw = new();

	public Camera3D camera;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void UpdateVectorToDraw(string key, Vector3 start, Vector3 end, Color? color = null)
	{
		vectorsToDraw[key] = new VectorToDraw(key, start, end, color);
		QueueRedraw();
	}

	public override void _Draw()
	{
		foreach (var vector in vectorsToDraw.Values)
		{
			var color = new Color(0, 1, 0);

			var start = camera.UnprojectPosition(vector.start);
			var end = camera.UnprojectPosition(vector.end);

			GD.Print($"Drawing {vector.key} line from {start} to {end}");
			DrawLine(start, end, color, 4.0f);
		}
	}
}

public struct VectorToDraw
{
	public string key;
	public Vector3 start;
	public Vector3 end;
	public Color color;

	public VectorToDraw(string key, Vector3 start, Vector3 end, Color? color)
	{
		this.key = key;
		this.start = start;
		this.end = end;
		this.color = color ?? new Color(0, 1, 0);
	}

}


